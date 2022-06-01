using System;
using System.Linq;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField]
    private SystemEventManager eventManager;

    [SerializeField]
    private Sprite testingSprite;

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
        if (Input.GetKeyDown(KeyCode.L))
        {
            Computer.Instance.NotificationCenter.RequestNotification(testingSprite, "Titulaso de la notificación", "bla bal 1231 esto esun texot asjd otre 213 1oi g", null);
            //((NumericPopup) eventManager.RequestPopUp("Pruebesita", App.AppType.NumericPopup)).Setup("1234");
        }
    }
#endif
}
