using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchParentColor : MonoBehaviour
{
    public Color c;
    SpriteRenderer[] childRenderers;
    void Start()
    {
        childRenderers = GetComponentsInChildren<SpriteRenderer>();
    }
    void Update()
    {
        c = GetComponent<SpriteRenderer>().color;

        for (int i=0; i<childRenderers.Length; i++)
        {
            childRenderers[i].color = c;
        }
    }
}
