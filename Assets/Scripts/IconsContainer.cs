using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class IconsContainer : MonoBehaviour
{
    [SerializeField]
    private int rows;
    [SerializeField]
    private int cols;
    [SerializeField]
    private RectTransform rect;
    [SerializeField]
    private WindowTopBar windowTopBar;

    private Dictionary<FolderPosition, Icon> grid = new Dictionary<FolderPosition, Icon>();


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

    private void RefreshGridPositions()
    {
        InitializeGrid();
        PositionateIcons();
    }


    public bool MoveIconTo(Icon icon, Vector3 pos)
    {
        FolderPosition assignedPos = GetClosestFreeSlot(pos, icon);

        if (assignedPos.absolutePosition.x != -1)
        {
            RemoveIcon(icon);
            grid[assignedPos] = icon;
            icon.SetPos(assignedPos.absolutePosition);
            icon.transform.SetParent(transform);

            return true;
        }

        return false;
    }

    public void RemoveIcon(Icon icon)
    {
        if (grid.ContainsValue(icon))
        {
            FolderPosition key = grid.FirstOrDefault(g => g.Value == icon).Key;
            grid[key] = null;
        }
    }

    private void PositionateIcons()
    {
        Icon[] icons = GetComponentsInChildren<Icon>();

        foreach (var icon in icons)
        {
            FolderPosition assignedPos = GetClosestFreeSlot(icon.Position);

            if (assignedPos.absolutePosition.x != -1)
            {
                icon.Container = this;
                grid[assignedPos] = icon;
            }

            icon.SetPos(assignedPos.absolutePosition);
        }
    }

    private void InitializeGrid()
    {
        grid.Clear();

        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Vector3 position = new Vector3(rect.position.x + rect.rect.width / (cols + 1) * (i + 1), rect.position.y + rect.rect.height / (rows + 1) * (j + 1), 0);
                FolderPosition key = new FolderPosition(new Vector2Int(i, j), position);

                if (!grid.ContainsKey(key))
                {
                    grid.Add(key, null);
                }
            }
        }
    }

    private FolderPosition GetClosestFreeSlot(Vector3 pos, Icon icon = null)
    {
        List<FolderPosition> freeSlots = grid.Where(g => g.Value == null || g.Value == icon).Select(g => g.Key).ToList();

        if (freeSlots.Count == 0)
        {
            return new FolderPosition(new Vector2Int(- 1, -1), Vector3.one * -1);
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

    public struct FolderPosition
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
    private void OnValidate()
    {
        InitializeGrid();
        PositionateIcons();

        SceneView.RepaintAll();
    }
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

                FolderPosition key = new FolderPosition(new Vector2Int(i , j), position);

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
