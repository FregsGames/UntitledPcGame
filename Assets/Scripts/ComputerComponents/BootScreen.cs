using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

public class BootScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject container;
    [SerializeField]
    private Image loadingBar;
    [SerializeField]
    private Image background;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private GraphicRaycaster raycaster;


    [Header("Parameters")]
    [SerializeField]
    private float backgroundFadeTime = 0.5f;

    [SerializeField]
    private float loadingBarTime = 0.5f;

    public async Task ShowBootScreen()
    {
        raycaster.enabled = true;
        canvasGroup.alpha = 1;
        loadingBar.fillAmount = 0;
        container.SetActive(true);

        await loadingBar.DOFillAmount(1f, loadingBarTime).AsyncWaitForCompletion();

    }
    public async Task HideBootScreen()
    {
        raycaster.enabled = false;
        await canvasGroup.DOFade(0, backgroundFadeTime).AsyncWaitForCompletion();
    }

}
