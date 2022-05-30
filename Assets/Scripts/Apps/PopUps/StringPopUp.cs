using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.Apps
{
    public class StringPopUp : App
    {
        [SerializeField]
        private Button confirmButton;
        [SerializeField]
        private Button cancelButton;
        [SerializeField]
        private TMP_InputField inputField;

        private void OnEnable()
        {
            confirmButton.onClick.AddListener(() => { systemEventManager.OnStringPopUpSubmit?.Invoke(inputField.text); Close(); });
            cancelButton.onClick.AddListener(() => { systemEventManager.OnStringPopUpSubmit?.Invoke(null); Close(); });
        }

        private void OnDisable()
        {
            confirmButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();
        }
    }
}
