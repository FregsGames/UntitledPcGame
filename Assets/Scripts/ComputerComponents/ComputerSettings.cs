using UnityEngine;

public class ComputerSettings : MonoBehaviour
{
    [SerializeField]
    public int maxSubfolderLevel = 5;

    public int MaxSubfolderLevel { get => maxSubfolderLevel; set => maxSubfolderLevel = value; }
}
