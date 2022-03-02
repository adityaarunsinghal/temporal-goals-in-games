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
        TMP_InputField runNameInputField = GameObject.FindGameObjectWithTag("runNameInput").GetComponent<TMP_InputField>();
        lastSavedRunName = runNameInputField.text;

        // save run so far and start new one
        ActivityLogger.saveLogs();

        // hard reset
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
