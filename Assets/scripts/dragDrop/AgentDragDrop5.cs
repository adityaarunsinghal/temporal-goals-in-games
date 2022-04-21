using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AgentDragDrop5 : DragDrop5 // Elastic Shooting Property
{

    void Start()
    {
        if (AgentStatus.active)
        {
            GameObject.FindGameObjectWithTag("runNameInput").GetComponent<TMP_InputField>().text = "Agent";
        }
    }
    void Update()
    {

    }

    public void artificialBallInteraction(Vector3 mousePosition)
    {
        // don't allow interactions if ball is untouchable
        if (alwaysAccessibleBall.GetComponent<customProperties>().touchable)
        {
            // move pedestal while the ball is being placed
            if (alwaysAccessibleBall.GetComponent<customProperties>().inSetup)
            {
                float new_x = Mathf.Clamp(mousePosition.x, EnvironmentVariables.minX, EnvironmentVariables.maxX);
                alwaysAccessibleBall.MovePosition(new Vector2(new_x,
                                    alwaysAccessibleBall.position.y));
                Rigidbody2D pedestal = alwaysAccessibleBall.GetComponentsInChildren<Rigidbody2D>()[0];
                if (pedestal)
                {
                    pedestal.transform.position = new Vector2(new_x, pedestal.position.y);
                }
                Retry.stabilize(alwaysAccessibleBall.GetComponent<Rigidbody2D>());
                Retry.putOutSetup();
            }
            else
            {
                // not in setup mode, so use mouse distance to shoot ball
                Vector2 dir = alwaysAccessibleBall.transform.position - mousePosition;
                dir *= throwSpeed;
                alwaysAccessibleBall.velocity = dir;

                // Save this shoot
                ActivityLogger.saveShootVelocity(dir);

                // player should only be able to shoot once before resetting
                alwaysAccessibleBall.GetComponent<customProperties>().makeUntouchable();
            }
        }
    }

    void FixedUpdate()
    {
        // no need to add to logs if agent is not the one interacting
        if (AgentStatus.active)
        {
            if (ActivityLogger.saveAllBallPos)
            {
                ActivityLogger.saveBallPosition(alwaysAccessibleBall.transform.position);
            }
            else
            {
                // save only setup time ball positions for replay
                if (Retry.isInSetup)
                {
                    ActivityLogger.saveBallPosition(ball.transform.position);
                }
            }
        }
    }
}