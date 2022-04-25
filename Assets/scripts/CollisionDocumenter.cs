using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDocumenter : MonoBehaviour
{
    public bool saveCollisions = true;
    private GameObject ball_object;
    public void Start()
    {
        ball_object = GameObject.FindGameObjectsWithTag("ball")[0];
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (saveCollisions)
        {
            if (!ball_object.GetComponent<customProperties>().touchable)
            {
                if (collision.gameObject == ball_object)
                {
                    ActivityLogger.saveBallCollision(this);
                    Debug.Log(string.Format("Hit {0}", this));
                }
            }
        }
    }
}