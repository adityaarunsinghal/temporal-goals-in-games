using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public static class ActivityLogger
{
    private static Stopwatch timer;
    private static System.DateTime localDate;
    private static List<Vector3> velocities;
    private static List<Vector3> ballPositions;
    private static List<Vector3[]> objectPositions;
    private static List<long> velocitiesCT;
    private static List<long> ballPositionsCT;
    private static List<long> objectPositionsCT;
    private static DragDrop[] foundObjects;

    public static void startLogging()
    {
        localDate = System.DateTime.Now;
        timer = new Stopwatch();
        velocities = new List<Vector3>();
        ballPositions = new List<Vector3>();
        objectPositions = new List<Vector3[]>();
        velocitiesCT = new List<long>();
        ballPositionsCT = new List<long>();
        objectPositionsCT = new List<long>();
        foundObjects = GameObject.FindObjectsOfType<DragDrop>();
        timer.Start();
    }

    public static void saveBallPosition(Vector3 ballPosition)
    {
        ballPositions.Add(ballPosition);
        ballPositionsCT.Add(timer.ElapsedTicks);
    }

    public static void saveShootVelocity(Vector3 shootVelocity)
    {
        velocities.Add(shootVelocity);
        velocitiesCT.Add(timer.ElapsedTicks);
    }

    public static void saveObjectPositions()
    {
        int top = foundObjects.Length;
        Vector3[] positions = new Vector3[top];
        for (int i = 0; i < top; i++)
        {
            positions[i] = foundObjects[i].transform.position;
        }
        objectPositions.Add(positions);
        objectPositionsCT.Add(timer.ElapsedTicks);
    }

    public static void saveLogs()
    {
        // name by current time
        string name = string.Format("InteractionLogs/logs_{0}.txt", localDate.ToString("yyyy_MM_dd_HH_mm"));
        string lines = "";
        lines += "\n-----BALL SHOOTS-----\n";

        for (int i = 0; i < ballPositions.Count; i++)
        {
            lines += ballPositions[i] + "\t" + ballPositionsCT[i] + "\tVelocity:\t" + velocities[i] + "\n";
        }
        lines += "\n-----OBJECT POSITIONS-----\n";

        for (int i = 0; i < objectPositions.Count; i++)
        {
            for (int j = 0; j < objectPositions[i].Length; j++)
            {
                lines += foundObjects[j].gameObject.tag + ":\t" + objectPositions[i][j] + "\t" + objectPositionsCT[i] + "\n";
            }
        }

        // save
        File.WriteAllText(name, lines);
    }
}