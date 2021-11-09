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
    }
    public void onButtonPress()
    {
        if (inSetupPhase)
        {
            for (int i = 0; i < saveableObjects.Length; i++)
            {
                saveableObjects[i].transform.rotation = Quaternion.Euler(0, 0, 0);
                saveableObjects[i].GetComponent<DragDropManager>().offDragDrop4();
                saveableObjects[i].GetComponent<DragDropManager>().onDragDrop1();
                saveableObjects[i].transform.position = objectPositions[i];
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
            }
        }

        inSetupPhase = !inSetupPhase;
    }
}