from PlaceAndShootGym import *
import random

GAME_NULL_SETUP = []
num_objs_to_place = random.randint(0, 5)
for each in range(num_objs_to_place):
    GAME_NULL_SETUP.append([0, 0, random.random()*random.choice([-1, 1]), random.random()*random.choice([-1, 1]),
                            Action.objectTagToActionVal(random.choice(objectOrder)), 0])
GAME_NULL_SETUP.append([0, 0, 0, 0, 0, 1])
for each in GAME_NULL_SETUP:
    print(Action(each))

x_limit = random.randint(-100, 80)/100.0
y_limit = random.randint(-100, 80)/100.0
required_percentage = random.randint(0, 30)/100.0
print("x_range = ", x_limit, x_limit+0.2)
print("y_range = ", y_limit, y_limit+0.2)
print("required_percent_of_shots_in_chosen_zone = ", required_percentage)

def GAME_NULL_REWARD(obsVec: List[Obs]) -> bool:
    """
    lands and rests on top of the made up platform on the corner
    """
    count = 0
    for any_obs in obsVec:
        if x_limit<=any_obs.ballPos.x<=x_limit+0.2 and y_limit<=any_obs.ballPos.y<=y_limit+0.2:
            count += 1
    percentage = count/len(obsVec)
    return percentage>required_percentage


GAME_NULL_TRANSFORMER = copy.deepcopy(NO_OBJECT_INTERACTION)


if __name__=="__main__":
    SERVER_BUILD = "/scratch/as11919/temporal-goals-in-games/Builds/Gym_View_26April22_Linux.x86_64"
    channel = EngineConfigurationChannel()
    channel.set_configuration_parameters(time_scale=50, quality_level=0)
    unity_env = UnityEnvironment(
        file_name=SERVER_BUILD, seed=1, side_channels=[channel], worker_id=3)

    # Start interacting with the environment.
    unity_env.reset()
    gym_env = UnityToGymWrapper(unity_env, allow_multiple_obs=False)
    env = PlaceAndShootGym(gym_env, reward_fn=GAME_NULL_REWARD,
                        actionTransformer=GAME_NULL_TRANSFORMER,
                        announce_actions=False)

    print("GYM READY")

    env.setup(GAME_NULL_SETUP)

    step_size = 0.05
    print(f"CHECKING PLAYABILITY AT step_size: {step_size}")
    env.isPlayable(step_size)
    env.save("/scratch/as11919/temporal-goals-in-games/Code/results/GAME_NULL_SOLVED.joblib")

    print(f"SAVED RUN!")
    env.close()