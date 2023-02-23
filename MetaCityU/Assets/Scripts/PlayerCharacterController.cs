using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))] 
public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector2 _viewRotateSpeed;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _pathObject;
    private NavMeshAgent _agent;

    private Vector3 _freshPlayerToCam;
    private Vector3 _mouseDragPreviousMousePosition;
    private bool _isMouseDragging;
    private NavMeshPath _navMeshPath;
    private List<Vector3> _drawCornerList;
    private Vector3 _currentDest;
    private Vector3 _currentStart;
    private bool _navPathIsFresh;
    private float _navProcessCountDown;
    private List<GameObject> _pathObjList;



    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        _isMouseDragging = false;
        _freshPlayerToCam = _camera.transform.position - transform.position;
        _navMeshPath = new NavMeshPath();
        _drawCornerList = new List<Vector3>();
        _pathObjList = new List<GameObject>();
        _navPathIsFresh = false;
    }

    // Update is called once per frame
    void Update()
    {
        //处理AI路径显示
        ShowPath();
        
        //处理鼠标输入
        ProcessMouseInput();
        
        //处理动画
        ProcessAnimator();
    }

    private void ShowPath()
    {
        //如果倒计时清0，那么处理一次路径
        if (_navProcessCountDown > 0)
        {
            _navProcessCountDown -= Time.deltaTime;
        }
        else if( _navProcessCountDown - (-1)  > 0.001f)
        {
            ProcessNavPath(); 
            
            //设置路径新鲜方便建立小球
            _navPathIsFresh = true;
            
            //设置为-1，强制停止处理
            _navProcessCountDown = -1;
        }

        if (_navPathIsFresh)
        {
            //摧毁之前的路径提示小球
            foreach (var o in _pathObjList)
            {
                Destroy(o);
            }
            _pathObjList.Clear();
            
            //重新生成小球
            foreach (var p in _drawCornerList)
            {
                _pathObjList.Add(Instantiate(_pathObject,  p + Vector3.up * 0.5f, Quaternion.identity));
            }
            
            //设置为不新鲜，不用再次生成
            _navPathIsFresh = false;
        }
    }

    private void ProcessAnimator()
    {
        if (_agent.remainingDistance > _agent.stoppingDistance)
        {
            _animator.SetInteger ("AnimationPar", 1);
        }
        else
        {
            _animator.SetInteger ("AnimationPar", 0);
        }
    }
    
    /// <summary>
    /// 往路径里面多塞一些点
    /// </summary>
    private void ProcessNavPath()
    {
        _drawCornerList.Clear();
        var corners = new List<Vector3>(_navMeshPath.corners);
        corners.Append(_currentDest);
        
        for (int i = 0; i < corners.Count - 1; i++)
        {
            for (int j = 0; j < Vector3.Distance(corners[i], corners[i + 1]); j++)
                _drawCornerList.Add(Vector3.Lerp(corners[i], corners[i + 1],
                    j / Vector3.Distance(corners[i], corners[i + 1])));
        }
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
            //提前算好一些东西
            Vector3 camToPlayer = transform.position - _camera.transform.position;
            camToPlayer.z = 0;
            Vector3 mouseMoveVector = Input.mousePosition - _mouseDragPreviousMousePosition;
            Vector3 myPosition = transform.position;
            Transform cameraTransform = _camera.transform;
            
            //计算两个轴的旋转量
            _camera.transform.RotateAround(myPosition, Vector3.up, mouseMoveVector.x * _viewRotateSpeed.x / 100);
            _camera.transform.RotateAround(myPosition, Vector3.Cross(camToPlayer, Vector3.up), mouseMoveVector.y * _viewRotateSpeed.y / 100);
            _camera.transform.rotation = Quaternion.Euler(cameraTransform.rotation.eulerAngles.x, cameraTransform.eulerAngles.y, 0);
            
            //记录当前鼠标位置
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
                _currentDest = hit.point;
                _currentStart = transform.position;
                NavMesh.CalculatePath(_currentStart, _currentDest, NavMesh.AllAreas, _navMeshPath);
                _agent.SetDestination(_currentDest);
                
                //设置倒计时等待处理路径并显示在屏幕上
                _navProcessCountDown = 0.3f;
            }
        }
    }
}
