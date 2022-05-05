import os
from GAME_1_SOLVER import *
from stable_baselines import PPO2
from stable_baselines.bench.monitor import Monitor
from stable_baselines.common.callbacks import CheckpointCallback
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfigurationChannel
from mlagents_envs.environment import UnityEnvironment

import warnings
warnings.filterwarnings("ignore")


if __name__ == "__main__":
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
                           announce_actions=True)

    print("GYM READY")

    env.setup(GAME_1_SETUP)

    run_name = "/scratch/as11919/temporal-goals-in-games/Code/results/GAME_1/PPO2_GAME_1"
    monitored_env = Monitor(env = env, filename= run_name, allow_early_resets = True)
    ppo = PPO2(policy = 'MlpPolicy', env = monitored_env, tensorboard_log=os.path.join(os.path.dirname(run_name), "tensorboard/"))
    checkpoint_callback = CheckpointCallback(save_freq=50000, save_path=os.path.join(os.path.dirname(run_name), "checkpoints/"),
                                            name_prefix='rl_model')
    model = ppo.learn(total_timesteps=1000000, callback=checkpoint_callback)
    model.save(run_name)

    print(f"SAVED RUN!")
    env.close()
