using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using TMPro;

public class ActivityLogger : MonoBehaviour
{
    private static long captureNum = 0; // custom timing method
    private static System.DateTime localDate;
    private static Save save;
    private static DragDrop[] foundObjects;
    private static TMP_InputField runNameInput;

    public static void startLogging()
    {
        foundObjects = GameObject.FindObjectsOfType<DragDrop>();
        save = new Save();
        runNameInput = GameObject.FindGameObjectWithTag("runNameInput").GetComponent<TMP_InputField>();

        // keep last runName if applicable
        runNameInput.text = FreshStart.lastSavedRunName;

        for (int i = 0; i < foundObjects.Length; i++)
        {
            save.foundObjectsTags.Add(foundObjects[i].gameObject.tag);
        }

        localDate = System.DateTime.Now;
        captureNum = 0;
    }

    public static int getObjectsCount()
    {
        return save.foundObjectsTags.Count;
    }

    public static DragDrop[] getFoundObjects()
    {
        return foundObjects;
    }

    public static List<string> getFoundObjectTags()
    {
        return save.foundObjectsTags;
    }

    public static void saveBallPosition(Vector3 ballPosition)
    {
        save.ballPositions.Add(ballPosition);
        save.ballPositionsCT.Add(captureNum);
    }

    public static Vector3? getLatestBallPosition()
    {
        if (save.ballPositions.Count > 0)
        {
            return save.ballPositions[save.ballPositions.Count - 1];
        }
        else
        {
            return null;
        }
    }

    public static void saveShootVelocity(Vector3 shootVelocity)
    {
        save.velocities.Add(shootVelocity);
        save.velocitiesCT.Add(captureNum);
    }

    public static Vector3 getLatestShootVelocity()
    {
        return save.velocities[save.velocities.Count - 1];
    }

    public static void saveObjectPositions()
    {
        // cannot make multi-array because cannot JSON serialize
        int objectsCount = getObjectsCount();
        for (int objectNum = 0; objectNum < objectsCount; objectNum++)
        {
            // it is essential for each object to have a 
            // unique tag for this saving mechanism to work
            save.objectPositions.Add(GameObject.FindGameObjectsWithTag(save.foundObjectsTags[objectNum])[0].transform.position);
        }
        save.objectPositionsCT.Add(captureNum);
    }

    public static Vector3[] getLatestObjectPositions()
    {
        int objectsCount = getObjectsCount();
        Vector3[] positions = new Vector3[objectsCount];
        for (int objectNum = objectsCount; objectNum > 0; objectNum--)
        {
            positions[objectsCount - objectNum] = (save.objectPositions[save.objectPositions.Count - objectNum]);
        }
        return positions;
    }

    public static Vector3[] getInitialObjectPositions()
    {
        int objectsCount = getObjectsCount();
        Vector3[] positions = new Vector3[objectsCount];
        for (int objectNum = 0; objectNum < objectsCount; objectNum++)
        {
            positions[objectNum] = (save.objectPositions[objectNum]);
        }
        return positions;
    }

    public static void saveNote(string note)
    {
        save.notes.Add(note);
        save.notesCT.Add(captureNum);
    }

    public static List<string> getNotesList()
    {
        return save.notes;
    }

    public static void saveLogs()
    {
        // name by current time
        string name = string.Format("logs_{0}_{1}.json", localDate.ToString("yyyy_MM_dd_HH_mm_ss"), runNameInput.text.Replace(" ", "_"));

        string dir_path = Path.Combine($"{Application.dataPath}", "InteractionLogs/");
        //check if directory doesn't exit
        if (!Directory.Exists(dir_path))
        {
            // create it
            Directory.CreateDirectory(dir_path);
        }

        string savePath = Path.Combine(dir_path, name);

        // mark end of recording
        save.lastStepNum = captureNum;

        // save
        File.WriteAllText(savePath, JsonUtility.ToJson(save, true));
        UnityEngine.Debug.Log("Run Saved!");
    }

    void FixedUpdate()
    {
        captureNum++;
    }

    void OnApplicationQuit()
    {
        makeImportantSaves();
    }

    public static void makeImportantSaves()
    {
        NoteSystem.grabNote();
        ActivityLogger.saveLogs();
    }
}