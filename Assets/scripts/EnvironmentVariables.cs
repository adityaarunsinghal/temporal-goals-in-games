using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentVariables: MonoBehaviour
{
    public static GameObject topWall;
    public static GameObject bottomWall;
    public static GameObject leftWall;
    public static GameObject rightWall;
    public static float minX;
    public static float maxX;
    public static float minY;
    public static float maxY;

    public void Start()
    {
        bottomWall = GameObject.FindGameObjectsWithTag("bottomWall")[0];
        topWall = GameObject.FindGameObjectsWithTag("wall")[0];
        leftWall = GameObject.FindGameObjectsWithTag("leftWall")[0];
        rightWall = GameObject.FindGameObjectsWithTag("rightWall")[0];
        float offset = topWall.transform.localScale.y;
        minX = leftWall.transform.position.x + offset;
        maxX = rightWall.transform.position.x - offset;
        minY = bottomWall.transform.position.y + offset;
        maxY = topWall.transform.position.y - offset;
    }
}