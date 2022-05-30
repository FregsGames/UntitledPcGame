using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumericPopupButton : MonoBehaviour
{
    [SerializeField]
    private int key = 1;
    [SerializeField]
    private Button button;


    private void OnEnable()
    {
        NumericPopup numericPopup = GetComponentInParent<NumericPopup>();
        button.onClick.AddListener(() => numericPopup.OnKeyPressed?.Invoke(key));
    }

    private void OnValidate()
    {
        if(button != null)
        {
            if(key == -1)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = "<";
                gameObject.name = $"NumericButton_<";
                return;
            }

            button.GetComponentInChildren<TextMeshProUGUI>().text = key.ToString();
            gameObject.name = $"NumericButton_{key.ToString()}";
        }
    }
}
