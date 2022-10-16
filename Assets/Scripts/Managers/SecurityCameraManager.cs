using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class SecurityCameraManager : SerializedSingleton<SecurityCameraManager>
{
    [SerializeField]
    private Dictionary<string, CameraInfo> securityCameras = new Dictionary<string, CameraInfo>();

    public Dictionary<string, CameraInfo> SecurityCameras { get => securityCameras; }

    public enum CameraVisibility {Ok, Hidden, Locked};

    public async UniTask<RenderTexture> LoadSecurityCamera(string name)
    {
        if (!SecurityCameras.ContainsKey(name))
        {
            return null;
        }

        await SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);

        return SecurityCameras[name].texture;
    }

    public async void CloseCamera(string name)
    {
        await SceneManager.UnloadSceneAsync(name);
    }

    public class CameraInfo
    {
        public CameraVisibility state;
        public RenderTexture texture;
        public string name;
    }
}

