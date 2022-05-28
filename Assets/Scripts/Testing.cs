using System.Linq;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField]
    private SystemEventManager eventManager;

#if UNITY_EDITOR
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    ISaveableState[] customSerializables = FindObjectsOfType<MonoBehaviour>().OfType<ISaveableState>().ToArray();


        //    foreach (var serializable in customSerializables)
        //    {
        //        serializable.Serialize();
        //        _ = SaveManager.Instance.SaveChanges();
        //    }
        //}
        if (Input.GetKeyDown(KeyCode.L))
        {
            eventManager.RequestPopUp();
        }
    }
#endif
}
