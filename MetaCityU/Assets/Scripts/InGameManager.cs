using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private PlayerCharacterController _characterController;
    [SerializeField] private CampusGate _campusGate;
    [SerializeField] private CanvasManager _canvasManager;

    public static InGameManager Instace;

    private bool _waitCameraEnablePlayer;

    private void Awake()
    {
        if (Instace != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instace = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        _waitCameraEnablePlayer = false;
    }

    private void Update()
    {
        if (_waitCameraEnablePlayer && _cameraManager.LerpPlayerFinish())
        {
            _waitCameraEnablePlayer = false;
            _characterController.DisableController(false);
        }
    }

    public async void RefreshBalance()
    {
        await SDKManager.Instance.CheckBalance();
    }

    public void EnterCampus()
    {
        _cameraManager.EnterCampus();
        _characterController.DisableController(true);
        _canvasManager.EnterCampus();
    }

    public void QuitCampus()
    {
        _cameraManager.ExitCampus();
        _waitCameraEnablePlayer = true;
        _campusGate.ExitGate();
        _canvasManager.ExitCampus();
    }
}
