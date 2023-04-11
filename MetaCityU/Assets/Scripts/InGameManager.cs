using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private CampusGate _campusGate;
    [SerializeField] private CanvasManager _canvasManager;
    [SerializeField] private List<GameObject> _interactiveObjects;

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
        EnableAllInteractiveObjects(false);
    }

    private void Update()
    {
        if (_waitCameraEnablePlayer && _cameraManager.LerpPlayerFinish())
        {
            _waitCameraEnablePlayer = false;
            PlayerCharacterController.Local.DisableController(false);
        }
    }

    public async void RefreshBalance()
    {
        await SDKManager.Instance.CheckBalance();
    }

    public void EnterCampus()
    {
        _cameraManager.EnterCampus();
        EnableAllInteractiveObjects(true);
        PlayerCharacterController.Local.DisableController(true);
        _canvasManager.EnterCampus();
    }

    public void QuitCampus()
    {
        _cameraManager.ExitCampus();
        EnableAllInteractiveObjects(false);
        _waitCameraEnablePlayer = true;
        _campusGate.ExitGate();
        _canvasManager.ExitCampus();
    }
    
    public void EnableAllInteractiveObjects(bool enable)
    {
        foreach (var interactiveObject in _interactiveObjects)
        {
            interactiveObject.SetActive(enable);
        }
    }
}
