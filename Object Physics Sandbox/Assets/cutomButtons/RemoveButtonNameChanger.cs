using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RemoveButtonNameChanger : MonoBehaviour
{
    public TMP_Text text;
    void FixedUpdate()
    {
        if (RemoveButton.toggled)
        {
            text.text = "Remove Mode: On";
        }
        else
        {
            text.text = "Remove Mode: Off";
        }
    }
}
