using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModeDescription : MonoBehaviour
{
    private TMP_Text modeOut;
    private GameObject ball;

    void Start()
    {
        modeOut = GetComponent<TMP_Text>();
        ball = GameObject.FindGameObjectWithTag("ball");
    }
    void Update()
    {
        if (ball.GetComponent<customProperties>().inSetup)
        {
            modeOut.text = "Placement Mode";
        }
        else
        {
            modeOut.text = "Shoot Mode";
        }
    }
}
