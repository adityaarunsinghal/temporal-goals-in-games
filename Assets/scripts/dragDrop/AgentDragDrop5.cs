using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AgentDragDrop5 : DragDrop5 // Elastic Shooting Property
{

    protected override void Start()
    {
        alwaysAccessibleBall = GetComponent<Rigidbody2D>();
        shootVelocity = null;
        ActivityLogger.startLogging();
    }
    protected override void Update()
    {
        // do nothing and override normal dragdrop5
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

                // Save this shoot in the next fixed update
                shootVelocity = dir;

                // player should only be able to shoot once before resetting
                alwaysAccessibleBall.GetComponent<customProperties>().makeUntouchable();
            }
        }
    }
    protected override void FixedUpdate()
    {
        if (shootVelocity != null)
        {
            ActivityLogger.saveShootVelocity((Vector3)shootVelocity);
            shootVelocity = null;
        }
    }
}