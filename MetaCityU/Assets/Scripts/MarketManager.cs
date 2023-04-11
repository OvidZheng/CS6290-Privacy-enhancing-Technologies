using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public static MarketManager Instance;
    [SerializeField] private GameObject _shopUI;
    private int mutexLock;
    
    private void Awake()
    {
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
    private void Start()
    {
        mutexLock = 0;
        CloseShopUI();
    }

    public void BuyChaShao()
    {
        if(mutexLock == 0)
        {
            TryGetChaShao();
            mutexLock = 1;
        }
    }

    public void OpenShopUI()
    {
        InGameManager.Instace.EnableAllInteractiveObjects(false);
        _shopUI.SetActive(true);
    }

    public void CloseShopUI()
    {
        InGameManager.Instace.EnableAllInteractiveObjects(true);
        _shopUI.SetActive(false);
    }

    private async void TryGetChaShao()
    {
        var transaction = await SDKManager.Instance.BuyItem("1");
        mutexLock = 0;
    }
}
