using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FolderManager : Singleton<FolderManager>
{
    private ComputerScreen computerScreen;
    private List<GameObject> folderPool = new List<GameObject>();
    private Dictionary<IconFolder, GameObject> currentlyOpenedFolders = new Dictionary<IconFolder, GameObject>();

    [SerializeField]
    private GameObject folderPrefab;
    [Header("Default values")]
    [SerializeField]
    private Vector2 defaultFolderSize = new Vector2(1200, 600);
    [SerializeField]
    private int poolBaseSize = 10;
    [SerializeField]
    private Vector2 poolInstantiatePos = new Vector2(10000, 10000);

    private void Start()
    {
        computerScreen = ComputerScreen.instance;

        for (int i = 0; i < poolBaseSize; i++)
        {
            InstantiateWindow();
        }
    }

    public GameObject OpenFolder(Vector2 size, IconFolder icon = null)
    {
        GameObject folder = folderPool.FirstOrDefault(w => !w.activeInHierarchy);
        RectTransform rect = folder.GetComponent<RectTransform>();

        if (folder == null)
        {
            folder = InstantiateWindow();
        }

        rect.sizeDelta = size == Vector2.zero ? defaultFolderSize : size;
        rect.position = new Vector3((ComputerScreen.instance.BackgroundSize.x - rect.sizeDelta.x) / 2,
            (ComputerScreen.instance.BackgroundSize.y - rect.sizeDelta.y) / 2, 0);

        folder.SetActive(true);
        return folder;
    }

    private GameObject InstantiateWindow()
    {
        var window = Instantiate(folderPrefab, poolInstantiatePos, Quaternion.identity, computerScreen.transform);
        window.SetActive(false);
        folderPool.Add(window);
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ICustomSerializable[] customSerializables = FindObjectsOfType<MonoBehaviour>().OfType<ICustomSerializable>().ToArray();


            foreach (var serializable in customSerializables)
            {
                SaveManager.instance.RemoveEntriesThatContains(serializable.ID);

                foreach (var entry in serializable.Serialize())
                {
                    SaveManager.instance.Save(entry.Key, entry.Value);
                }
            }
        }
    }

}
