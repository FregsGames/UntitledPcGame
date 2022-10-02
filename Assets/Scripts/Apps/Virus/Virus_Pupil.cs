using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus_Pupil : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private RectTransform parentTransform;

    [SerializeField]
    private float movementRange = 6f;

    void Update()
    {
        Debug.DrawLine(parentTransform.position, Input.mousePosition);

        rectTransform.position = parentTransform.position + (Input.mousePosition -parentTransform.position).normalized * movementRange;
    }
}
