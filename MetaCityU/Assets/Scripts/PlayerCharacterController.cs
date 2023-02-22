using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))] 
public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector2 _viewRotateSpeed;
    private NavMeshAgent _agent;

    private Vector3 _freshPlayerToCam;
    private Vector3 _mouseDragPreviousMousePosition;
    private bool _isMouseDragging;
    
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        _isMouseDragging = false;
        _freshPlayerToCam = _camera.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessMouseInput();
    }


    private void ProcessMouseInput()
    {
        _camera.transform.position = transform.position + _freshPlayerToCam;

        ProcessLeftMouse();
        ProcessRightMouse();
        
        _freshPlayerToCam = _camera.transform.position - transform.position;
    }

    private void ProcessRightMouse()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _isMouseDragging = true;
            _mouseDragPreviousMousePosition = Input.mousePosition;
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            _isMouseDragging = false;
        }

        if (_isMouseDragging && Vector3.Distance(_mouseDragPreviousMousePosition, Input.mousePosition) > 0.001f)
        {
            Vector3 camToPlayer = transform.position - _camera.transform.position;
            camToPlayer.z = 0;
            Vector3 mouseMoveVector = Input.mousePosition - _mouseDragPreviousMousePosition;
            Vector3 myPosition = transform.position;
            Transform cameraTransform = _camera.transform;
            
            _camera.transform.RotateAround(myPosition, Vector3.up, mouseMoveVector.x * _viewRotateSpeed.x / 100);
            _camera.transform.RotateAround(myPosition, Vector3.Cross(camToPlayer, Vector3.up), mouseMoveVector.y * _viewRotateSpeed.y / 100);
            _camera.transform.rotation = Quaternion.Euler(cameraTransform.rotation.eulerAngles.x, cameraTransform.eulerAngles.y, 0);
            _mouseDragPreviousMousePosition = Input.mousePosition;
        }
    }

    private void ProcessLeftMouse()
    {
        //左键
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseDirectionRay = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hitResult = Physics.Raycast(mouseDirectionRay, out hit, Single.PositiveInfinity,
                LayerMask.GetMask("Ground"));

            if (hitResult)
            {
                _agent.SetDestination(hit.point);
            }
        }
    }
}
