using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FreshStart : MonoBehaviour
{
    public static string lastSavedRunName = null;
    public static List<string> lastSavedNotes = null;

    public static void OnButtonPress()
    {
        // keep the entered name
        lastSavedRunName = GameObject.FindGameObjectWithTag("runNameInput").GetComponent<TMP_InputField>().text;

        // save notes bank
        // although there's no point doing this if I can't re-instate it
        lastSavedNotes = ActivityLogger.getNotesList();

        // save run so far and start new one
        ActivityLogger.makeImportantSaves();

        // hard reset
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public static void softReset()
    {
        Retry.OnButtonPress();
        DragDrop[] foundObjects = ActivityLogger.getFoundObjects();
        Vector3[] initPos = ActivityLogger.getInitialObjectPositions();
        for (int objectNum = 0; objectNum < foundObjects.Length; objectNum++)
        {
           foundObjects[objectNum].gameObject.transform.position = initPos[objectNum];
        }
    }
}
