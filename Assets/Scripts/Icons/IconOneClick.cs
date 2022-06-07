using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IconOneClick : Icon
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!dragging)
        {
            if (associatedApp != null)
            {
                associatedApp.RecenterOnUI();
            }
            else
            {
                associatedApp = InstantiatorManager.Instance.Instantiate(associatedAppType, associatedAppID).GetComponentInChildren<App>();
                associatedApp.Open();

                if (associatedAppID == string.Empty)
                {
                    associatedAppID = associatedApp.ID;
                    Serialize();
                }
            }
        }
    }
}
