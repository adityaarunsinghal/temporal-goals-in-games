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

    private void setup()
    {
        timeText.text = "Playback Time: N/A";
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
        setup();
        startPlaying();
    }
    public void startPlaying()
    {
        UnityEngine.Debug.Log("Playing back from logs in Real Time");

        timer.Start();
        int shootsCount = save.velocities.Count;
        int objectsCount = save.foundObjectsTags.Count;
        int ballSnapsCount = save.ballPositions.Count;
        int objectSnapsCount = save.objectPositions.Count / objectsCount;
        int objectSnapNum = 0;
        int ballSnapNum = 0;
        int shootNum = 0;

        while ((objectSnapNum < objectSnapsCount - 1))
        {
            timeText.text = "Playback Time: " + timer.ElapsedTicks.ToString();
            // UnityEngine.Debug.Log("Object Snap Num: " + objectSnapNum + " of " + objectSnapsCount);
            while (timer.ElapsedTicks < save.objectPositionsCT[objectSnapNum])
            {
                // let timer run until its time
                continue;
            }

            // pause timer while objects are being moved
            Time.timeScale = 0;
            objectSnapNum++;

            // place all objects 
            for (int objectNum = 0; objectNum < objectsCount; objectNum++)
            {
                GameObject obj = GameObject.FindGameObjectsWithTag(save.foundObjectsTags[objectNum])[0];
                if (obj)
                {
                    obj.transform.position = save.objectPositions[(objectSnapNum * objectsCount) + objectNum];
                }
                else
                {
                    UnityEngine.Debug.Log("Object with tag not found: " + save.foundObjectsTags[objectNum]);
                }

                // continue timer
                Time.timeScale = 1;
            }

            if (ballSnapNum < ballSnapsCount - 1)
            {
                if (timer.ElapsedTicks >= save.ballPositionsCT[ballSnapNum])
                {
                    // Time.timeScale = 0;
                    ballSnapNum++;
                    // attach to pedestal
                    Restart.putInSetup();

                    // place ball
                    GameObject ball = GameObject.FindGameObjectsWithTag("ball")[0];
                    if (ball)
                    {
                        ball.transform.position = save.ballPositions[ballSnapNum];
                    }
                    else
                    {
                        UnityEngine.Debug.Log("Object with tag not found: ball");
                    }

                    // Time.timeScale = 1;
                }
            }

            if (shootNum < shootsCount - 1)
            {
                if (timer.ElapsedTicks >= save.velocitiesCT[shootNum])
                {
                    shootNum++;

                    // detach from pedestal
                    Restart.putOutSetup();

                    GameObject ball = GameObject.FindGameObjectsWithTag("ball")[0];
                    if (ball)
                    {
                        ball.GetComponent<Rigidbody2D>().velocity = save.velocities[shootNum];
                    }
                    else
                    {
                        UnityEngine.Debug.Log("Object with tag not found: ball");
                    }
                }
            }
        }
    }
}
