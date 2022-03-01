using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FileSystem : MonoBehaviour
{
    [SerializeField]
    private List<Disk> disks = new List<Disk>();

    public Disk MainDisk { get => disks.FirstOrDefault(d => d.IsMainDisk); }
}
