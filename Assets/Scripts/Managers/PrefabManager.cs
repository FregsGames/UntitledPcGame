using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    [SerializeField]
    private PrefabsDB prefabsDB;

    public PrefabsDB PrefabsDB { get => prefabsDB; }
}
