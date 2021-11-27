using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Restart : MonoBehaviour
{
    public static Rigidbody2D ball;
    public static Rigidbody2D pedestal;
    public static Vector3 initialPosition;
    public void Start()
    {
        GameObject ball_object = GameObject.FindGameObjectsWithTag("ball")[0];
        GameObject pedestal_object = GameObject.FindGameObjectsWithTag("pedestal")[0];
        ball = ball_object.GetComponent<Rigidbody2D>();
        pedestal = pedestal_object.GetComponent<Rigidbody2D>();
        // pedestal.mass = ball.mass;
        // pedestal.angularDrag = ball.angularDrag;
        // pedestal.drag = ball.drag;
        initialPosition = new Vector3(-1.38f, -4.12f, 0.0f);
        putInSetup();
    }
    public static void OnButtonPress()
    {
        putInSetup();
    }

    public static void putInSetup()
    {
        ball.transform.position = initialPosition;
        Vector3 temp = pedestal.transform.position;
        temp.x = initialPosition.x;
        pedestal.transform.position = temp;
        stabilize(ball);
        stabilize(pedestal);
        pedestal.transform.parent = ball.transform;
        ball.constraints = RigidbodyConstraints2D.FreezePositionY;
        ball.GetComponent<customProperties>().inSetup = true;
    }

    public static void putOutSetup()
    {
        pedestal.transform.parent = null;
        ball.constraints = RigidbodyConstraints2D.None;
        ball.GetComponent<customProperties>().inSetup = false;
    }

    public static void stabilize(Rigidbody2D obj)
    {
        obj.velocity = Vector3.zero;
        obj.angularVelocity = 0.0f;
    }
}
