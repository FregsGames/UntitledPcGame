using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FolderManager : Singleton<FolderManager>
{
    private ComputerScreen computerScreen;

    [SerializeField]
    private GameObject folderPrefab;
    [Header("Default values")]
    [SerializeField]
    private Vector2 defaultFolderSize = new Vector2(1200, 600);


    public void Initialize()
    {
        computerScreen = ComputerScreen.Instance;
    }

    public GameObject OpenFolder(string id = "", bool lockedFolder = false)
    {
        GameObject folder = InstantiateFolder();
        RectTransform rect = folder.GetComponent<RectTransform>();

        rect.sizeDelta = defaultFolderSize;
        rect.position = new Vector3((ComputerScreen.Instance.BackgroundSize.x - rect.sizeDelta.x) *ComputerScreen.Instance.ScreenRelation.x / 2,
            (ComputerScreen.Instance.BackgroundSize.y - (rect.sizeDelta.y / 2f)) * ComputerScreen.Instance.ScreenRelation.y, 0);

        FolderContainer folderContainer = folder.GetComponentInChildren<FolderContainer>();

        if (id != string.Empty)
        {
            folderContainer.ID = id;
        }
        else
        {
            folderContainer.ID = Guid.NewGuid().ToString();
        }

        if (lockedFolder)
        {
            folderContainer.Type = App.AppType.LockedFolder;
        }

        folder.SetActive(true);
        folderContainer.LoadState();

        return folder;
    }

    private GameObject InstantiateFolder()
    {
        var window = Instantiate(folderPrefab, Vector3.one * 10000, Quaternion.identity, computerScreen.Desktop);
        return window;
    }

    protected override void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
}
