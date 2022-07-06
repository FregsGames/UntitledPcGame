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

    public void ShowBootScreen()
    {
        raycaster.enabled = true;
        canvasGroup.alpha = 1;
        ResetLoadBar();
        container.SetActive(true);
    }

    public async Task LoadBarTo(float to)
    {
        await loadingBar.DOFillAmount(to, (to- loadingBar.fillAmount) * loadingBarTime).SetEase(Ease.Linear).AsyncWaitForCompletion();
    }

    public void ResetLoadBar()
    {
        loadingBar.fillAmount = 0;
    }

    public async Task HideBootScreen()
    {
        raycaster.enabled = false;
        await canvasGroup.DOFade(0, backgroundFadeTime).AsyncWaitForCompletion();
    }

}
