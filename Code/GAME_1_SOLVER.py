from PlaceAndShootGym import *
from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfigurationChannel

GAME_1_SETUP = [[0, 0, -0.55, 0.6, Action.objectTagToActionVal("triangle"), 0],
                [0, 0, -0.55, 0.1, Action.objectTagToActionVal("corner"), 0],
                [0, 0, -0.55, -0.4, Action.objectTagToActionVal("crate"), 0],
                [0, 0, -0.85, -0.91, Action.objectTagToActionVal("bucket"), 0],
                [0, 0, 0, 0, 0, 1]]


def GAME_1_REWARD(obsVec: List[Obs]) -> int:
    if not endsInBucket(obsVec):
        return 0
    def top(pos, triangle):
        return pos.y>triangle.y
    def mid1(pos, corner, triangle):
        return pos.y>corner.y and pos.y<triangle.y
    def mid2(pos, crate, corner):
        return pos.y>crate.y and pos.y<corner.y
    def bottom(pos, crate):
        return pos.y<crate.y
    for each_obs in obsVec:
        pos = each_obs.ballPos
        if bottom(pos, each_obs.objPos["crate"]): return 4
        if mid2(pos, each_obs.objPos["crate"], each_obs.objPos["corner"]): return 3
        if mid1(pos, each_obs.objPos["corner"], each_obs.objPos["triangle"]): return 2
        if top(pos, each_obs.objPos["triangle"]): return 1
    return 0


GAME_1_TRANSFORMER = copy.deepcopy(NO_OBJECT_INTERACTION)
GAME_1_TRANSFORMER.ban_mouse_position_x = (-1, -0.5)

print("MADE ALL THE CLASSES")

SERVER_BUILD = "../Builds/Gym_View_25April22_server.app"
channel = EngineConfigurationChannel()
channel.set_configuration_parameters(time_scale=50, quality_level=0)
unity_env = UnityEnvironment(
    file_name=SERVER_BUILD, seed=1, side_channels=[channel], worker_id=1)

# Start interacting with the environment.
unity_env.reset()
gym_env = UnityToGymWrapper(unity_env, allow_multiple_obs=False)
env = PlaceAndShootGym(gym_env, reward_fn=GAME_1_REWARD,
                       actionTransformer=GAME_1_TRANSFORMER,
                       announce_actions=True)

print("GYM READY")

env.setup(GAME_1_SETUP)

step_size = 0.1
print(f"CHECKING PLAYABILITY AT step_size: {step_size}")
env.isPlayable(step_size)
env.save("results/GAME_1_SOLVED.joblib")

print(f"SAVED RUN!")
