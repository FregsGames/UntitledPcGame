using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickManager : MonoBehaviour
{
    [SerializeField]
    private UIEventManager uIEventManager;

    private Icon previousSelectedIcon;

    [SerializeField]
    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    private void Start()
    {
        eventSystem = EventSystem.current;
    }

    private void ActivateHoverBackground(Icon icon)
    {
        if (icon?.BackgroundHover == null)
            return;

        icon?.BackgroundHover.SetActive(true);
        if ((icon == null && previousSelectedIcon != null) || (icon != null && previousSelectedIcon != null && icon != previousSelectedIcon))
        {
            previousSelectedIcon.BackgroundHover.SetActive(false);
        }
        previousSelectedIcon = icon;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            List<RaycastResult> results = GetWhatsUnderMouse();
            ActivateHoverBackground(null);
            foreach (var item in results)
            {
                Icon icon = item.gameObject.GetComponent<Icon>();
                if (icon != null)
                {
                    ActivateHoverBackground(icon);
                    break;
                }

                App app = item.gameObject.GetComponent<App>();
                if (app != null && app.Type != App.AppType.Desktop)
                {
                    app.Root.SetAsLastSibling();
                    ActivateHoverBackground(null);
                    break;
                }

                WindowTopBar windowTopBar = item.gameObject.GetComponent<WindowTopBar>();
                if (windowTopBar != null && windowTopBar.App != null && windowTopBar.App.Type != App.AppType.Desktop)
                {
                    windowTopBar.App.Root.SetAsLastSibling();
                    ActivateHoverBackground(null);
                    break;
                }
            }
        }
    }

    private List<RaycastResult> GetWhatsUnderMouse()
    {
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);
        return results;
    }
}
