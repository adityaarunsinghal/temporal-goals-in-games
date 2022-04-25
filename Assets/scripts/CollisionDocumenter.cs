using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDocumenter : MonoBehaviour
{
    public bool saveCollisions = true;
    private GameObject ball_object;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (saveCollisions)
        {
            if (!ball_object.GetComponent<customProperties>().touchable)
            {
                if (collision.gameObject == ball_object)
                {
                    ActivityLogger.saveBallCollision(this.tag);
                    Debug.Log(string.Format("Hit {0}", this.tag));
                }
            }
        }
    }
}