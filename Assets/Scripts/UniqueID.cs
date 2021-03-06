using System;
using UnityEngine;

public class UniqueID : MonoBehaviour
{
    [SerializeField]
    private string id = Guid.NewGuid().ToString();

    public string ID { get => id; set => id = value; }

    [ContextMenu("Generate new ID")]
    public void RegenerateGUID() => id = Guid.NewGuid().ToString();

}
