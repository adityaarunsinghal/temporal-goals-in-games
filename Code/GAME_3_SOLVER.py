from PlaceAndShootGym import *
from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfigurationChannel

GAME_3_SETUP = [[0, 0, 0.96, 0.6, Action.objectTagToActionVal("corner"), 0],
                [0, 0, 0.80, 0.575, Action.objectTagToActionVal("crate"), 0],
                [0, 0, 0, 0, 0, 1]]


def GAME_3_REWARD(obsVec: List[Obs]) -> bool:
    """
    lands and rests on top of the made up platform on the corner
    """
    ballPos = obsVec[-1].ballPos
    ballVel = obsVec[-1].ballVel
    corner = obsVec[-1].objPos["corner"]
    crate = obsVec[-1].objPos["crate"]

    if ballPos.y>corner.y or ballPos.y>crate.y:
        if ballVel.x<=VEL_THRESHOLD and ballVel.y<=VEL_THRESHOLD:
            return True
    return False


GAME_3_TRANSFORMER = copy.deepcopy(NO_OBJECT_INTERACTION)
GAME_3_TRANSFORMER

print("MADE ALL THE CLASSES")

SERVER_BUILD = "/scratch/as11919/temporal-goals-in-games/Builds/Gym_View_26April22_Linux.x86_64"
channel = EngineConfigurationChannel()
channel.set_configuration_parameters(time_scale=50, quality_level=0)
unity_env = UnityEnvironment(
    file_name=SERVER_BUILD, seed=1, side_channels=[channel], worker_id=3)

# Start interacting with the environment.
unity_env.reset()
gym_env = UnityToGymWrapper(unity_env, allow_multiple_obs=False)
env = PlaceAndShootGym(gym_env, reward_fn=GAME_3_REWARD,
                       actionTransformer=GAME_3_TRANSFORMER,
                       announce_actions=False)

print("GYM READY")

env.setup(GAME_3_SETUP)

step_size = 0.05
print(f"CHECKING PLAYABILITY AT step_size: {step_size}")
env.isPlayable(step_size)
env.save("/scratch/as11919/temporal-goals-in-games/Code/results/GAME_3_SOLVED.joblib")

print(f"SAVED RUN!")
env.close()