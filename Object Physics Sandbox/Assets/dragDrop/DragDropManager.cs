using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropManager : MonoBehaviour
{
    private DragDrop5 dragDrop5;
    private DragDrop dragDrop1;
    public void onDragDrop1()
    {
        gameObject.AddComponent<DragDrop>();
    }

    public void onDragDrop5()
    {
        dragDrop5 = GetComponent<DragDrop5>();
        dragDrop5.enabled = true;
    }

    public void offDragDrop5()
    {
        dragDrop5 = GetComponent<DragDrop5>();
        dragDrop5.enabled = false;
    }

    public void offDragDrop1()
    {
        Destroy(GetComponent<DragDrop>());
    }
}