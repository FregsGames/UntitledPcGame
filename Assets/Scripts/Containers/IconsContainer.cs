using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static PrefabsDB;

public class IconsContainer : UniqueID, ISaveableState
{
    [SerializeField]
    protected Icon iconPrefab;

    [SerializeField]
    protected int rows;
    [SerializeField]
    protected int cols;
    [SerializeField]
    private RectTransform rect;
    [SerializeField]
    private WindowTopBar windowTopBar;
    [SerializeField]
    private AppType type;

    public AppType Type { get => type; }
    protected Dictionary<FolderPosition, Icon> grid = new Dictionary<FolderPosition, Icon>();
    string ISaveableState.ID { get => ID; }


    private void OnEnable()
    {
        if (windowTopBar != null)
        {
            windowTopBar.OnWindowEndDrag += RefreshGridPositions;
        }
    }

    private void OnDisable()
    {
        if (windowTopBar != null)
        {
            windowTopBar.OnWindowEndDrag -= RefreshGridPositions;
        }
    }

    public void LoadState()
    {
        if (SaveManager.instance.RetrieveString(ID) != string.Empty)
        {
            Icon[] childrenIcons = GetComponentsInChildren<Icon>();
            for (int i = childrenIcons.Length - 1; i >= 0; i--)
            {
                Destroy(childrenIcons[i].gameObject);
            }

            Deserialize();
        }
        else
        {
            InitializeGrid();
            PositionateIcons();
        }
    }

    private void RefreshGridPositions()
    {
        InitializeGrid();
        PositionateIcons();
    }

    protected void InitializeGrid()
    {
        grid.Clear();

        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Vector3 position = new Vector3(rect.position.x + rect.rect.width / (cols + 1) * (i + 1), (rect.position.y + rect.rect.height) - rect.rect.height / (rows + 1) * (j + 1), 0);
                FolderPosition key = new FolderPosition(new Vector2Int(i, j), position);

                if (!grid.ContainsKey(key))
                {
                    grid.Add(key, null);
                }
            }
        }
    }

    protected virtual void PositionateIcons()
    {

    }


    public virtual bool MoveIconTo(Icon icon, Vector3 pos)
    {
        return true;
    }

    public void MoveIconTo(Icon icon, Vector2Int gridPos)
    {
        FolderPosition folderPos = grid.FirstOrDefault(g => g.Key.gridPosition == gridPos).Key;

        if (folderPos != null && grid[folderPos] == null)
        {
            grid[folderPos] = icon;
            icon.SetPos(folderPos.absolutePosition);
            icon.transform.SetParent(transform);
            icon.Container = this;
        }
        else
        {
            Debug.Log($"Error Moving Icon to grid pos {gridPos} in folder {gameObject.name}. " +
                $"Either pos does not exist or pos is already occupied");
        }
    }

    public void RemoveIconIfAlreadyExists(Icon icon)
    {
        if (grid.ContainsValue(icon))
        {
            FolderPosition key = grid.FirstOrDefault(g => g.Value == icon).Key;
            grid[key] = null;
        }
    }


    protected FolderPosition GetClosestFreeSlot(Vector3 pos, Icon icon = null)
    {
        List<FolderPosition> freeSlots = grid.Where(g => g.Value == null || g.Value == icon).Select(g => g.Key).ToList();

        if (freeSlots.Count == 0)
        {
            return new FolderPosition(new Vector2Int(-1, -1), Vector3.one * -1);
        }
        else
        {
            int closestIndex = 0;

            for (int i = 0; i < freeSlots.Count; i++)
            {
                if (Vector3.Distance(freeSlots[i].absolutePosition, pos) < Vector3.Distance(freeSlots[closestIndex].absolutePosition, pos))
                {
                    closestIndex = i;
                }
            }
            return freeSlots[closestIndex];
        }

    }

    public Dictionary<string, string> Serialize()
    {
        Dictionary<string, string> serialized = new Dictionary<string, string>();

        serialized.Add($"{ID}", ID);
        serialized.Add($"{ID}_rows", rows.ToString());
        serialized.Add($"{ID}_cols", cols.ToString());

        foreach (var item in grid.Where(pos => pos.Value != null))
        {
            serialized.Add($"{ID}_iconAt_{item.Key.gridPosition.x}_{item.Key.gridPosition.y}", item.Value.ID);
       
        }

        return serialized;
    }

    public void Deserialize()
    {
        rows = int.Parse(SaveManager.instance.RetrieveString($"{ID}_rows"));
        cols = int.Parse(SaveManager.instance.RetrieveString($"{ID}_cols"));

        InitializeGrid();

        Dictionary<string, string> iconsIDs = SaveManager.instance.RetrieveStringThatContains($"{ID}_iconAt_");

        foreach (var iconID in iconsIDs)
        {
            Icon icon = Instantiate(iconPrefab, transform);
            icon.ID = iconID.Value;
            icon.Deserialize();
            string[] splittedId = iconID.Key.Split('_');

            MoveIconTo(icon, new Vector2Int(int.Parse(splittedId[2]), int.Parse(splittedId[3])));
        }
    }

    public class FolderPosition
    {
        public Vector2Int gridPosition;
        public Vector3 absolutePosition;

        public FolderPosition(Vector2Int gridPosition, Vector3 absolutePosition)
        {
            this.gridPosition = gridPosition;
            this.absolutePosition = absolutePosition;
        }
    }

    // Odin Stuff
    [PropertySpace(10, 0)]
    [Button("New ID", ButtonSizes.Medium)]
    protected void RegenerateID()
    {
        RegenerateGUID();
    }

#if UNITY_EDITOR
    [SerializeField]
    private bool gizmosOn = true;


    private void OnDrawGizmos()
    {
        if (!gizmosOn)
            return;

        Gizmos.color = Color.red;

        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Vector3 position = new Vector3(rect.position.x + rect.rect.width / (cols + 1) * (i + 1), rect.position.y + rect.rect.height / (rows + 1) * (j + 1), 0);

                FolderPosition key = new FolderPosition(new Vector2Int(i, j), position);

                if (grid.ContainsKey(key) && grid[key] != null)
                {
                    Gizmos.DrawCube(key.absolutePosition, Vector3.one * 20);
                }
                else
                {
                    Gizmos.DrawSphere(key.absolutePosition, 20);
                }

            }
        }
    }
#endif
}
