from PlaceAndShootGym import *
from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfigurationChannel

# crate in the middle and bucket on floor for bounce and bucket game
GAME_2_SETUP = [[0, 0, 0, 0.8, Action.objectTagToActionVal("crate"), 0],
                [0, 0, -0.85, -0.9, Action.objectTagToActionVal("bucket"), 0],
                [0, 0, 0, 0, 0, 1]]


def GAME_2_REWARD(obsVec: List[Obs]) -> bool:
    """
    Custom Reward Fn:
    hits crate and goes in bucket
    """
    hitCrate = False
    for each_obs in obsVec:
        if each_obs.collidedWith == "crate":
            hitCrate = True
            break
    return hitCrate and endsInBucket(obsVec)

# no setting up on bucket!
GAME_2_TRANSFORMER = copy.deepcopy(NO_OBJECT_INTERACTION)
GAME_2_TRANSFORMER.ban_mouse_position_x = (-1, -0.8)

print("MADE ALL THE CLASSES")

SERVER_BUILD = "../Builds/Gym_View_25April22_server.app"
channel = EngineConfigurationChannel()
channel.set_configuration_parameters(time_scale=50, quality_level=0)
unity_env = UnityEnvironment(
    file_name=SERVER_BUILD, seed=1, side_channels=[channel], worker_id=2)

# Start interacting with the environment.
unity_env.reset()
gym_env = UnityToGymWrapper(unity_env, allow_multiple_obs=False)
env = PlaceAndShootGym(gym_env, reward_fn=GAME_2_REWARD,
                       actionTransformer=GAME_2_TRANSFORMER,
                       announce_actions=True)

print("GYM READY")

env.setup(GAME_2_SETUP)

step_size = 0.1
print(f"CHECKING PLAYABILITY AT step_size: {step_size}")
env.isPlayable(step_size)
env.save("results/GAME_2_SOLVED.joblib")

print(f"SAVED RUN!")