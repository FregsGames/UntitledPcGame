using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class IconsGridContainer : IconsContainer
{
    [SerializeField]
    protected int rows;
    [SerializeField]
    protected int cols;
    [SerializeField]
    private RectTransform rect;
    [SerializeField]
    private WindowTopBar windowTopBar;

    protected Dictionary<FolderPosition, Icon> grid = new Dictionary<FolderPosition, Icon>();


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
        if (SaveManager.Instance.RetrieveString(ID) != string.Empty)
        {
            RemoveChildren();
            Deserialize();
        }
        else
        {
            InitializeGrid();
            PositionateIcons();
            SerializeFirstDepthChildren();
            Serialize();
            InitializeInnerContainers();
        }
    }

    public void RemoveChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        InitializeGrid();
    }

    private void SerializeFirstDepthChildren()
    {
        foreach (Transform child in transform)
        {
            Icon childIcon = child.GetComponent<Icon>();
            if (childIcon != null)
            {
                childIcon.Serialize();
            }
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
        for (int j = 0; j < rows; j++)
        {
            for (int i = 0; i < cols; i++)
            {

                Vector3 position = GetGridPositionOfPoint(i, j);

                FolderPosition key = new FolderPosition(new Vector2Int(i, j), position);

                if (!grid.ContainsKey(key))
                {
                    grid.Add(key, null);
                }
            }
        }
    }

    private Vector3 GetGridPositionOfPoint(int i, int j)
    {
        var stepX = rect.rect.width / cols * ComputerScreen.Instance.ScreenRelation.x;
        var startX = rect.position.x + (stepX / 2f);

        var barsHeight = (windowTopBar != null) ?
            0 // Top bar not included in the container, so it is not neccesary to substract
            : FindObjectOfType<ToolBar>().GetComponent<RectTransform>().rect.height +
            GameObject.Find("BottomBar").GetComponent<RectTransform>().rect.height;

        var stepY = (rect.rect.height - barsHeight) / rows * ComputerScreen.Instance.ScreenRelation.y;
        var startY = (
            (windowTopBar != null) ?
            rect.position.y :
            rect.position.y + GameObject.Find("BottomBar").GetComponent<RectTransform>().rect.height * ComputerScreen.Instance.ScreenRelation.y  +
            GameObject.Find("BottomBar").GetComponent<RectTransform>().position.y
            ) + (stepY / 2f) + stepY * (rows - 1);

        Vector3 position = new Vector3(
            startX + stepX * i, startY - stepY * j, 0);
        return position;
    }

    public virtual void PositionateIcons()
    {

    }

    private void InitializeInnerContainers()
    {
        foreach (var childContainer in GetComponentsInChildren<IconsGridContainer>())
        {
            if (childContainer != this)
            {
                childContainer.LoadState();
            }
        }
    }

    public override bool MoveIconTo(Icon icon, Vector3 pos)
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
            icon.transform.SetAsFirstSibling();
            icon.Container = this;
        }
        else
        {
            Debug.Log($"Error Moving Icon to grid pos {gridPos} in folder {gameObject.name}. " +
                $"Either pos does not exist or pos is already occupied");
        }
    }

    public override void RemoveIconIfAlreadyExists(Icon icon)
    {
        if (grid.ContainsValue(icon))
        {
            FolderPosition key = grid.FirstOrDefault(g => g.Value == icon).Key;
            grid[key] = null;
            Serialize();
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

    protected override List<Icon> Icons()
    {
        return grid.Values.ToList();
    }

    #region Serialization

    public override void Serialize()
    {
        base.Serialize();
        Dictionary<string, string> serialized = new Dictionary<string, string>();

        serialized.Add($"{ID}", ID);
        serialized.Add($"{ID}_rows", rows.ToString());
        serialized.Add($"{ID}_cols", cols.ToString());

        foreach (var item in grid.Where(pos => pos.Value != null))
        {
            serialized.Add($"{ID}_iconAt_{item.Key.gridPosition.x}_{item.Key.gridPosition.y}", item.Value.ID);

        }

        SaveManager.Instance.Save(serialized, ID);
    }

    public override void Deserialize()
    {
        base.Deserialize();
        rows = int.Parse(SaveManager.Instance.RetrieveString($"{ID}_rows"));
        cols = int.Parse(SaveManager.Instance.RetrieveString($"{ID}_cols"));

        InitializeGrid();

        Dictionary<string, string> iconsIDs = SaveManager.Instance.RetrieveStringThatContains($"{ID}_iconAt_");

        foreach (var iconID in iconsIDs)
        {
            Icon icon = Instantiate(iconPrefab, transform);
            icon.ID = iconID.Value;
            icon.Deserialize();
            string[] splittedId = iconID.Key.Split('_');

            MoveIconTo(icon, new Vector2Int(int.Parse(splittedId[2]), int.Parse(splittedId[3])));
        }
    }

    #endregion

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
                Vector3 position = GetGridPositionOfPoint(i, j);

                FolderPosition key = new FolderPosition(new Vector2Int(i, j), position);

                if (grid.ContainsKey(key) && grid[key] != null)
                {
                    Gizmos.DrawCube(key.absolutePosition, Vector3.one * 15);
                }
                else
                {
                    if (i == 0 && j == 0)
                    {
                        Gizmos.DrawSphere(key.absolutePosition, 5);
                    }
                    else
                    {
                        Gizmos.DrawSphere(key.absolutePosition, 15);
                    }
                }

            }
        }
    }
#endif
}
