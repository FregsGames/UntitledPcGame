using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.Apps
{
    public class ConfirmationPopUp : App, IPopUp
    {
        [SerializeField]
        private Button confirmButton;
        [SerializeField]
        private Button cancelButton;
        [SerializeField]
        private TextMeshProUGUI text;

        private void OnEnable()
        {
            confirmButton.onClick.AddListener(() => { systemEventManager.OnPopUpSubmit?.Invoke(true); Close(); });
            cancelButton.onClick.AddListener(() => { systemEventManager.OnPopUpCancel?.Invoke(); Close(); });
        }

        private void OnDisable()
        {
            confirmButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();
        }

        public void SetText(string text)
        {
            this.text.text = text;
        }
    }
}
