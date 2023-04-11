using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveHint : MonoBehaviour
{
    [SerializeField] private GameObject _hintObj;
    [SerializeField] private GameObject _uiBoard;
    [SerializeField] private Vector3 _uiBoardOffset;
    [SerializeField] private GameObject _clickEnableUI;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnMouseEnter()
    {
        _hintObj.SetActive(true);
        _uiBoard.SetActive(true);
    }

    private void OnMouseExit()
    {
        _hintObj.SetActive(false);
        _uiBoard.SetActive(false);
    }

    private void Update()
    {
        if (_uiBoard.activeSelf)
        {
            Vector3 posOnScreen = _camera.WorldToScreenPoint(transform.position);
            _uiBoard.transform.position = posOnScreen + _uiBoardOffset;
        }
    }

    private void OnMouseUp()
    {
        _clickEnableUI.SetActive(true);
        _uiBoard.SetActive(false);
        InGameManager.Instace.EnableAllInteractiveObjects(false);
    }
}
