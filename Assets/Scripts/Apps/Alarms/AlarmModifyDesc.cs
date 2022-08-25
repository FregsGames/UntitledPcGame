using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class AlarmModifyDesc : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Alarm alarm;

    public void OnPointerClick(PointerEventData eventData)
    {
        alarm.ModifyDescription();
    }
}
