using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faucet : MonoBehaviour
{
    [SerializeField] private int _tokenAmount;
    private int mutexLock;

    private void Start()
    {
        mutexLock = 1;
    }

    private void OnMouseDown()
    {
        if (mutexLock == 1)
        {
            GivePlayerToken(_tokenAmount);
            mutexLock = -1;
        }

    }

    private async void GivePlayerToken(int num)
    {
        var transaction = await SDKManager.Instance.Claim(_tokenAmount);
        mutexLock = 1;
    }
}
