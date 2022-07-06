using System.Threading.Tasks;
using UnityEngine;

public class Booter : MonoBehaviour
{
    [SerializeField]
    private BootScreen bootScreen;
    [SerializeField]
    private bool loadFirstTimeGameIfNotSaveFile;

    [SerializeField]
    private bool showBootScreen = true;

    private async Task Start()
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

        await bootScreen.LoadBarTo(0.4f);

        FolderManager.Instance.Initialize();
        AlarmsManager.Instance.LoadSettings();

        await bootScreen.LoadBarTo(0.6f);

        if (!saveExists || !loadFirstTimeGameIfNotSaveFile)
        {
            Debug.Log("Save file not found, loading first time game");
            await bootScreen.LoadBarTo(0.8f);
            FirstGameLoader.Instance.LoadFirstGameData();
        }
        else
        {
            Debug.Log("Save file found");
        }

        await bootScreen.LoadBarTo(1f);
        await bootScreen.HideBootScreen();
    }
}
