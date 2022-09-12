using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class DownloadManager : SerializedSingleton<DownloadManager>
{
    [SerializeField]
    private Dictionary<string, DownloableFile> downloads = new Dictionary<string, DownloableFile>();
    [SerializeField]
    private List<string> downloaded = new List<string>();


    [SerializeField]
    private SystemEventManager eventManager;

    [SerializeField]
    private Icon iconPrefab;

    private void OnEnable()
    {
        eventManager.OnFileDownloadRequest += DownloadFile;
    }

    public void Initialize()
    {
        Dictionary<string, string> lockedDictionary = SaveManager.Instance.RetrieveStringThatEndsWith("_downloaded");

        foreach (var downloadedFileId in lockedDictionary.Values)
        {
            downloaded.Add(downloadedFileId);
        }
    }

    private void DownloadFile(string fileId)
    {
        if (!downloads.ContainsKey(fileId))
        {
            Debug.LogWarning("Trying to download an unexisting file");
            return;
        }

        if (downloaded.Contains(fileId))
        {
            Debug.LogWarning("File has already been downloaded");
            return;
        }

        DownloableFile downloableFile = downloads[fileId];

        if (!DownloadFolderExists())
        {
            CreateDownloadFolder();
        }

        if(downloableFile is DownloableTextFile)
        {
            Computer.Instance.CreateFile(fileId, downloableFile.name, downloableFile.fileType, "download-folder", $"{fileId}-associated");
            SaveManager.Instance.Save($"{fileId}-associated_content", ((DownloableTextFile)downloableFile).content);
        }
        else
        {
            Computer.Instance.CreateFile(fileId, downloableFile.name, downloableFile.fileType, "download-folder");
        }

        downloaded.Add(fileId);

        var openedDownloadFolder = Computer.Instance.Ram.GetOpenedApp("download-folder") as FolderContainer;

        if (openedDownloadFolder != null)
        {
            var file = Instantiate(iconPrefab, Computer.Instance.Desktop.transform);
            file.Setup(downloableFile.fileType, downloableFile.name, null, false);
            file.AssociatedAppID = $"{fileId}-associated";
            openedDownloadFolder.MoveIconTo(file, Vector2.zero);
        }

        SaveManager.Instance.Save($"{fileId}_downloaded", fileId);
    }

    private void CreateDownloadFolder()
    {
        Computer.Instance.CreateFile("download-folder-icon", "Descargas", App.AppType.Folder, "", "download-folder");

        var downloadFolderIcon = Instantiate(iconPrefab, Computer.Instance.Desktop.transform);
        downloadFolderIcon.RegenerateGUID();
        downloadFolderIcon.AssociatedAppID = "download-folder";
        downloadFolderIcon.Setup(App.AppType.Folder, "Descargas", "folder", false);
        Computer.Instance.Desktop.MoveIconTo(downloadFolderIcon, Vector2.zero);
        SaveManager.Instance.Save("download-folder", "download-folder");
    }

    private bool DownloadFolderExists()
    {
        bool downloadFolderExists = false;

        foreach (var desktopIcon in Computer.Instance.Desktop.GetComponentsInChildren<Icon>())
        {
            if (desktopIcon.AssociatedAppID == "download-folder")
            {
                downloadFolderExists = true;
                break;
            }
        }

        return downloadFolderExists;
    }

    private void OnDisable()
    {
        eventManager.OnFileDownloadRequest -= DownloadFile;
    }
}


[Serializable]
public class DownloableFile
{
    public App.AppType fileType;
    public string name;
}

[Serializable]
public class DownloableTextFile : DownloableFile
{
    public string content;
}

