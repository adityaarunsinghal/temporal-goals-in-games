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
            Vector3 placeOrShootBall = new Vector3(actions.ContinuousActions[0], actions.ContinuousActions[1], 0);
            ball_object.GetComponent<AgentDragDrop5>().artificialBallInteraction(placeOrShootBall);
        }

        if (actions.DiscreteActions[0] != -1)
        {
            GameObject objectToMove = foundObjects[actions.DiscreteActions[0]].gameObject;
            Vector3 placeObject = new Vector3(actions.ContinuousActions[2], actions.ContinuousActions[3], 0);
            objectToMove.GetComponent<AgentDragDrop>().setPosition(placeObject);
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
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
        continuousActions[2] = Input.GetAxisRaw("Horizontal2");
        continuousActions[3] = Input.GetAxisRaw("Vertical2");
        KeyCode[] keyCodes = {
                            KeyCode.Alpha0,
                            KeyCode.Alpha1,
                            KeyCode.Alpha2,
                            KeyCode.Alpha3,
                            KeyCode.Alpha4,
                        };
        int numberPressed = -1;
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                numberPressed = i;
            }
        }
        UnityEngine.Debug.Log(numberPressed);
        discreteActions[0] = numberPressed;
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
            ret += x.ToString() + " ";
        }
        ret += "\nDiscrete Actions: ";
        foreach (int x in actions.DiscreteActions)
        {
            ret += x.ToString() + " ";
        }
        ret += "\n\n";

        return ret;
    }

    public void giveRewards()
    {
        float reward = 0f;
        // arbitrary reward function for now
        if (ball_object.transform.position == new Vector3(0, 0, 0))
        {
            UnityEngine.Debug.Log("Ball is at 0, 0, 0");
            reward += 5f;

            // penalize destruction
            reward -= DestroyCounter.destroyedCount;
            SetReward(reward);
            EndEpisode();
        }
    }
}
