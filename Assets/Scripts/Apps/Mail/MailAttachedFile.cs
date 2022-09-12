using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MailAttachedFile : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private SystemEventManager systemEventManager;

    public string FileId { get; set; }

    public void Setup(string fileId)
    {
        FileId = fileId;
    }

    public void OnClick()
    {
        systemEventManager.OnFileDownloadRequest?.Invoke(FileId);
    }
}
