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
    private int numActionsTaken;
    public bool saveLogs = false;
    public float contValueScale;

    public void Start()
    {
        AgentStatus.active = true;
        ActivityLogger.saveAllBallPos = true;
        UnityEngine.Debug.Log("Agent is Active!");
        ActivityLogger.saveMode = saveLogs;
        foundObjects = ActivityLogger.getFoundObjects();
        ball_object = GameObject.FindGameObjectsWithTag("ball")[0];
        ball_object.GetComponent<DragDropManager>().onAgentDragDrop5();
        contValueScale = EnvironmentVariables.box_radius;
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
        // artificial discrete actions for gym env
        int[] discrete = new int[2];
        // between 0 and 5 inclusive for choosing object
        discrete[0] = Mathf.RoundToInt(Mathf.Abs(actions.ContinuousActions[4]) * 5.49f);
        // 0 or 1 for resetting
        discrete[1] = Mathf.RoundToInt(Mathf.Abs(actions.ContinuousActions[5]));

        // scaled continuous actions
        float[] continuous = new float[4];
        continuous[0] = (Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f) * contValueScale);
        continuous[1] = (Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f) * contValueScale);
        continuous[2] = (Mathf.Clamp(actions.ContinuousActions[2], -1f, 1f) * contValueScale);
        continuous[3] = (Mathf.Clamp(actions.ContinuousActions[3], -1f, 1f) * contValueScale);
        // Debug.Log(actions.ContinuousActions[0]);

        // if resetting, don't do anything else
        if (discrete[1] == 1)
        {
            Retry.OnButtonPress();
            ball_object.GetComponent<AgentDragDrop5>().erasePowerLine();
        }
        else
        {
            // abstain from placing or shooting if place value is 0 in X or Y
            if (continuous[0] != 0f | continuous[1] != 0f)
            {
                Vector3 mousePosition = new Vector3(continuous[0], continuous[1], 1);
                ball_object.GetComponent<AgentDragDrop5>().artificialBallInteraction(mousePosition);
                numActionsTaken++;
            }

            // abstain from moving objects if 0
            if (discrete[0] != 0)
            {
                GameObject objectToMove = foundObjects[discrete[0] - 1].gameObject;
                Vector3 placeObject = new Vector3(continuous[2], continuous[3], 1);
                objectToMove.GetComponent<AgentDragDrop>().setPosition(placeObject);
                numActionsTaken++;
            }
        }

        UnityEngine.Debug.Log((string)actionsToString(continuous, discrete));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        int objectsCount = ActivityLogger.getObjectsCount();
        Vector3[] positions = ActivityLogger.getLatestObjectPositions();
        for (int objectNum = 0; objectNum < objectsCount; objectNum++)
        {
            sensor.AddObservation(positions[objectNum].x);
            sensor.AddObservation(positions[objectNum].y);
        }

        // TODO: I need it to get observations ALL the time, not just once per action
        sensor.AddObservation(ball_object.transform.position.x);
        sensor.AddObservation(ball_object.transform.position.y);

        // keeping track of if ball is moving or not
        sensor.AddObservation(ball_object.GetComponent<Rigidbody2D>().velocity);

        // Gives -1 if no new collisions else idx of the collider objects
        sensor.AddObservation(ActivityLogger.getNewCollision());

        // technically the agent should know this itself
        sensor.AddObservation(Retry.isInSetup);
    }

    private string actionsToString(float[] continuous, int[] discrete)
    {
        string ret = "Continuous Actions: ";
        foreach (float x in continuous)
        {
            ret += (Mathf.Round(x * 1000) * 0.001).ToString() + " ";
        }

        ret += ":: Discrete Actions: ";
        foreach (int x in discrete)
        {
            ret += x + " ";
        }

        return ret;
    }
}
