using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class objectsCacher // not monobehavior
{

    public static List<Vector2> objectPositions;
    public static List<DragDropManager> saveableObjects;
    private static DragDropManager[] foundObjects;
    public static int top;

    public static void scan() // resets the scanner and finds all existing objects
    {
        foundObjects = GameObject.FindObjectsOfType<DragDropManager>();
        saveableObjects = new List<DragDropManager>();
        objectPositions = new List<Vector2>(); 

        top = foundObjects.Length;

        for (int i = 0; i < top; i++)
        {
            saveableObjects.Add(foundObjects[i]);
            objectPositions.Add(saveableObjects[i].transform.position);
        }
    }

    public static void cacheObject(GameObject obj)
    {
        saveableObjects.Add(obj.GetComponent<DragDropManager>());
        objectPositions.Add(saveableObjects[top].transform.position);
        top += 1;
    }

    public static void removeObject(GameObject obj)
    {
        for (int i = 0; i < top; i++)
        {
            if (GameObject.ReferenceEquals(obj, saveableObjects[i].transform.gameObject))
            {
                saveableObjects.RemoveAt(i);
                objectPositions.RemoveAt(i);
                top -= 1;
            }
        }
    }
}