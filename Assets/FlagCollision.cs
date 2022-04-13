using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCollision : MonoBehaviour
{
    public GameObject collidedWith;
    public GameObject childCollider;
    public bool collided;

    public void OnCollisionEnter2D(Collision2D another)
    {
        collided = true;
        childCollider = another.otherCollider.gameObject;
        collidedWith = another.gameObject;
    }

    public void reset()
    {
        collided = false;
        childCollider = null;
        collidedWith = null;
    }
}