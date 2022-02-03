using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class ActivityLogger
{
    private static Stopwatch timer;
    private static System.DateTime localDate;
    private static Save save;
    private static DragDrop[] foundObjects;

    public static void startLogging()
    {
        foundObjects = GameObject.FindObjectsOfType<DragDrop>();
        save = new Save();

        for (int i = 0; i < foundObjects.Length; i++)
        {
            save.foundObjectsTags.Add(foundObjects[i].gameObject.tag);
        }

        localDate = System.DateTime.Now;
        timer = new Stopwatch();
        timer.Start();
    }

    public static void saveBallPosition(Vector3 ballPosition)
    {
        save.ballPositions.Add(ballPosition);
        save.ballPositionsCT.Add(timer.ElapsedTicks);
    }

    public static void saveShootVelocity(Vector3 shootVelocity)
    {
        save.velocities.Add(shootVelocity);
        save.velocitiesCT.Add(timer.ElapsedTicks);
    }

    public static void saveObjectPositions()
    {
        int top = save.foundObjectsTags.Count;
        Vector3[] positions = new Vector3[top];
        for (int i = 0; i < top; i++)
        {
            // it is essential for each object to have a 
            // unique tag for this saving mechanism to work
            positions[i] = GameObject.FindGameObjectsWithTag(save.foundObjectsTags[i])[0].transform.position;
        }
        save.objectPositions.Add(positions);
        save.objectPositionsCT.Add(timer.ElapsedTicks);
    }

    public static void saveLogs()
    {
        // name by current time
        string name = string.Format("InteractionLogs/logs_{0}", localDate.ToString("yyyy_MM_dd_HH_mm"));

        // string savePath = Path.Combine(Application.persistentDataPath, name + ".save");
        // string readableLogsPath = Path.Combine(Application.persistentDataPath, name + "_readable.txt");

        string savePath = name + ".json";
        string readableLogsPath = name + "_readable.txt";

        string readableLines = "";
        readableLines += "\n-----BALL SHOOTS-----\n";
        for (int i = 0; i < save.ballPositions.Count; i++)
        {
            readableLines += save.ballPositions[i] + "\t" + save.ballPositionsCT[i] + "\tVelocity:\t" + save.velocities[i] + "\n";
        }
        readableLines += "\n-----OBJECT POSITIONS-----\n";
        for (int i = 0; i < save.objectPositions.Count; i++)
        {
            for (int j = 0; j < save.objectPositions[i].Length; j++)
            {
                readableLines += save.foundObjectsTags[j] + ":\t" + save.objectPositions[i][j] + "\t" + save.objectPositionsCT[i] + "\n";
            }
        }

        // save

        // BinaryFormatter bf = new BinaryFormatter();
        // FileStream file = File.Create(savePath);
        // bf.Serialize(file, save);
        // file.Close();
        
        File.WriteAllText(savePath, JsonUtility.ToJson(save));
        File.WriteAllText(readableLogsPath, readableLines);
        UnityEngine.Debug.Log("Run Saved");
    }
}