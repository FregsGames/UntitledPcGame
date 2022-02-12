using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class IconsContainer : MonoBehaviour
{
    [SerializeField]
    private int rows;
    [SerializeField]
    private int cols;
    [SerializeField]
    private RectTransform rect;

    private Dictionary<Vector3, Icon> grid = new Dictionary<Vector3, Icon>();

#if UNITY_EDITOR
    [SerializeField]
    private bool gizmosOn = true;
#endif

    private void OnValidate()
    {
        InitializeGrid();
        PositionateIcons();

        SceneView.RepaintAll();
    }

    public bool MoveIconTo(Icon icon, Vector3 pos)
    {
        Vector3 assignedPos = GetClosestFreeSlot(pos, icon);

        if (assignedPos.x != -1)
        {
            RemoveIcon(icon);
            grid[assignedPos] = icon;
            icon.SetPos(assignedPos);
            icon.transform.SetParent(transform);

            return true;
        }

        return false;
    }

    public void RemoveIcon(Icon icon)
    {
        if (grid.ContainsValue(icon))
        {
            Vector3 key = grid.FirstOrDefault(g => g.Value == icon).Key;
            grid[key] = null;
        }
    }

    private void PositionateIcons()
    {
        Icon[] icons = GetComponentsInChildren<Icon>();

        foreach (var icon in icons)
        {
            Vector3 assignedPos = GetClosestFreeSlot(icon.Position);

            if (assignedPos.x != -1)
            {
                icon.Container = this;
                grid[assignedPos] = icon;
            }

            icon.SetPos(assignedPos);
        }
    }

    private void InitializeGrid()
    {
        grid.Clear();

        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Vector3 key = new Vector3(rect.position.x + rect.rect.width / (cols + 1) * (i + 1), rect.position.y + rect.rect.height / (rows + 1) * (j + 1), 0);
                if (!grid.ContainsKey(key))
                {
                    grid.Add(key, null);
                }
            }
        }
    }

    private Vector3 GetClosestFreeSlot(Vector3 pos, Icon icon = null)
    {
        List<Vector3> freeSlots = grid.Where(g => g.Value == null || g.Value == icon).Select(g => g.Key).ToList();

        if(freeSlots.Count == 0)
        {
            return Vector3.one * -1;
        }
        else
        {
            int closestIndex = 0;

            for (int i = 0; i < freeSlots.Count; i++)
            {
                if(Vector3.Distance(freeSlots[i], pos) < Vector3.Distance(freeSlots[closestIndex], pos))
                {
                    closestIndex = i;
                }
            }
            return freeSlots[closestIndex];
        }

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!gizmosOn)
            return;

        Gizmos.color = Color.red;

        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Vector3 key = new Vector3(rect.position.x + rect.rect.width / (cols + 1) * (i + 1), rect.position.y + rect.rect.height / (rows + 1) * (j + 1), 0);

                if (grid.ContainsKey(key) && grid[key] != null)
                {
                    Gizmos.DrawCube(key, Vector3.one * 20);
                }
                else
                {
                    Gizmos.DrawSphere(key, 20);
                }

            }
        }
    }
#endif
}
