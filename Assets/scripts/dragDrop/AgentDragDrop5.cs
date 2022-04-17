using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AgentDragDrop5 : DragDrop5 // Elastic Shooting Property
{
    private Rigidbody2D alwaysAccessibleBall;

    void Start()
    {
        alwaysAccessibleBall = GetComponent<Rigidbody2D>();
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
                // directly moveable for agent
                alwaysAccessibleBall.MovePosition(alwaysAccessibleBall.transform.position - new Vector3(mousePosition[0], 0, 0));

                // detach pedestal and get ready to shoot
                {
                    alwaysAccessibleBall.velocity = Vector2.zero;
                    Retry.putOutSetup();
                }
            }
            else
            {
                // not in setup phase, so use mouse distance to shoot ball
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
            if (Retry.isInSetup)
            {
                // save only setup time ball positions for replay
                ActivityLogger.saveBallPosition(alwaysAccessibleBall.transform.position);
            }
        }
    }
}