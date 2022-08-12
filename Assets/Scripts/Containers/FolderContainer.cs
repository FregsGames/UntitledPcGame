using System.Linq;
using UnityEngine;

public class FolderContainer : IconsContainer
{
    public override void PositionateIcons()
    {
        Icon[] icons = GetComponentsInChildren<Icon>();

        foreach (var icon in icons)
        {
            FolderPosition assignedPos = GetFirstFreeSlot();

            if (assignedPos.absolutePosition.x != -1)
            {
                icon.Container = this;
                grid[assignedPos] = icon;
            }

            icon.SetPos(assignedPos.absolutePosition);
        }
    }

    public override void Close()
    {
        Serialize();
        transform.parent.gameObject.SetActive(false);
        systemEventManager.OnAppClosed?.Invoke(ID);
    }

    private FolderPosition GetFirstFreeSlot()
    {
        FolderPosition firstFreePosition = grid.FirstOrDefault(g => g.Value == null).Key;

        return firstFreePosition;
    }

    public override bool MoveIconTo(Icon icon, Vector3 pos)
    {
        if (IconContainsContainerRecursive(icon) || grid.ContainsValue(icon) || MaxSubfolderReached(icon))
            return false;

        FolderPosition assignedPos = GetFirstFreeSlot();

        if (assignedPos.absolutePosition.x != -1)
        {
            RemoveIconIfAlreadyExists(icon);
            grid[assignedPos] = icon;
            icon.SetPos(assignedPos.absolutePosition);
            icon.transform.SetParent(transform);

            Serialize();

            return true;
        }
        return false;
    }

    private bool MaxSubfolderReached(Icon icon)
    {
        return false;
    }
}
