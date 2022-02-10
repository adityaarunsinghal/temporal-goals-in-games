using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop5 : MonoBehaviour // Elastic Shooting Property
{
    public Rigidbody2D ball;
    Vector3 mousePosition;
    public float dragSpeed = 15;
    public float throwSpeed = 10;
    Vector3 lastPosition;
    LineRenderer line;
    Vector3[] linePos;

    void Start()
    {
        // make the system work on a clock cycle for easier replay?
        // Time.captureFramerate = 50; 

        ActivityLogger.startLogging();
        linePos = new Vector3[2];
        line = GetComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply")); // for the green to red effect
    }

    void Update()
    {
        // track mouse
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            // Select object 
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);

            // if something was actually clicked
            if (targetObject)
            {
                // this script only concerns the ball's behavior
                if (targetObject.gameObject.tag == "ball")
                {
                    // check if ball is in "touchable" node
                    if (targetObject.GetComponent<customProperties>().touchable)
                    {
                        ball = targetObject.gameObject.GetComponent<Rigidbody2D>();
                    }
                }
            }
        }

        if (ball) // player is touching the ball
        {
            // move pedestal while the ball is being placed
            if (ball.GetComponent<customProperties>().inSetup)
            {
                Vector2 dir = mousePosition - ball.transform.position;
                dir *= dragSpeed;
                ball.velocity = dir;

                // detach pedestal and get ready to shoot
                if (Input.GetMouseButtonUp(0))
                {
                    ball.velocity = Vector2.zero;
                    ball = null;
                    Restart.putOutSetup();
                }
            }
            else
            {
                updatePowerLine(ball.transform.position, mousePosition, 0.07f);

                // not in setup phase, so use mouse distance to shoot ball
                if (Input.GetMouseButtonUp(0))
                {
                    updatePowerLine(ball.transform.position, ball.transform.position, 0f);
                    Vector2 dir = ball.transform.position - mousePosition;
                    dir *= throwSpeed;
                    ball.velocity = dir;

                    // Save this shoot
                    ActivityLogger.saveShootVelocity(dir);

                    // player should only be able to shoot once before resetting
                    ball.GetComponent<customProperties>().makeUntouchable();
                    ball = null;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (ball) 
        {
            if (Restart.isInSetup)
            {
                // save non-shoot ball positions for replay
                ActivityLogger.saveBallPosition(ball.transform.position);
            }
        }

        // always save where objects are
        ActivityLogger.saveObjectPositions();
    }

    private void updatePowerLine(Vector3 start, Vector3 end, float width)
    // just draws the line over and over
    {
        linePos[0] = start;
        linePos[1] = end;
        line.startWidth = width;
        line.endWidth = width;
        line.SetPositions(linePos);
        line.useWorldSpace = true;
    }

    void OnApplicationQuit()
    {
        ActivityLogger.saveLogs();
    }
}