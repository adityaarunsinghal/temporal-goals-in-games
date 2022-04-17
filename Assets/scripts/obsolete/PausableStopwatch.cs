using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PausableStopwatch
{
    public double time;
    public string textTime;
    public bool timerOn;
    public int frameRate = 60;

    public void Start()
    {
        timerOn = true;
        time = 0.0d;
        Time.captureFramerate = frameRate;
    }

    void Update()
    {
        if (timerOn)
        {
            time += Time.deltaTime;
        }
    }

    public void Pause()
    {
        timerOn = false;
    }

    public void Unpause()
    {
        timerOn = true;
    }

}