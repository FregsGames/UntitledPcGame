using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconsContainer : App
{
    [SerializeField]
    protected Icon iconPrefab;

    public virtual void RemoveIconIfAlreadyExists(Icon icon)
    {

    }

    public virtual bool MoveIconTo(Icon icon, Vector3 pos)
    {
        return false;
    }
    protected virtual List<Icon> Icons()
    { 
        return new List<Icon>();
    }

    protected bool IconContainsContainerRecursive(Icon icon)
    {
        if (icon != null && icon.AssociatedApp != null)
        {
            if (icon.AssociatedApp == this)
            {
                return true;
            }
            else
            {
                if (icon.AssociatedApp is IconsContainer)
                {
                    foreach (var subIcon in ((IconsContainer)icon.AssociatedApp).Icons())
                    {
                        bool isSubContainerOfIcon = IconContainsContainerRecursive(subIcon);

                        if (isSubContainerOfIcon)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
}