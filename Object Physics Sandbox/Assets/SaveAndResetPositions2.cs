using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndResetPositions2 : MonoBehaviour
{
    bool inSetupPhase = true;

    void Start()
    {
        objectsCacher.scan();
    }
    
    public void onButtonPress()
    {
        if (inSetupPhase)
        {
            // return to positions and pause all objects
            for (int i = 0; i < objectsCacher.top; i++)
            {
                objectsCacher.saveableObjects[i].GetComponent<DragDropManager>().offDragDrop4();
                objectsCacher.saveableObjects[i].GetComponent<DragDropManager>().onDragDrop1();
                objectsCacher.saveableObjects[i].transform.position = objectsCacher.objectPositions[i];
                objectsCacher.saveableObjects[i].GetComponent<Rigidbody2D>().gravityScale = 0f;
                objectsCacher.saveableObjects[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                objectsCacher.saveableObjects[i].GetComponent<Rigidbody2D>().angularVelocity = 0f;
                objectsCacher.saveableObjects[i].transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        else if (!inSetupPhase)
        {
            // save setup and turn on game play
            for (int i = 0; i < objectsCacher.top; i++)
            {
                objectsCacher.saveableObjects[i].GetComponent<DragDropManager>().onDragDrop4();
                objectsCacher.saveableObjects[i].GetComponent<DragDropManager>().offDragDrop1();
                objectsCacher.objectPositions[i] = objectsCacher.saveableObjects[i].transform.position;
                objectsCacher.saveableObjects[i].GetComponent<Rigidbody2D>().gravityScale = 1f;
            }
        }

        inSetupPhase = !inSetupPhase;
    }
}