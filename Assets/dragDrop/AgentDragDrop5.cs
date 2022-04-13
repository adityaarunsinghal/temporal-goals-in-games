using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentDragDrop5 : DragDrop5 // Elastic Shooting Property
{
    void Update()
    {

    }

    public void artificialBallInteraction(Vector3 mousePosition)
    {
        Rigidbody2D ball = GetComponent<Rigidbody2D>();
        
        // don't allow interactions if ball is untouchable
        if (ball.GetComponent<customProperties>().touchable)
        {
            // move pedestal while the ball is being placed
            if (ball.GetComponent<customProperties>().inSetup)
            {
                // directly moveable for agent
                ball.MovePosition(ball.transform.position - new Vector3(mousePosition[0], 0, 0));

                // detach pedestal and get ready to shoot
                {
                    ball.velocity = Vector2.zero;
                    Retry.putOutSetup();
                }
            }
            else
            {
                // not in setup phase, so use mouse distance to shoot ball
                Vector2 dir = ball.transform.position - mousePosition;
                dir *= throwSpeed;
                ball.velocity = dir;

                // Save this shoot
                ActivityLogger.saveShootVelocity(dir);

                // player should only be able to shoot once before resetting
                ball.GetComponent<customProperties>().makeUntouchable();
            }
        }
    }

    void FixedUpdate()
    {

    }
}