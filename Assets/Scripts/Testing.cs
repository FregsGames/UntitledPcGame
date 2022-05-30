using System;
using System.Linq;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField]
    private SystemEventManager eventManager;

    private void OnEnable()
    {
        eventManager.OnNumericPopUpSubmit += Check;
    }

    private void Check(bool obj)
    {
        Debug.Log("the key is " + (obj ? "correct" : "incorrect"));
    }

    private void OnDisable()
    {
        eventManager.OnNumericPopUpSubmit -= Check;
    }

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
            ((NumericPopup)eventManager.RequestPopUp(App.AppType.NumericPopup)).Setup("145612");
        }
    }
#endif
}
