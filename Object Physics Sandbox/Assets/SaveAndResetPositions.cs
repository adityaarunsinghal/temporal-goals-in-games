using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndResetPositions : MonoBehaviour
{
    bool inSetupPhase = true;
    private Vector2[] objectPositions;
    private float[] gravities;
    private DragDrop3[] saveableObjects;

    void Awake()
    {
        saveableObjects = FindObjectsOfType<DragDrop3>();
        objectPositions = new Vector2[saveableObjects.Length];
    }
    public void onButtonPress()
    {
        if (inSetupPhase)
        {
            // reset positions and turn off gravity
            for (int i = 0; i < saveableObjects.Length; i++)
            {
                gravities[i] = saveableObjects[i].GetComponent <Rigidbody2D>().gravityScale;
                saveableObjects[i].GetComponent <Rigidbody2D>().gravityScale = 0.0f;
                saveableObjects[i].transform.position = objectPositions[i];
            }
            inSetupPhase = !inSetupPhase;
        }

        else if (!inSetupPhase)
        {
            // save setup and turn on game play
            for (int i = 0; i < objectPositions.Length; i++)
            {
                saveableObjects[i].GetComponent <Rigidbody2D>().gravityScale = gravities[i];
                objectPositions[i] = saveableObjects[i].transform.position;
            }
            inSetupPhase = !inSetupPhase;
        }
    }
}