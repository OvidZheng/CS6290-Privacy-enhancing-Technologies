using UnityEngine;
using Thirdweb;

public class SDKManager : MonoBehaviour
{
    /// <summary>
    /// Manager单例
    /// </summary>
    public static SDKManager Instance;
    
    public ThirdwebSDK SDK;

    private void Awake()
    {
        //单例已经实现则销毁冗余
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        #if UNITY_EDITOR
        return;
        #endif
        
        SDK = new ThirdwebSDK("goerli");
    }
}