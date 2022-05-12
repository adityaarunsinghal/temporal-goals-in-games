from PlaceAndShootGym import *

GAME_4_SETUP = [[0, 0, -2.600778/CONT_VALUE_SCALE, 3.299083/CONT_VALUE_SCALE, Action.objectTagToActionVal("triangle"), 0],
                [0, 0, -3.832676/CONT_VALUE_SCALE, -0.528815/CONT_VALUE_SCALE, Action.objectTagToActionVal("corner"), 0],
                [0, 0, 0.457935/CONT_VALUE_SCALE, -0.114176/CONT_VALUE_SCALE, Action.objectTagToActionVal("crate"), 0],
                [0, 0, -1.537139/CONT_VALUE_SCALE, 1.340065/CONT_VALUE_SCALE, Action.objectTagToActionVal("gear"), 0],
                [0, 0, 0, 0, 0, 1]]

def GAME_4_REWARD(obsVec: List[Obs]) -> bool:
    
    hits=0
    for each_obs in obsVec:
        if (each_obs.collidedWith in ["corner", "crate", "triangle", "gear"]):
            # reward increases each time
            hits += hits + 1
        
    return hits
    
GAME_4_TRANSFORMER = copy.deepcopy(NO_OBJECT_INTERACTION)


if __name__=="__main__":
    SERVER_BUILD = "/scratch/as11919/temporal-goals-in-games/Builds/Gym_View_12May22_Linux.x86_64"
    channel = EngineConfigurationChannel()
    channel.set_configuration_parameters(time_scale=50, quality_level=0)
    unity_env = UnityEnvironment(
        file_name=SERVER_BUILD, seed=1, side_channels=[channel], worker_id=3)

    # Start interacting with the environment.
    unity_env.reset()
    gym_env = UnityToGymWrapper(unity_env, allow_multiple_obs=False)
    env = PlaceAndShootGym(gym_env, reward_fn=GAME_4_REWARD,
                        actionTransformer=GAME_4_TRANSFORMER,
                        announce_actions=False)

    print("GYM READY")

    env.setup(GAME_4_SETUP)

    step_size = 0.05
    print(f"CHECKING PLAYABILITY AT step_size: {step_size}")
    env.isPlayable(step_size)
    env.save("/scratch/as11919/temporal-goals-in-games/Code/results/GAME_4_SOLVED.joblib")

    print(f"SAVED RUN!")
    env.close()