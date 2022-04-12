using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DestroyCounter
{
    public static int destroyedCount;

    static void Start()
    {
        destroyedCount = 0;
    }
    public static void addOne()
    {
        destroyedCount++;
    }

    public static void reset()
    {
        destroyedCount = 0;
    }
}