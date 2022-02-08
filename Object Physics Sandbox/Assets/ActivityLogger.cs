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
        // cannot make multi-array because cannot JSON serialize
        int objectsCount = save.foundObjectsTags.Count;
        for (int objectNum = 0; objectNum < objectsCount; objectNum++)
        {
            // it is essential for each object to have a 
            // unique tag for this saving mechanism to work
            save.objectPositions.Add(GameObject.FindGameObjectsWithTag(save.foundObjectsTags[objectNum])[0].transform.position);
        }
        save.objectPositionsCT.Add(timer.ElapsedTicks);
    }

    public static void saveLogs()
    {
        // name by current time
        string name = string.Format("InteractionLogs/logs_{0}", localDate.ToString("yyyy_MM_dd_HH_mm"));
        string savePath = name + ".json";
        
        // save
        File.WriteAllText(savePath, JsonUtility.ToJson(save, true));
        UnityEngine.Debug.Log("Run Saved");
    }
}