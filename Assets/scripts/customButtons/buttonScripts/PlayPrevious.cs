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
    private string absPath;
    private long captureNum = 0;
    public TMP_Text timeText;
    private Save save;
    private int shootsCount;
    private int objectsCount;
    private int notesCount;
    private int resetCount;
    private int ballSnapsCount;
    private int objectSnapsCount;
    private int objectSnapNum;
    private int ballSnapNum;
    private int shootNum;
    private int noteNum;
    private int resetNum;

    private List<string> printNotes;
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
        if (GameObject.FindGameObjectWithTag("replayPathInput"))
        {
            if (GameObject.FindGameObjectWithTag("replayPathInput").GetComponent<TMP_InputField>().text != "")
            {
                absPath = GameObject.FindGameObjectWithTag("replayPathInput").GetComponent<TMP_InputField>().text;
                UnityEngine.Debug.Log("Found path");
                startPlaying();
            }
            else
            {
                UnityEngine.Debug.Log("Please enter replay absolute path in text field");
            }
        }
        else
        {
            UnityEngine.Debug.Log("Did not find input field with correct tag");
        }
    }
    public void startPlaying()
    {
        timeText.text = "Playback Step: N/A";
        printNotes = new List<string>();
        loadLogs(absPath);

        UnityEngine.Debug.Log("Playing back from logs in Real Time");

        shootsCount = save.velocities.Count;
        objectsCount = save.foundObjectsTags.Count;
        notesCount = save.notes.Count;
        resetCount = save.resetCT.Count;
        ballSnapsCount = save.ballPositions.Count;
        objectSnapsCount = save.objectPositions.Count / objectsCount;
        objectSnapNum = 0;
        ballSnapNum = 0;
        shootNum = 0;
        noteNum = 0;
        resetNum = 0;

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

        if (resetNum < resetCount)
        {
            if (stepNum == save.resetCT[resetNum])
            {
                Retry.putInSetup();
                resetNum++;
            }
        }

        if (shootNum < shootsCount)
        {
            if (stepNum == save.velocitiesCT[shootNum])
            {
                // detach from pedestal everytime a shoot was attempted
                Retry.putOutSetup();
            }
        }
        
        if (ballSnapNum < ballSnapsCount)
        {
            if (stepNum == save.ballPositionsCT[ballSnapNum])
            {
                // transform ball position
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

        if (shootNum < shootsCount)
        {
            if (stepNum == save.velocitiesCT[shootNum])
            {
                GameObject ball = GameObject.FindGameObjectsWithTag("ball")[0];
                if (ball)
                {
                    // no need to use physics of shoot if every single ball pos was saved
                    if (!ActivityLogger.saveAllBallPos)
                    {
                        ball.GetComponent<Rigidbody2D>().velocity = save.velocities[shootNum];
                    }
                }
                else
                {
                    UnityEngine.Debug.Log("Object with tag not found: ball");
                }

                shootNum++;
            }
        }

        if (noteNum < notesCount)
        {
            if (stepNum == save.notesCT[noteNum])
            {
                printNotes.Add(save.notes[noteNum]);
                NoteSystem.updateOutNotes(printNotes);
                noteNum++;
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
