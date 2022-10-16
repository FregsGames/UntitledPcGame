using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUtils : Singleton<MyUtils>
{
    public void DestroyChildren(Transform t)
    {
        for (int i = t.childCount - 1; i >= 0; i--)
        {
            Destroy(t.GetChild(i).gameObject);
        }
    }

    public void DestroyChildren(Transform t, Type type)
    {
        for (int i = t.childCount - 1; i >= 0; i--)
        {
            Transform child = t.GetChild(i);
            if (child.GetComponent(type) != null)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
