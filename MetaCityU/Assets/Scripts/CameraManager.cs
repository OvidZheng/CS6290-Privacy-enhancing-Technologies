using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private Transform _campusCameraTrans;

    private bool _lerpToCampus;
    private Vector3 _playerCamPosition;
    private Quaternion _playerCamRotation;
    private bool _lerpToPlayer;
    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        _lerpToCampus = false;
        _lerpToPlayer = false;
    }

    public void EnterCampus()
    {
        _lerpToCampus = true;
        _lerpToPlayer = false;
        _playerCamPosition = transform.position;
        _playerCamRotation = transform.rotation;
    }

    public void ExitCampus()
    {
        _lerpToPlayer = true;
        _lerpToCampus = false;
    }
    
    private void Update()
    {
        if (_lerpToCampus)
        {
            Vector3 myPos = transform.position;
            transform.position = Vector3.Lerp(myPos, _campusCameraTrans.position, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, _campusCameraTrans.rotation, Time.deltaTime);

            if (Vector3.Distance(myPos, _campusCameraTrans.position) < 1f)
            {
                _lerpToCampus = false;
            }
        }
        
        if (_lerpToPlayer)
        {
            Vector3 myPos = transform.position;
            float distToDest = Vector3.Distance(myPos, _playerCamPosition);
            transform.position = Vector3.Lerp(myPos, _playerCamPosition, Time.deltaTime * ((distToDest > 5f)? 1 : 5/distToDest));
            transform.rotation = Quaternion.Lerp(transform.rotation, _playerCamRotation, Time.deltaTime * ((distToDest > 5f)? 1 : 4));

            if (distToDest < 0.01f)
            {
                _lerpToPlayer = false;
            }
        }
    }

    public bool LerpPlayerFinish()
    {
        return !_lerpToPlayer;
    }
}
