using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UI Event Manager", menuName = "ScriptableObjects/UI Event Manager", order = 1)]
public class UIEventManager : ScriptableObject
{
    public Action<Icon> OnIconStartDrag;
    public Action<Icon> OnEndStartDrag;
    public Action<float> OnUIScaleChanged;

    public Action<string> OnMailButtonClicked;
}
