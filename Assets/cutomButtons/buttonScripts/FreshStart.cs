using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FreshStart : MonoBehaviour
{
    public static string lastSavedRunName = null;
    public void OnButtonPress()
    {
        // keep the entered name
        lastSavedRunName = GameObject.FindGameObjectWithTag("runNameInput").GetComponent<TMP_InputField>().text;

        // save run so far and start new one
        ActivityLogger.makeImportantSaves();

        // hard reset
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
