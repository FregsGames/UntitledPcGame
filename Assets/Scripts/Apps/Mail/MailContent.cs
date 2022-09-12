using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MailContent : MonoBehaviour
{
    [SerializeField]
    private GameObject attachedFilePrefab;

    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI from;
    [SerializeField]
    private TextMeshProUGUI body;

    [SerializeField]
    private Transform container;

    public void Setup(Mail mail)
    {
        title.text = mail.about;
        from.text = mail.from;
        body.text = mail.body;

        MailAttachedFile[] mailAttachedFiles = GetComponentsInChildren<MailAttachedFile>();

        for (int i = mailAttachedFiles.Length - 1; i >= 0; i--)
        {
            Destroy(mailAttachedFiles[i].gameObject);
        }

        foreach (var attachedFileId in mail.attachedFilesIds)
        {
            MailAttachedFile file = Instantiate(attachedFilePrefab, container).GetComponent<MailAttachedFile>();
            file.Setup(attachedFileId);
        }
    }
}
