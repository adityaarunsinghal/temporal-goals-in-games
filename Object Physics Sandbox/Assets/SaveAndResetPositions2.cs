using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndResetPositions2 : MonoBehaviour
{
    bool inSetupPhase = true;
    private Vector2[] objectPositions;
    private float[] gravities;
    private DragDropManager[] saveableObjects;

    void Start()
    {
        saveableObjects = FindObjectsOfType<DragDropManager>();
        objectPositions = new Vector2[saveableObjects.Length];
        for (int i = 0; i < objectPositions.Length; i++)
        {
            objectPositions[i] = saveableObjects[i].transform.position;
        }
    }
    public void onButtonPress()
    {
        if (inSetupPhase)
        {   
            // return to positions and pause all objects
            for (int i = 0; i < saveableObjects.Length; i++)
            {
                saveableObjects[i].GetComponent<DragDropManager>().offDragDrop4();
                saveableObjects[i].GetComponent<DragDropManager>().onDragDrop1();
                saveableObjects[i].transform.position = objectPositions[i];
                saveableObjects[i].GetComponent<Rigidbody2D>().gravityScale = 0f;
                saveableObjects[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                saveableObjects[i].GetComponent<Rigidbody2D>().angularVelocity = 0f;
                saveableObjects[i].transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        else if (!inSetupPhase)
        {
            // save setup and turn on game play
            for (int i = 0; i < objectPositions.Length; i++)
            {
                saveableObjects[i].GetComponent<DragDropManager>().onDragDrop4();
                saveableObjects[i].GetComponent<DragDropManager>().offDragDrop1();
                objectPositions[i] = saveableObjects[i].transform.position;
                saveableObjects[i].GetComponent<Rigidbody2D>().gravityScale = 1f;
            }
        }

        inSetupPhase = !inSetupPhase;
    }
}