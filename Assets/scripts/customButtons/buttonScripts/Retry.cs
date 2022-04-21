using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Retry : MonoBehaviour
{
    public static Rigidbody2D ball;
    public static Rigidbody2D pedestal;
    public static GameObject ball_object;
    public static GameObject pedestal_object;
    public static Vector3 absolutePositionBall;
    public static Vector3 relativePositionPedestal;
    public static bool isInSetup;
    public void Start()
    {
        ball_object = GameObject.FindGameObjectsWithTag("ball")[0];
        pedestal_object = GameObject.FindGameObjectsWithTag("pedestal")[0];
        ball = ball_object.GetComponent<Rigidbody2D>();
        pedestal = pedestal_object.GetComponent<Rigidbody2D>();
        // initial position of ball on screen
        absolutePositionBall = new Vector3(-1.35f, -4.12f, 0.0f);
        // under wherever the ball is
        relativePositionPedestal = new Vector3(0.0f, -0.58f, 0.0f);
        // makeJoint();
        putInSetup();
    }
    public static void OnButtonPress()
    {
        putInSetup();
        ActivityLogger.saveResetTime();
    }

    public static void putInSetup() // hardcoded settings for pedestal and ball
    {
        isInSetup = true;
        ball.transform.position = absolutePositionBall;
        pedestal.transform.position = absolutePositionBall + relativePositionPedestal;
        stabilize(ball);
        stabilize(pedestal);
        // make pedestal move with ball
        pedestal.transform.parent = ball.transform;
        // onJoint();
        // pedestal_object.AddComponent<DragDropLinear>();
        ball.constraints = RigidbodyConstraints2D.FreezePositionY; // only horizontal movement
        ball.GetComponent<customProperties>().inSetup = true;
        ball.GetComponent<customProperties>().makeTouchable();
    }

    public static void putOutSetup()
    {
        isInSetup = false;
        // make pedestal independent 
        pedestal.transform.parent = null;
        // offJoint();
        ball.constraints = RigidbodyConstraints2D.None;
        // Destroy(pedestal_object.GetComponent<DragDropLinear>());
        ball.GetComponent<customProperties>().inSetup = false;
    }

    public static void stabilize(Rigidbody2D obj)
    {
        obj.velocity = Vector3.zero;
        obj.angularVelocity = 0.0f;
        obj.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public static void makeJoint()
    {
        RelativeJoint2D joint = ball_object.AddComponent<RelativeJoint2D>();
        joint.connectedBody = pedestal;
        // joint.frequency = 0;

        joint = pedestal_object.AddComponent<RelativeJoint2D>();
        joint.connectedBody = ball;
        // joint.frequency = 0;
        // joint.linearOffset = relativePositionPedestal;
        joint.enableCollision = false;
    }

    public static void offJoint()
    {
        ball_object.GetComponent<RelativeJoint2D>().enabled = false;
        pedestal_object.GetComponent<RelativeJoint2D>().enabled = false;
    }

    public static void onJoint()
    {
        ball_object.GetComponent<RelativeJoint2D>().enabled = true;
        pedestal_object.GetComponent<RelativeJoint2D>().enabled = true;
    }
}
