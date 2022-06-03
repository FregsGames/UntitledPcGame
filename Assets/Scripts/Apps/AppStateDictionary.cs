using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class AppStateDictionary : SerializedMonoBehaviour
{
    [SerializeField]
    private Dictionary<System.Enum, GameObject> states = new Dictionary<System.Enum, GameObject>();

    public Dictionary<Enum, GameObject> States { get => states; set => states = value; }
}
