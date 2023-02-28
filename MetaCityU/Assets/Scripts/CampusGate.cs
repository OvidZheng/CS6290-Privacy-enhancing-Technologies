using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampusGate : MonoBehaviour
{
    private bool hasEntered;

    private void Start()
    {
        hasEntered = false;
    }

    private void OnMouseDown()
    {
        if (!hasEntered)
        {
            InGameManager.Instace.EnterCampus();
            hasEntered = true;
        }
        
    }

    public void ExitGate()
    {
        hasEntered = false;
    }
}
