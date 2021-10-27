using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndResetPositions : MonoBehaviour
{
    private Vector2[] objectPositions;
    private DragDrop3[] saveableObjects = FindObjectsOfType<DragDrop3>();

    void Awake()
    {
        objectPositions = new Vector2[saveableObjects.Length];
    }

    void SavePositions()
    {
        for (int i = 0; i < objectPositions.Length; i++)
        {
            objectPositions[i] = saveableObjects[i].transform.position;
        }
    }

    void ResetPositions()
    {
        for (int i = 0; i < saveableObjects.Length; i++)
        {
            saveableObjects[i].transform.position = objectPositions[i];
        }
    }
}