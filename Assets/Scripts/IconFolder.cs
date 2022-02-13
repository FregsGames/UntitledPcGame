using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IconFolder : Icon
{
    private IconsContainer folder;

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            folder = FolderManager.Instance.OpenFolder(Vector2.zero).GetComponentInChildren<IconsContainer>();
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (Input.mousePosition != originalPos && Container != null)
        {
            IconsContainer newContainer = GetContainerUnderMouse();
            {
                if (newContainer != null)
                {
                    if (newContainer == folder)
                        return;

                    if (newContainer.MoveIconTo(this, Input.mousePosition) && Container != newContainer)
                    {
                        Container.RemoveIcon(this);
                        Container = newContainer;
                    }
                    return;
                }

                Container.MoveIconTo(this, Input.mousePosition);
            }
        }
    }
}
