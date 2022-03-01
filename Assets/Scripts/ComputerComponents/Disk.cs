using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disk : MonoBehaviour
{
    [SerializeField]
    private bool isMainDisk;

    [SerializeField]
    private List<IconsContainer> iconsContainers = new List<IconsContainer>();

    public bool IsMainDisk { get => isMainDisk;}

    public void Initialize()
    {
        foreach (var iconContainer in iconsContainers)
        {
            if (iconContainer.gameObject.activeInHierarchy)
            {
                iconContainer.LoadState();
            }
        }
    }
}
