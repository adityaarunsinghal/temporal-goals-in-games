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

    public float contValueScale = 10f;
    void Start()
    {
        foundObjects = ActivityLogger.getFoundObjects();
        ball_object = GameObject.FindGameObjectsWithTag("ball")[0];
    }

    public override void OnEpisodeBegin()
    {
        FreshStart.softReset();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (actions.ContinuousActions[0] != 9999f | actions.ContinuousActions[1] != 9999f)
        {
            Vector3 placeOrShootBall = new Vector3(actions.ContinuousActions[0] * contValueScale,
                                                    actions.ContinuousActions[1] * contValueScale, 0);
            ball_object.GetComponent<AgentDragDrop5>().artificialBallInteraction(placeOrShootBall);
        }

        if (actions.DiscreteActions[0] != -1)
        {
            GameObject objectToMove = foundObjects[actions.DiscreteActions[0]].gameObject;
            Vector3 placeObject = new Vector3(actions.ContinuousActions[2] * contValueScale,
                                                    actions.ContinuousActions[3] * contValueScale, 0);
            objectToMove.GetComponent<AgentDragDrop>().setPosition(placeObject);
        }

        if (actions.DiscreteActions[1] == 1)
        {
            Retry.OnButtonPress();
            EndEpisode();
        }

        UnityEngine.Debug.Log((string)actionsToString(actions));

        // Vector3 ballShotFrom;
        // if (ActivityLogger.getLatestBallPosition()!=null)
        // {
        //     ballShotFrom = (Vector3) ActivityLogger.getLatestBallPosition();
        // }

        giveRewards();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        string latestNote = ActivityLogger.getLatestNote();
        if (latestNote != null)
        {
            string[] inputs = latestNote.Split(' ');
            float[] cont = new float[4];
            for (int i = 0; i < 4; i++)
            {
                cont[i] = float.Parse(inputs[i]);
            }

            continuousActions[0] = cont[0];
            continuousActions[1] = cont[1];
            continuousActions[2] = cont[2];
            continuousActions[3] = cont[3];
            discreteActions[0] = int.Parse(inputs[4]);
            discreteActions[1] = int.Parse(inputs[5]);
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
    }

    private string actionsToString(ActionBuffers actions)
    {
        string ret = "Continuous Actions: ";
        foreach (float x in actions.ContinuousActions)
        {
            ret += (x * contValueScale).ToString() + " ";
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
        // arbitrary reward function for now
        if (ball_object.transform.position == new Vector3(0, 0, 0))
        {
            reward += 5f;
            UnityEngine.Debug.Log("Got +5 Reward!");
        }

        if (DestroyCounter.destroyedCount > 0)
        {
            // penalize destruction
            reward -= DestroyCounter.destroyedCount;
            UnityEngine.Debug.Log(string.Format("Destroyed {0} objects", DestroyCounter.destroyedCount));
        }
        SetReward(reward);
    }
}
