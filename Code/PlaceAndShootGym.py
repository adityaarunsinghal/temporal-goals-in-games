import copy
import json
import joblib
import numpy as np
from tqdm import tqdm
from typing import List
from collections import namedtuple
from gym_unity.envs import UnityToGymWrapper
from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfigurationChannel

Vector2 = namedtuple('Vector2', 'x y')

objectOrder = ["bucket", "corner", "crate", "gear", "triangle"]
colliderOrder = ["bottomWall", "bucket", "corner", "crate", "gear",
                 "leftWall", "pedestal", "rightWall", "topWall", "triangle"]

VEL_THRESHOLD = 0.001

class Obs():
    def __init__(self, raw_obs):
        """
        Converts Unity Agent outputted Vector Observation to 
        named format
        """
        self.raw_obs = raw_obs
        self.objPos = {}
        self.objPos[objectOrder[0]] = Vector2(raw_obs[0], raw_obs[1])
        self.objPos[objectOrder[1]] = Vector2(raw_obs[2], raw_obs[3])
        self.objPos[objectOrder[2]] = Vector2(raw_obs[4], raw_obs[5])
        self.objPos[objectOrder[3]] = Vector2(raw_obs[6], raw_obs[7])
        self.objPos[objectOrder[4]] = Vector2(raw_obs[8], raw_obs[9])
        self.ballPos = Vector2(raw_obs[10], raw_obs[11])
        self.ballVel = Vector2(raw_obs[12], raw_obs[13])
        self.rawColliderVal = raw_obs[14]
        self.colliderIdx = int(self.rawColliderVal)
        if self.colliderIdx >= 0:
            self.collidedWith = colliderOrder[self.colliderIdx]
        else:
            self.collidedWith = None
        self.reset = bool(raw_obs[15])

    def __str__(self) -> str:
        """
        Pretty Print Observation
        """
        s = ""
        for each_obj in self.objPos:
            s += f"{each_obj}: {self.objPos[each_obj]}\n"
        s += f"Ball Position: {self.ballPos}\n"
        s += f"Ball Velocity: {self.ballVel}\n"
        s += f"Collided With: {self.collidedWith}\n"
        s += f"In Reset?: {self.reset}\n"
        return s

    def __repr__(self):
        return str(self.toArray())

    def toArray(self):
        return self.raw_obs


class Action():
    def __init__(self, raw_action=[0, 0, 0, 0, 0, 0], force=False):

        self.raw_action = raw_action
        self.mouseX = raw_action[0]
        self.mouseY = raw_action[1]
        self.objX = raw_action[2]
        self.objY = raw_action[3]
        self.rawObjVal = raw_action[4]
        self.objIdx = self.mapActionValToDiscreteIdx(self.rawObjVal)
        if self.objIdx >= 0:
            self.objName = objectOrder[self.objIdx]
        else:
            self.objName = None
        self.reset = bool(raw_action[5])
        self.force = force
        self.transformed = False

    def __str__(self):
        return str([round(self.mouseX, 3), round(self.mouseY, 3), round(self.objX, 3), round(self.objY, 3), self.objName, self.reset])

    def __repr__(self):
        return str(self.toArray())

    def isEmpty(self):
        return sum(self.toArray(raw=False)) == 0.0

    def toArray(self, raw=True):
        if raw:
            return [self.mouseX, self.mouseY, self.objX, self.objY, self.rawObjVal, int(self.reset)]
        else:
            return [self.mouseX, self.mouseY, self.objX, self.objY, self.objIdx, int(self.reset)]

    def setObject(self, name):
        self.rawObjVal = self.objectTagToActionVal(name)
        self.objIdx = self.objectTagToActionVal(name)
        self.objName = name

    @staticmethod
    def mapActionValToDiscreteIdx(value):
        assert -1 <= value <= 1
        value = abs(value)
        value *= 5.49
        value = round(value)
        value = value - 1
        return value

    @staticmethod
    def objectTagToActionVal(object):
        idx = objectOrder.index(object)
        # 0 is abstain
        idx = idx + 1
        return idx/5.49


class ActionTransformer():
    def __init__(self, ban_object=[], ban_mouse_position_x=(99, 999), ban_mouse_position_y=(99, 999),
                 ban_object_position_x=(99, 999), ban_object_position_y=(99, 999), default_action=Action()):

        self.ban_object = ban_object
        assert ban_mouse_position_x[1] > ban_mouse_position_x[0]
        self.ban_mouse_position_x = ban_mouse_position_x
        assert ban_mouse_position_y[1] > ban_mouse_position_y[0]
        self.ban_mouse_position_y = ban_mouse_position_y
        assert ban_object_position_x[1] > ban_object_position_x[0]
        self.ban_object_position_x = ban_object_position_x
        assert ban_object_position_y[1] > ban_object_position_y[0]
        self.ban_object_position_y = ban_object_position_y
        self.default_action = default_action.toArray()

    def transform(self, action: Action):
        if action.force:
            return action
        else:
            default_action = Action(self.default_action)
            default_action.transformed = True
            if action.objName in self.ban_object:
                return default_action
            if self.ban_mouse_position_x[0] <= action.mouseX <= self.ban_mouse_position_x[1]:
                return default_action
            if self.ban_mouse_position_y[0] <= action.mouseY <= self.ban_mouse_position_y[1]:
                return default_action
            if self.ban_object_position_x[0] <= action.objX <= self.ban_object_position_x[1]:
                return default_action
            if self.ban_object_position_y[0] <= action.objY <= self.ban_object_position_y[1]:
                return default_action
        action.transformed = True
        return action

    def __repr__(self):
        return json.dumps(vars(self))

    def __str__(self):
        return self.__repr__()


