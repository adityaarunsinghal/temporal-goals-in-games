using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DuplicateButtonNameChanger : MonoBehaviour
{
    public TMP_Text text;
    void FixedUpdate()
    {
        if (DuplicateButton.toggled)
        {
            text.text = "Duplicate Mode: On";
        }
        else
        {
            text.text = "Duplicate Mode: Off";
        }
    }
}
