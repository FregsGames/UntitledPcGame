using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class FolderContainer : IconsContainer
{
    [SerializeField]
    private RectTransform container;
    [SerializeField]
    private int iconSize = 75;
    [SerializeField]
    private GridLayoutGroup gridLayout;

    public override void Close()
    {
        Serialize();
        root.gameObject.SetActive(false);
        systemEventManager.OnAppClosed?.Invoke(ID);
    }

    public override void Open()
    {
        base.Open();
    }

    private async void OnEnable()
    {
        await UniTask.DelayFrame(10);
        SetupLayoutSizes();
    }

    public override bool MoveIconTo(Icon icon, Vector3 pos)
    {
        if (IconContainsContainerRecursive(icon) || Icons().Contains(icon))
            return false;

        if (icon != null)
        {
            icon.transform.SetParent(container);
            icon.Container = this;
            SetupLayoutSizes();
            return true;
        }

        return false;
    }

    public override void RemoveIconIfAlreadyExists(Icon icon)
    {

    }

    protected override List<Icon> Icons()
    {
        List<Icon> icons = new List<Icon>();

        foreach (Transform transform in container)
        {
            var icon = transform.gameObject.GetComponentInChildren<Icon>();
            if (icon != null)
            {
                icons.Add(icon);
            }
        }

        return icons;
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
            SerializeFirstDepthChildren();
            Serialize();
            InitializeInnerContainers();
        }
    }

    private void RemoveChildren()
    {

    }

    public void SetupLayoutSizes()
    {
        int elementsPerRow = CountElementsPerRow();
        var padding = gridLayout.padding.left + gridLayout.padding.right;

        // w = n * size + (n - 1) * s + P

        var spacing = (container.rect.width - padding - elementsPerRow * (iconSize)) / (elementsPerRow - 1);
        gridLayout.spacing = new Vector2(spacing, iconSize);
    }


    private int CountElementsPerRow()
    {
        if (container.childCount == 0)
            return 0;

        var padding = gridLayout.padding.left + gridLayout.padding.right;
        return Mathf.FloorToInt(container.rect.width - padding + iconSize) / (iconSize + iconSize);
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

    private void InitializeInnerContainers()
    {
        foreach (var childContainer in GetComponentsInChildren<FolderContainer>())
        {
            if (childContainer != this)
            {
                childContainer.LoadState();
            }
        }
    }

    #region Serialization

    public override void Serialize()
    {
        base.Serialize();
        Dictionary<string, string> serialized = new Dictionary<string, string>();

        serialized.Add($"{ID}", ID);
        serialized.Add($"{ID}_xsize", root.GetComponent<RectTransform>().sizeDelta.x.ToString());
        serialized.Add($"{ID}_ysize", root.GetComponent<RectTransform>().sizeDelta.y.ToString());

        foreach (Transform element in container)
        {
            Icon icon = element.GetComponent<Icon>();
            serialized.Add($"{ID}_icon_{icon.ID}", icon.ID);

        }

        SaveManager.Instance.Save(serialized, ID);
    }

    public override void Deserialize()
    {
        base.Deserialize();
        var width = float.Parse(SaveManager.Instance.RetrieveString($"{ID}_xsize"));
        var height = float.Parse(SaveManager.Instance.RetrieveString($"{ID}_ysize"));

        root.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

        Dictionary<string, string> iconsIDs = SaveManager.Instance.RetrieveStringThatContains($"{ID}_icon_");

        foreach (var iconID in iconsIDs)
        {
            Icon icon = Instantiate(iconPrefab, transform);
            icon.ID = iconID.Value;
            icon.Deserialize();

            MoveIconTo(icon, Vector3.zero);
        }

        SetupLayoutSizes();
    }

    #endregion

}
