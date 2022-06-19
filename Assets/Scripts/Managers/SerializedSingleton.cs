using Sirenix.OdinInspector;
using UnityEngine;

public class SerializedSingleton<T> : SerializedMonoBehaviour where T : SerializedMonoBehaviour
{
    protected static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    var singletonObj = new GameObject();
                    singletonObj.name = typeof(T).ToString();
                    instance = singletonObj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = GetComponent<T>();

        DontDestroyOnLoad(gameObject);
    }
}
