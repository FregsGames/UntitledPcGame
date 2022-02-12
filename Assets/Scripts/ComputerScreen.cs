using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerScreen : Singleton<ComputerScreen>
{
    [SerializeField]
    private GraphicRaycaster raycaster;
    public GraphicRaycaster GraphicRaycaster { get => raycaster; }
}
