using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FileSystem : MonoBehaviour
{
    [SerializeField]
    private List<Disk> disks = new List<Disk>();

    public Disk MainDisk { get => disks.FirstOrDefault(d => d.IsMainDisk); }
}
