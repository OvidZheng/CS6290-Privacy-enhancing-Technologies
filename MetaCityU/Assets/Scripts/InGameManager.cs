using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public async void RefreshBalance()
    {
        await SDKManager.Instance.CheckBalance();
    }
}
