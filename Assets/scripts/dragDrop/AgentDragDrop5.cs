using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AgentDragDrop5 : DragDrop5 // Elastic Shooting Property
{
    public float shootScale = 1f;

    protected override void Start()
    {
        alwaysAccessibleBall = GetComponent<Rigidbody2D>();
        shootVelocity = null;
        ActivityLogger.startLogging();
        linePos = new Vector3[2];
        line = GetComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
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
                updatePowerLine(alwaysAccessibleBall.transform.position, mousePosition, 0.07f);
                // not in setup mode, so use mouse distance to shoot ball
                Vector2 dir = alwaysAccessibleBall.transform.position - mousePosition;
                dir *= throwSpeed;
                alwaysAccessibleBall.velocity = dir;

                if (!ActivityLogger.saveAllBallVel)
                {
                    // Save this shoot in the next fixed update
                    shootVelocity = dir;
                }
                ActivityLogger.saveShootTime();

                // player should only be able to shoot once before resetting
                alwaysAccessibleBall.GetComponent<customProperties>().makeUntouchable();
            }
        }
    }
    protected override void FixedUpdate()
    {
        if (ActivityLogger.saveAllBallVel)
        {
            ActivityLogger.saveVelocity(alwaysAccessibleBall.velocity);
        }
        else
        {
            if (shootVelocity != null)
            {
                ActivityLogger.saveVelocity((Vector3)shootVelocity);
                shootVelocity = null;
            }
        }
    }

    public void erasePowerLine()
    {
        Vector3[] zero = new Vector3[2]{Vector3.zero, Vector3.zero};
        line.SetPositions(zero);
        line.startWidth = 0;
        line.endWidth = 0;
        line.useWorldSpace = true;
    }
}