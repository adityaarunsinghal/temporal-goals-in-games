from PlaceAndShootGym import *

GAME_1_SETUP = [[0, 0, -0.55, 0.6, Action.objectTagToActionVal("triangle"), 0],
                [0, 0, -0.55, 0.1, Action.objectTagToActionVal("corner"), 0],
                [0, 0, -0.55, -0.4, Action.objectTagToActionVal("crate"), 0],
                [0, 0, -0.85, -0.91, Action.objectTagToActionVal("bucket"), 0],
                [0, 0, 0, 0, 0, 1]]

def GAME_1_REWARD(obsVec: List[Obs]) -> int:
    # nothing matters unless ends in bucket
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
        # only check y when x is in line with all objects
        if pos.x != -0.55:
            pass
        # return highest value first if true
        if top(pos, each_obs.objPos["triangle"]): return 4
        if mid1(pos, each_obs.objPos["corner"], each_obs.objPos["triangle"]): return 3
        if mid2(pos, each_obs.objPos["crate"], each_obs.objPos["corner"]): return 2
        if bottom(pos, each_obs.objPos["crate"]): return 1
    return 0

GAME_1_TRANSFORMER = copy.deepcopy(NO_OBJECT_INTERACTION)
GAME_1_TRANSFORMER.ban_mouse_position_x = (-1, -0.5)

if __name__=="__main__":
    SERVER_BUILD = "/scratch/as11919/temporal-goals-in-games/Builds/Gym_View_26April22_Linux.x86_64"
    channel = EngineConfigurationChannel()
    channel.set_configuration_parameters(time_scale=50, quality_level=0)
    unity_env = UnityEnvironment(
        file_name=SERVER_BUILD, seed=1, side_channels=[channel], worker_id=1)

    # Start interacting with the environment.
    unity_env.reset()
    gym_env = UnityToGymWrapper(unity_env, allow_multiple_obs=False)
    env = PlaceAndShootGym(gym_env, reward_fn=GAME_1_REWARD,
                        actionTransformer=GAME_1_TRANSFORMER,
                        announce_actions=False)

    print("GYM READY")

    env.setup(GAME_1_SETUP)

    step_size = 0.05
    print(f"CHECKING PLAYABILITY AT step_size: {step_size}")
    env.isPlayable(step_size)
    env.save("/scratch/as11919/temporal-goals-in-games/Code/results/GAME_1_SOLVED.joblib")

    print(f"SAVED RUN!")
    env.close()