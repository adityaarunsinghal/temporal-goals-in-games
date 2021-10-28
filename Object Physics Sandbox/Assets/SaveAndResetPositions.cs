using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndResetPositions : MonoBehaviour
{
    bool inSetupPhase = true;
    private Vector2[] objectPositions;
    private float[] gravities;
    private DragDrop3[] saveableObjects;

    void Start()
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
                saveableObjects[i].GetComponent<Rigidbody2D>().gravityScale = 0.0f;
                saveableObjects[i].transform.rotation = Quaternion.Euler(0, 0, 0);
                saveableObjects[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                saveableObjects[i].transform.position = objectPositions[i];
            }
            inSetupPhase = !inSetupPhase;
        }

        else if (!inSetupPhase)
        {
            // save setup and turn on game play
            for (int i = 0; i < objectPositions.Length; i++)
            {
                saveableObjects[i].GetComponent<Rigidbody2D>().gravityScale = 1f;
                if (saveableObjects[i].tag=="notFrozen")
                {
                    saveableObjects[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                }
                objectPositions[i] = saveableObjects[i].transform.position;
            }
            inSetupPhase = !inSetupPhase;
        }
    }
}