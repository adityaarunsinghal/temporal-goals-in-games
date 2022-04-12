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

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (actions.ContinuousActions[0]!=0f && actions.ContinuousActions[1]!=0f)
        {
            Vector3 placeOrShootBall = new Vector3(actions.ContinuousActions[0], actions.ContinuousActions[1], 0);
            ball_object.GetComponent<AgentDragDrop5>().artificialBallInteraction(placeOrShootBall);

        }

        if (actions.DiscreteActions[0]!=-1)
        {
            GameObject objectToMove = foundObjects[actions.DiscreteActions[0]].gameObject;
            Vector3 placeObject = new Vector3(actions.ContinuousActions[2], actions.ContinuousActions[3], 0);
            objectToMove.GetComponent<AgentDragDrop>().setPosition(placeObject);
        }

        UnityEngine.Debug.Log((string) actionsToString(actions));

        // Vector3 ballShotFrom;
        // if (ActivityLogger.getLatestBallPosition()!=null)
        // {
        //     ballShotFrom = (Vector3) ActivityLogger.getLatestBallPosition();
        // }

        if (ball_object.transform.position == new Vector3(0, 0, 0))
        {
            UnityEngine.Debug.Log("Ball is at 0, 0, 0");
            SetReward(1f);
            EndEpisode();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        int objectsCount = ActivityLogger.getObjectsCount();
        Vector3[] positions = ActivityLogger.getLatestObjectPositions();
        for (int objectNum = 0; objectNum < objectsCount; objectNum++)
        {
            sensor.AddObservation(positions[objectNum]);
            // UnityEngine.Debug.Log(objectNum);
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
}
