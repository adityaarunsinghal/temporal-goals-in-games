using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FreezeButtonNameChanger : MonoBehaviour
{
    public TMP_Text text;
    void FixedUpdate()
    {
        if (FreezeButton2.toggled)
        {
            text.text = "Freeze Mode: On";
        }
        else
        {
            text.text = "Freeze Mode: Off";
        }
    }
}
