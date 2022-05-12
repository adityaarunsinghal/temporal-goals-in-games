from PlaceAndShootGym import *

GAME_5_SETUP = [[0, 0, -0.85, -0.9, Action.objectTagToActionVal("bucket"), 0],
                [0, 0, 0, 0, 0, 1]]

def GAME_5_REWARD(obsVec: List[Obs]) -> bool:
    if not endsInBucket(obsVec):
        return False
    hitLeft = False
    hitRight = False
    hitTop = False
    for each_obs in obsVec:
        if each_obs.collidedWith == "leftWall":
            hitLeft = True
        elif each_obs.collidedWith == "rightWall":
            hitRight = True
        elif each_obs.collidedWith == "topWall":
            hitTop = True
        
    return hitTop and hitRight and hitLeft
    
GAME_5_TRANSFORMER = copy.deepcopy(NO_OBJECT_INTERACTION)
GAME_5_TRANSFORMER.ban_mouse_position_x = (-1, -0.34)

if __name__=="__main__":
    SERVER_BUILD = "/Users/aditya/Documents/GitHub/game_creation_research/Object Physics Sandbox/Builds/Gym_View_12May22_Linux.x86_64"
    channel = EngineConfigurationChannel()
    channel.set_configuration_parameters(time_scale=50, quality_level=0)
    unity_env = UnityEnvironment(
        file_name=SERVER_BUILD, seed=1, side_channels=[channel], worker_id=3)

    # Start interacting with the environment.
    unity_env.reset()
    gym_env = UnityToGymWrapper(unity_env, allow_multiple_obs=False)
    env = PlaceAndShootGym(gym_env, reward_fn=GAME_5_REWARD,
                        actionTransformer=GAME_5_TRANSFORMER,
                        announce_actions=False)

    print("GYM READY")

    env.setup(GAME_5_SETUP)

    step_size = 0.05
    print(f"CHECKING PLAYABILITY AT step_size: {step_size}")
    env.isPlayable(step_size)
    env.save("/scratch/as11919/temporal-goals-in-games/Code/results/GAME_5_SOLVED.joblib")

    print(f"SAVED RUN!")
    env.close()