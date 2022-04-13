using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class dragAndShootAgent : Agent
{
    private DragDrop[] foundObjects;
    private GameObject ball_object;
    private GameObject bucket_object;
    private GameObject wall_object;
    private GameObject bottom_wall_object;
    private int numActionsTaken;
    public bool saveLogs = false;
    public float contValueScale = 1000f;

    public void Start()
    {
        ActivityLogger.saveMode = saveLogs;
        foundObjects = ActivityLogger.getFoundObjects();
        ball_object = GameObject.FindGameObjectsWithTag("ball")[0];
        bucket_object = GameObject.FindGameObjectsWithTag("bucket")[0];
        wall_object = GameObject.FindGameObjectsWithTag("wall")[0];
        bottom_wall_object = GameObject.FindGameObjectsWithTag("bottomWall")[0];
    }

    public override void OnEpisodeBegin()
    {
        numActionsTaken = 0;

        if (DestroyCounter.destroyedCount > 0)
        {
            FreshStart.OnButtonPress();
        }
        else
        {
            FreshStart.softReset();
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // abstain from placing or shooting if place value is 1 in X or Y
        if (actions.ContinuousActions[0] != 1f | actions.ContinuousActions[1] != 1f)
        {
            Vector3 placeBall = new Vector3(actions.ContinuousActions[0] * contValueScale,
                                                    actions.ContinuousActions[1] * contValueScale, 0);
            ball_object.GetComponent<AgentDragDrop5>().artificialBallInteraction(placeBall);

            // abstain from shooting if shoot value is 1 in X or Y
            if (actions.ContinuousActions[2] != 1f | actions.ContinuousActions[3] != 1f)
            {
                Vector3 shootBall = new Vector3(actions.ContinuousActions[2] * contValueScale,
                                                        actions.ContinuousActions[3] * contValueScale, 0);
                ball_object.GetComponent<AgentDragDrop5>().artificialBallInteraction(shootBall);
            }

            numActionsTaken++;
        }

        // abstain from moving objects if 5
        if (actions.DiscreteActions[0] != 5)
        {
            GameObject objectToMove = foundObjects[actions.DiscreteActions[0]].gameObject;
            Vector3 placeObject = new Vector3(actions.ContinuousActions[4] * contValueScale,
                                                    actions.ContinuousActions[5] * contValueScale, 0);
            objectToMove.GetComponent<AgentDragDrop>().setPosition(placeObject);
            numActionsTaken++;
        }

        if (actions.DiscreteActions[1] == 1)
        {
            // agent tried to setup
            Retry.OnButtonPress();
        }

        if (wall_object.GetComponent<FlagCollision>().collidedWith == ball_object & 
        wall_object.GetComponent<FlagCollision>().childCollider == bottom_wall_object)
        {
            giveRewards();
            UnityEngine.Debug.Log("Episode Ended!");
            wall_object.GetComponent<FlagCollision>().reset();
            EndEpisode();
        }

        UnityEngine.Debug.Log((string)actionsToString(actions));
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        string latestNote = ActivityLogger.getLatestNote();
        if (latestNote != null)
        {
            string[] inputs = latestNote.Split(' ');
            float[] cont = new float[6];
            for (int i = 0; i <= 5; i++)
            {
                cont[i] = float.Parse(inputs[i]);
            }

            continuousActions[0] = cont[0];
            continuousActions[1] = cont[1];
            continuousActions[2] = cont[2];
            continuousActions[3] = cont[3];
            continuousActions[4] = cont[4];
            continuousActions[5] = cont[5];
            discreteActions[0] = int.Parse(inputs[6]);
            discreteActions[1] = int.Parse(inputs[7]);
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        int objectsCount = ActivityLogger.getObjectsCount();
        Vector3[] positions = ActivityLogger.getLatestObjectPositions();
        for (int objectNum = 0; objectNum < objectsCount; objectNum++)
        {
            sensor.AddObservation(positions[objectNum]);
        }
        sensor.AddObservation(ball_object.transform.position);
    }

    private string actionsToString(ActionBuffers actions)
    {
        string ret = "Continuous Actions: ";
        foreach (float x in actions.ContinuousActions)
        {
            ret += (Mathf.Round(x * 1000) * 0.001).ToString() + " ";
        }
        ret += ":: Discrete Actions: ";
        foreach (int x in actions.DiscreteActions)
        {
            ret += x.ToString() + " ";
        }

        return ret;
    }

    public void giveRewards()
    {
        float reward = 0f;

        // arbitrary reward function for now: close to 0, 0 vector
        float x2 = ball_object.transform.position[0] * ball_object.transform.position[0];
        float y2 = ball_object.transform.position[1] * ball_object.transform.position[1];
        float dist = Mathf.Sqrt(x2 + y2);
        reward -= dist;

        // try to be close to bucket
        float x = ball_object.transform.position[0] - bucket_object.transform.position[0];
        float y = ball_object.transform.position[1] - bucket_object.transform.position[1];
        x2 = x*x;
        y2 = y*y;
        dist = Mathf.Sqrt(x2 + y2);
        reward += dist;

        // get there as quickly as possible
        // reward -= numActionsTaken;

        if (DestroyCounter.destroyedCount > 0)
        {
            // penalize destruction
            reward -= DestroyCounter.destroyedCount;
            UnityEngine.Debug.Log(string.Format("Destroyed {0} objects", DestroyCounter.destroyedCount));
        }
        SetReward(reward);
        UnityEngine.Debug.Log(string.Format("Got {0} Reward", reward));
    }
}
