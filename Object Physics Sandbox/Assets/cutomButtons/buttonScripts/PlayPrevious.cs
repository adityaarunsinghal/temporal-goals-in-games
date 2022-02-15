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
    private long captureNum = 0;
    public TMP_Text timeText;
    private Save save;
    private int shootsCount;
    private int objectsCount;
    private int ballSnapsCount;
    private int objectSnapsCount;
    private int objectSnapNum;
    private int ballSnapNum;
    private int shootNum;
    private bool inPlayback = false;


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
        timeText.text = "Playback Step: N/A";
        loadLogs(absPath);

        UnityEngine.Debug.Log("Playing back from logs in Real Time");

        shootsCount = save.velocities.Count;
        objectsCount = save.foundObjectsTags.Count;
        ballSnapsCount = save.ballPositions.Count;
        objectSnapsCount = save.objectPositions.Count / objectsCount;
        objectSnapNum = 0;
        ballSnapNum = 0;
        shootNum = 0;

        captureNum = 0;
        inPlayback = true;

    }

    public void stopPlaying()
    {
        timeText.text = "Playback Step: N/A";
        UnityEngine.Debug.Log("Playback Ended");
        inPlayback = false;
    }

    void playStepNum(long stepNum)
    {
        timeText.text = "Playback Step: " + stepNum + "/" + save.lastStepNum;

        if (objectSnapNum < objectSnapsCount)
        {
            if (stepNum == save.objectPositionsCT[objectSnapNum])
            {

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

                }
                objectSnapNum++;
            }
        }


        if (ballSnapNum < ballSnapsCount)
        {
            if (stepNum == save.ballPositionsCT[ballSnapNum])
            {
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

                ballSnapNum++;
            }
        }


        if (shootNum < shootsCount)
        {
            if (stepNum == save.velocitiesCT[shootNum])
            {

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

                shootNum++;
            }
        }
    }

    void FixedUpdate()
    {
        if (inPlayback)
        {
            if (captureNum < save.lastStepNum)
            {
                playStepNum(captureNum++);
            }
            else
            {
                stopPlaying();
            }
        }
    }
}
