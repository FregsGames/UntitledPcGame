using UnityEngine;
using UnityEngine.UI;

public class ComputerScreen : Singleton<ComputerScreen>
{
    [SerializeField]
    private GraphicRaycaster raycaster;
    [SerializeField]
    private RectTransform bottomBar;
    [SerializeField]
    private RectTransform background;
    [SerializeField]
    private BlockPanel blockPanel;
    [SerializeField]
    private Transform desktop;

    [SerializeField]
    private UIEventManager eventManager;

    [Header("UI Scaling")]
    [SerializeField]
    private float uiScale = 1f;
    [SerializeField]
    private Vector2 iconsBaseSize = new Vector2(100, 100);
    [SerializeField]
    private float bottomBarBaseSize = 108f;
    [SerializeField]
    private float defaultIconTextSize = 30f;

    public float BottomBarHeight { get => bottomBar.rect.height; }
    public GraphicRaycaster GraphicRaycaster { get => raycaster; }
    public Vector2 IconsBaseSize { get => iconsBaseSize; }
    public float DefaultIconTextSize { get => defaultIconTextSize; }
    public Vector2 BackgroundSize { get => background.rect.size; }
    public BlockPanel BlockPanel { get => blockPanel; }
    public Transform Desktop { get => desktop; }

    private void OnEnable()
    {
        eventManager.OnUIScaleChanged += UpdateBottomBarSize;
    }

    private void OnDisable()
    {
        eventManager.OnUIScaleChanged -= UpdateBottomBarSize;
    }

    private void UpdateBottomBarSize(float uiScale)
    {
        bottomBar.sizeDelta = new Vector2(bottomBar.sizeDelta.x, bottomBarBaseSize * uiScale);
        background.offsetMin = new Vector2(0, bottomBarBaseSize * uiScale);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    uiScale += 0.05f;
        //    eventManager.OnUIScaleChanged?.Invoke(uiScale);
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    uiScale -= 0.05f;
        //    eventManager.OnUIScaleChanged?.Invoke(uiScale);
        //}
    }

    protected override void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
}