class PlaceAndShootGym(UnityToGymWrapper):
    def __init__(self, gym_env, reward_fn, actionTransformer=ActionTransformer(), 
                announce_actions=True):
        self.gym_env = gym_env
        self.reward_fn = reward_fn
        self.actionTransformer = actionTransformer
        # unsure if this is always true
        self.velTresh = VEL_THRESHOLD
        self.lastObsVec = None
        self.announce_actions = announce_actions
        self.winning_shots = []
        self.setup_array = [[0,0,0,0,0,1]]

    def step(self, action, allow_empty=True, quiet=False):
        """
        Step is defined as doing something ball has stopped
        """
        if type(action) != Action:
            action = Action(action)

        if not action.transformed:
            action = self.actionTransformer.transform(action)
        if action.isEmpty() and not allow_empty:
            return (None, None, None, None)

        obsVec = []

        # first step
        if self.announce_actions and not quiet:
            print(action)
        raw_obs, _reward, done, info = self.gym_env.step(action.toArray())
        obsVec.append(Obs(raw_obs))

        # continued steps
        while (any([abs(f) > self.velTresh for f in obsVec[-1].ballVel])):
            raw_obs, _reward, done, info = self.gym_env.step(action.toArray())
            obsVec.append(Obs(raw_obs))
        reward = self.getRewards(obsVec)

        self.lastObsVec = obsVec
        return (obsVec[-1].toArray(), reward, done, info)

    def setup(self, actionVec, checkWithTransformer=False) -> bool:
        """
        Setup steps must be a sequence of actions that end with a reset of the ball
        """
        assert actionVec[-1][-1] == 1
        self.setup_array = actionVec
        for each_raw_action in actionVec:
            if checkWithTransformer:
                each_action = self.actionTransformer.transform(
                    Action(each_raw_action))
            else:
                each_action = Action(each_raw_action)
            self.gym_env.step(each_action.toArray())

    def getRewards(self, obsVec: List[Obs]) -> float:
        return float(self.reward_fn(obsVec))

    def isPlayable(self, step_size=0.1):
        winning_shots = []
        all_tries = set()
        for place_mouse_x in tqdm(np.arange(-1.0, 1.0, step_size)):
            for shoot_mouse_x in np.arange(-1.0, 1.0, step_size):
                for shoot_mouse_y in np.arange(-1.0, 1.0, step_size):

                    place_action = self.actionTransformer.transform(
                        Action([place_mouse_x, 0, 0, 0, 0, 0]))
                    shoot_action = self.actionTransformer.transform(
                        Action([shoot_mouse_x, shoot_mouse_y, 0, 0, 0, 0], force=True))

                    action_pair = (tuple(place_action.toArray()),
                                   tuple(shoot_action.toArray()))
                    if action_pair in all_tries:
                        continue
                    all_tries.add(action_pair)

                    # reset
                    self.step([0, 0, 0, 0, 0, 1], quiet=True)

                    # place pedestal
                    if self.announce_actions:
                        print("Place:")
                    raw_obs, reward, _done, _info = self.step(
                        place_action, allow_empty=False)

                    # shoot ball
                    if self.announce_actions:
                        print("Shoot:")
                    # force True because shooting should be allowed to utilize those banned float values
                    raw_obs, reward, _done, _info = self.step(
                        shoot_action, allow_empty=False)

                    if reward and reward > 0:
                        print(f"Game is playable!")
                        winning_shots.append({"place_action": action_pair[0],
                                              "shoot_action": action_pair[1],
                                              "reward": reward})

        self.winning_shots = winning_shots
        self.num_tries = len(all_tries)
        return self.winning_shots

    def reset(self):
        self.gym_env.reset()

    def close(self):
        self.gym_env.close()

    def save(self, path):
        temp = self.gym_env
        self.gym_env = None
        joblib.dump(self, path)
        self.gym_env = temp


def endsInBucket(obsVec: List[Obs]) -> bool:
    """
    Custom Reward Fn:
    Is that ball in bucket at the end or no?
    """
    MIN_X_DELTA = -0.1927506923675537
    MAX_X_DELTA = 0.2523689270019531
    MIN_Y_DELTA = -0.24334418773651123
    MAX_Y_DELTA = 0.6142134666442871

    ball_x, ball_y = obsVec[-1].ballPos
    bucket_x, bucket_y = obsVec[-1].objPos["bucket"]
    x_delta = ball_x - bucket_x
    y_delta = ball_y - bucket_y

    return (MAX_X_DELTA >= x_delta >= MIN_X_DELTA) and (MAX_Y_DELTA >= y_delta >= MIN_Y_DELTA)


NO_OBJECT_INTERACTION = ActionTransformer(
    ban_object=["crate", "bucket", "corner", "gear", "triangle"], default_action=Action([-0.001, 0, 0, 0, 0, 0]))
