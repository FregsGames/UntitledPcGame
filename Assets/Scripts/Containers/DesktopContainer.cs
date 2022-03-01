using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DesktopContainer : IconsContainer
{
    protected override void PositionateIcons()
    {
        foreach (Transform child in transform)
        {
            Icon icon = child.gameObject.GetComponent<Icon>();
            if (icon == null)
                continue;

            FolderPosition assignedPos = GetClosestFreeSlot(icon.Position);

            if (assignedPos.absolutePosition.x != -1)
            {
                icon.Container = this;
                grid[assignedPos] = icon;
            }

            icon.SetPos(assignedPos.absolutePosition);
        }
    }

    public override bool MoveIconTo(Icon icon, Vector3 pos)
    {
        FolderPosition assignedPos = GetClosestFreeSlot(pos, icon);

        if (assignedPos.absolutePosition.x != -1)
        {
            RemoveIconIfAlreadyExists(icon);
            grid[assignedPos] = icon;
            icon.SetPos(assignedPos.absolutePosition);
            icon.transform.SetParent(transform);

            return true;
        }
        return false;
    }
}
