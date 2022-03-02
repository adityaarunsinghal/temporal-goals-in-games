using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Retry : MonoBehaviour
{
    public static Rigidbody2D ball;
    public static Rigidbody2D pedestal;
    public static Vector3 absolutePositionBall;
    public static Vector3 relativePositionPedestal;
    public static bool isInSetup;
    public void Start()
    {
        GameObject ball_object = GameObject.FindGameObjectsWithTag("ball")[0];
        GameObject pedestal_object = GameObject.FindGameObjectsWithTag("pedestal")[0];
        ball = ball_object.GetComponent<Rigidbody2D>();
        pedestal = pedestal_object.GetComponent<Rigidbody2D>();
        absolutePositionBall = new Vector3(-1.35f, -4.12f, 0.0f); // initial position of ball on screen
        relativePositionPedestal = new Vector3(0.0f, 0.58f, 0.0f); // under wherever ball is
        putInSetup();
    }
    public static void OnButtonPress()
    {
        putInSetup();
    }

    public static void putInSetup() // hardcoded settings for pedestal and ball
    {
        isInSetup = true;
        ball.transform.position = absolutePositionBall;
        pedestal.transform.position = absolutePositionBall - relativePositionPedestal;
        stabilize(ball);
        stabilize(pedestal);
        pedestal.transform.parent = ball.transform; // make pedestal move with ball
        ball.constraints = RigidbodyConstraints2D.FreezePositionY; // only horizontal movement
        ball.GetComponent<customProperties>().inSetup = true;
        ball.GetComponent<customProperties>().makeTouchable();
    }

    public static void putOutSetup()
    {
        isInSetup = false;
        pedestal.transform.parent = null; // make pedestal independent 
        ball.constraints = RigidbodyConstraints2D.None;
        ball.GetComponent<customProperties>().inSetup = false;
    }

    public static void stabilize(Rigidbody2D obj)
    {
        obj.velocity = Vector3.zero;
        obj.angularVelocity = 0.0f;
        obj.transform.rotation = Quaternion.Euler(0, 0, 0); 
    }
}