using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class ActivityLogger : MonoBehaviour
{
    private static long captureNum = 0;
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
        captureNum = 0;
    }

    public static void saveBallPosition(Vector3 ballPosition)
    {
        save.ballPositions.Add(ballPosition);
        save.ballPositionsCT.Add(captureNum);
    }

    public static void saveShootVelocity(Vector3 shootVelocity)
    {
        save.velocities.Add(shootVelocity);
        save.velocitiesCT.Add(captureNum);
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
        save.objectPositionsCT.Add(captureNum);
    }

    public static void saveLogs()
    {
        // name by current time
        string name = string.Format("InteractionLogs/logs_{0}", localDate.ToString("yyyy_MM_dd_HH_mm"));
        string savePath = name + ".json";

        // mark end of recording
        save.lastStepNum = captureNum;

        // save
        File.WriteAllText(savePath, JsonUtility.ToJson(save, true));
        UnityEngine.Debug.Log("Run Saved");
    }

    void FixedUpdate()
    {
        captureNum++;
    }
}