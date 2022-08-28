using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;

public class Booter : MonoBehaviour
{
    [SerializeField]
    private BootScreen bootScreen;
    [SerializeField]
    private bool loadFirstTimeGameIfNotSaveFile;
    [SerializeField]
    private Texture2D defaultCursor;

    [SerializeField]
    private bool showBootScreen = true;


    public async UniTask Start()
    {
        if (showBootScreen)
        {
            bootScreen.ShowBootScreen();
            await bootScreen.LoadBarTo(0.2f);
        }

        bool saveExists = SaveManager.Instance.LoadSave();
        LockManager.Instance.LoadSettings();
        Computer.Instance.Desktop.LoadState();
        Computer.Instance.NotificationCenter.Initialize();
        Computer.Instance.ComputerSettings.Initialize();
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);

        if (showBootScreen)
        { 
            await bootScreen.LoadBarTo(0.4f);
        }

        AlarmsManager.Instance.LoadSettings();

        if (showBootScreen)
        {
            await bootScreen.LoadBarTo(0.6f);
        }

        if (!saveExists && loadFirstTimeGameIfNotSaveFile)
        {
            Debug.Log("Save file not found, loading first time game");
            if (showBootScreen)
            {
                await bootScreen.LoadBarTo(0.8f);
            }
            FirstGameLoader.Instance.LoadFirstGameData();
        }
        else
        {
            FolderManager.Instance.Initialize();
            Debug.Log("Save file found");
        }

        if (showBootScreen)
        {
            await bootScreen.LoadBarTo(1);
        }
        await bootScreen.HideBootScreen();
    }
}
