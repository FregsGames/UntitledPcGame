using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DragGhost : MonoBehaviour
{
    [SerializeField]
    private Image image;

    private bool ghostActive = false;

    [SerializeField]
    private UIEventManager eventManager;

    private void OnEnable()
    {
        eventManager.OnIconStartDrag += StartGhost;
        eventManager.OnEndStartDrag += EndGhost;
    }

    private void OnDisable()
    {
        eventManager.OnIconStartDrag -= StartGhost;
        eventManager.OnEndStartDrag -= EndGhost;
    }

    public void StartGhost(Icon icon)
    {
        ghostActive = true;
        image.sprite = icon.Sprite;
        image.gameObject.SetActive(true);
        image.color = new Color(icon.SpriteColor.r, icon.SpriteColor.g, icon.SpriteColor.b, 0.5f);
        StartCoroutine(MoveGhost());
    }

    public void EndGhost(Icon icon)
    {
        ghostActive = false;
        image.gameObject.SetActive(false);
    }

    private IEnumerator MoveGhost()
    {
        while (ghostActive)
        {
            image.GetComponent<RectTransform>().position = Input.mousePosition;
            yield return 0;
        }
    }
}
