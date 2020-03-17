# Eden
Simulated agent environment using MLAgents architecture.

The Eden Academy Script trains multiple parallel iterations of an environment where AdamAgent finds apple for EveAgent and obtains 
rewards.

The trained neural net is continuously called on by each agent to find quickest search optimisation.

![training](https://user-images.githubusercontent.com/20610948/76891121-7b45ed80-68ae-11ea-8530-c9ec07473f49.PNG)

Training uses movement input from AdamAgent and couple of other Vector Observations to find cummulative reward through PPO algorithm 
then uses RayPerception3D to acquire visual input to find reward objects.

![training2](https://user-images.githubusercontent.com/20610948/76893269-12607480-68b2-11ea-8fdf-d0ca0e0ef352.PNG)

