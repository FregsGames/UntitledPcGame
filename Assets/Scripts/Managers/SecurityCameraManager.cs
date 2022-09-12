using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class SecurityCameraManager : SerializedSingleton<SecurityCameraManager>
{
    [SerializeField]
    private Dictionary<string, RenderTexture> securityCameras = new Dictionary<string, RenderTexture>();

    public async UniTask<RenderTexture> LoadSecurityCamera(string name)
    {
        if (!securityCameras.ContainsKey(name))
        {
            return null;
        }

        await SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);

        return securityCameras[name];
    }

    public async void CloseCamera(string name)
    {
        await SceneManager.UnloadSceneAsync(name);
    }
}
