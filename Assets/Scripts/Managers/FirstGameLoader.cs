using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class FirstGameLoader : SerializedSingleton<FirstGameLoader>
{
    [SerializeField]
    private GameObject iconPrefab;
    [Header("Debug")]
    [SerializeField]
    private (int,int) firstAlarmTime;

    public void LoadFirstGameData()
    {
        CreateFirstAlarm();
        CreateLockForSecurityCameras();
        LoadDesktopIcons();
        LockManager.instance.AddLock("wifi", false, "4539");
    }
    private void CreateFirstAlarm()
    {
        var alarmTime = DateTime.Now.AddMinutes(2);
        firstAlarmTime = (alarmTime.Hour, alarmTime.Minute);

        AlarmData alarmData = new AlarmData()
        {
            description = "Alarma con información importante. NO BORRAR",
            enabled = true,
            id = Guid.NewGuid().ToString(),
            time = firstAlarmTime
        };
        AlarmsManager.Instance.OnAlarmCreated?.Invoke(alarmData);
        LockManager.instance.AddLock("admin", false, firstAlarmTime.Item1.ToString() + firstAlarmTime.Item2.ToString());
    }

    private void CreateLockForSecurityCameras()
    {
        LockManager.Instance.ClearAll();

        LockManager.Instance.AddLock(App.AppType.SecurityCameras, firstAlarmTime.Item1.ToString() + firstAlarmTime.Item2.ToString());
        FindObjectsOfType<Icon>().FirstOrDefault(i => i.AssociatedAppType == App.AppType.SecurityCameras).SetLock(true);
    }

    private void LoadDesktopIcons()
    {
        Computer.Instance.Desktop.RemoveChildren();
        FolderManager.Instance.Initialize();
        var cameraIcon = Instantiate(iconPrefab, Computer.Instance.Desktop.transform).GetComponent<Icon>();
        cameraIcon.RegenerateGUID();
        cameraIcon.Setup(App.AppType.SecurityCameras, "Security Cameras", "camera", true);

        var personalNotes = Instantiate(iconPrefab, Computer.Instance.Desktop.transform).GetComponent<Icon>();
        personalNotes.RegenerateGUID();
        personalNotes.AssociatedAppID = "personal-notes";
        personalNotes.Setup(App.AppType.TextFile, "Personal notes", "info", false);
        SaveManager.Instance.Save("personal-notes_content", "Estas son tus notas personales.");

        var folder = Instantiate(iconPrefab, Computer.Instance.Desktop.transform).GetComponent<Icon>();
        folder.RegenerateGUID();
        folder.AssociatedAppID = "main-folder";
        folder.Setup(App.AppType.Folder, "Mis archivos", "folder", false);
        SaveManager.Instance.Save("main-folder", "main-folder");

        Computer.Instance.Desktop.MoveIconTo(cameraIcon, Vector2.zero);
        Computer.Instance.Desktop.MoveIconTo(personalNotes, Vector2.zero);
        Computer.Instance.Desktop.MoveIconTo(folder, Vector2.zero);

        Computer.Instance.CreateFile("manuals", "Manuales", App.AppType.Folder, "main-folder");
        Computer.Instance.CreateFile("network-manual-icon", "Manual del red", App.AppType.TextFile, "manuals-folder", "network-manual-textfile");
        Computer.Instance.CreateFile("pc-manual-icon", "Manual del Pc", App.AppType.TextFile, "manuals-folder", "pc-manual-textfile");

        SaveManager.Instance.Save("network-manual-textfile_content", "Bienvenido al manual de configuración de red de Moon Os. Con este manual podrás Lorem ipsum dolor sit amet consectetur adipiscing elit ultricies dictum, suscipit augue suspendisse sodales facilisis ullamcorper hendrerit scelerisque eu sollicitudin, lectus dapibus ut aptent pharetra placerat conubia nisl. Proin torquent ad dictum eleifend purus tellus velit sapien, commodo pretium libero consequat odio magna sem, nostra varius habitasse curabitur gravida scelerisque sagittis. Mauris blandit facilisis nostra iaculis suspendisse condimentum, hendrerit turpis conubia urna eros tortor, pretium hac vulputate commodo auctor.");
    }
}
