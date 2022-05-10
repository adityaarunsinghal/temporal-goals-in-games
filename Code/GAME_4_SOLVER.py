from PlaceAndShootGym import *

GAME_4_SETUP = [[0, 0, -2.600778, 3.299083, Action.objectTagToActionVal("triangle"), 0],
                [0, 0, -3.832676, -0.528815, Action.objectTagToActionVal("corner"), 0],
                [0, 0, 0.457935, -0.114176, Action.objectTagToActionVal("crate"), 0],
                [0, 0, -1.537139, 1.340065, Action.objectTagToActionVal("gear"), 0],
                [0, 0, 0, 0, 0, 1]]

def GAME_4_REWARD(obsVec: List[Obs]) -> bool:
    """
    Custom Reward Fn:
    call it a win if we hit 3 objects prior to hitting the ground. 
    """
    
    hits=0
    for each_obs in obsVec:
        if (each_obs.collidedWith == "corner") or (each_obs.collidedWith == "crate") or (each_obs.collidedWith == "gear") or (each_obs.collidedWith == "triangle" ):
            hits+=1
        
    return hits>3
    
GAME_4_TRANSFORMER = copy.deepcopy(NO_OBJECT_INTERACTION)


if __name__=="__main__":
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