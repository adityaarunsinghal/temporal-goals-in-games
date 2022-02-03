using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class PlayPrevious : MonoBehaviour
{
    public string absPath;
    private Stopwatch timer;
    public TMP_Text timeText;
    private Save save;

    private void Start()
    {
        timeText.text = "Playback Elapsed Ticks: N/A";
        timer = new Stopwatch();
        loadLogs(absPath);
    }
    private void loadLogs(string absPath)
    {
        if (File.Exists(absPath))
        {
            save = JsonUtility.FromJson<Save>(File.ReadAllText(absPath));
            UnityEngine.Debug.Log("Game Loaded");
        }
        else
        {
            UnityEngine.Debug.Log("Provided Absolute Path does not exist");
        }
    }
    public void OnButtonPress()
    {
        startPlaying();
    }
    public void startPlaying()
    {
        timer.Start();
        int shootsCount = save.ballPositions.Count;
        int objectsCount = save.foundObjectsTags.Count;

        for (int shootNum = 0; shootNum < shootsCount; shootNum++)
        {
            // wait for time to match live play
            while (timer.ElapsedTicks < save.ballPositionsCT[shootNum])
            {
                timeText.text = "Playback Elapsed Ticks: " + timer.ElapsedTicks.ToString();
                continue;
            }
            // place all objects where they were during shoot
            for (int objectNum = 0; objectNum < objectsCount; objectNum++)
            {
                GameObject obj = GameObject.FindGameObjectsWithTag(save.foundObjectsTags[objectNum])[0];
                if (obj)
                {
                    obj.transform.position = save.objectPositions[(shootNum * objectsCount) + objectNum];
                }
                else
                {
                    UnityEngine.Debug.Log("Object with tag not found: " + save.foundObjectsTags[objectNum]);
                }
            }
            // place ball and shoot it
            GameObject ball = GameObject.FindGameObjectsWithTag("ball")[0];
            if (ball)
            {
                ball.transform.position = save.ballPositions[shootNum];
                ball.GetComponent<Rigidbody2D>().velocity = save.velocities[shootNum];
            }
            else
            {
                UnityEngine.Debug.Log("Object with tag not found: ball");
            }
        }
    }
}