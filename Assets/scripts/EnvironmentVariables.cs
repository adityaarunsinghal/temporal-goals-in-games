using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentVariables : MonoBehaviour
{
    public static GameObject topWall;
    public static GameObject bottomWall;
    public static GameObject leftWall;
    public static GameObject rightWall;
    public static float minX;
    public static float maxX;
    public static float minY;
    public static float maxY;
    public static float box_radius;


    public void Start()
    {
        bottomWall = GameObject.FindGameObjectsWithTag("bottomWall")[0];
        topWall = GameObject.FindGameObjectsWithTag("topWall")[0];
        leftWall = GameObject.FindGameObjectsWithTag("leftWall")[0];
        rightWall = GameObject.FindGameObjectsWithTag("rightWall")[0];
        float wallThickness = topWall.transform.localScale.y;
        minX = leftWall.transform.position.x + wallThickness;
        maxX = rightWall.transform.position.x - wallThickness;
        minY = bottomWall.transform.position.y + wallThickness;
        maxY = topWall.transform.position.y - wallThickness;
        box_radius = (maxX - minX)/2f;
        printToConsole();
    }

    public void printToConsole()
    {
        UnityEngine.Debug.Log("Max X for Box: " + maxX);
        UnityEngine.Debug.Log("Max Y for Box: " + maxY);
        UnityEngine.Debug.Log("Min X for Box: " + minX);
        UnityEngine.Debug.Log("Min Y for Box: " + minY);
        UnityEngine.Debug.Log("Box Radius: " + box_radius);
    }
}