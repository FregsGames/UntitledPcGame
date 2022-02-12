using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Event Manager", menuName = "ScriptableObjects/Event Manager", order = 1)]
public class EventManager : ScriptableObject
{
    public Action<Icon> OnIconStartDrag;
    public Action<Icon> OnEndStartDrag;

}
