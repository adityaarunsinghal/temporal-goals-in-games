using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using UnityEngine.UI;

public class PlayPrevious : MonoBehaviour
{
    public string absPath;
    private static Stopwatch timer;
    public static Text timeText;
    private static Save save;

    public void Start()
    {
        loadLogs(absPath);
    }
    public static void loadLogs(string absPath)
    {
        if (File.Exists(absPath))
        {
            // BinaryFormatter bf = new BinaryFormatter();
            // FileStream file = File.Open(absPath, FileMode.Open);
            // save = (Save)bf.Deserialize(file);
            // file.Close();
            save = JsonUtility.FromJson<Save>(File.ReadAllText(absPath));
            UnityEngine.Debug.Log("Game Loaded");
        }
        else
        {
            UnityEngine.Debug.Log("Provided Absolute Path does not exist");
        }
    }
    public static void OnButtonPress()
    {
        startPlaying();
    }
    public static void startPlaying() // hardcoded settings for pedestal and ball
    {
        timer = new Stopwatch();
        timer.Start();
        for (int i=0; i<save.ballPositions.Count; i++)
        {
            while (timer.ElapsedTicks < save.ballPositionsCT[i])
                {
                    continue;
                }
            for (int j=0; j<save.foundObjectsTags.Count; j++)
            {
                // FindGameObjectsWithTag(save.foundObjectsTags[i])[0]
                UnityEngine.Debug.Log(save.foundObjectsTags[j]); // temporary - no placing
            }
            // place ball at this point and shoot it
        }
    }

    public static void Update()
    {
        timeText.text = timer.ElapsedTicks.ToString();
    }
}