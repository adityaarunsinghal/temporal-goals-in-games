using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndResetPositionsDestructive : MonoBehaviour
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
            // destroy RigidBody
            for (int i = 0; i < saveableObjects.Length; i++)
            {
                saveableObjects[i].transform.rotation = Quaternion.Euler(0, 0, 0);
                Destroy(saveableObjects[i].GetComponent<Rigidbody2D>()); // TODO: Instead, try adding DragDrop(1) and turn off DragDrop3 for easy movement/placement of objects and then destroy it when entering 'play' mode
                saveableObjects[i].transform.position = objectPositions[i];
            }
            inSetupPhase = !inSetupPhase;
        }

        else if (!inSetupPhase)
        {
            // save setup and turn on game play
            for (int i = 0; i < objectPositions.Length; i++)
            {
                saveableObjects[i].transform.gameObject.AddComponent<Rigidbody2D>();
                objectPositions[i] = saveableObjects[i].transform.position;
            }
            inSetupPhase = !inSetupPhase;
        }
    }
}