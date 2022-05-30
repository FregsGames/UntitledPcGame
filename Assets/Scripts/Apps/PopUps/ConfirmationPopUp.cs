using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Apps
{
    public class ConfirmationPopUp : App
    {
        [SerializeField]
        private Button confirmButton;
        [SerializeField]
        private Button cancelButton;

        private void OnEnable()
        {
            confirmButton.onClick.AddListener(() => { systemEventManager.OnPopUpSubmit?.Invoke(true); Close(); });
            cancelButton.onClick.AddListener(() => { systemEventManager.OnPopUpSubmit?.Invoke(false); Close(); });
        }

        private void OnDisable()
        {
            confirmButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();
        }
    }
}
