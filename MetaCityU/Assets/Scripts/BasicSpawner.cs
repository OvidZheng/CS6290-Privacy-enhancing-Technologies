using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkRunner _networkRunner;
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [SerializeField] private GameObject _bownPosition;
    
    private Dictionary<PlayerRef, NetworkObject> _playerList = new Dictionary<PlayerRef, NetworkObject>();
    private Camera _camera;


    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        StartGame(GameMode.AutoHostOrClient);
    }

    async void StartGame(GameMode mode)
    {
        _networkRunner.ProvideInput = true;

        await _networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "Meta CityU Room",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
    
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        NetworkObject networkPlayerObject = _networkRunner.Spawn(_playerPrefab, _bownPosition.transform.position, Quaternion.identity, player);
        _playerList.Add(player, networkPlayerObject);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if(_playerList.TryGetValue(player, out NetworkObject networkPlayerObject))
        {
            _networkRunner.Despawn(networkPlayerObject);
            _playerList.Remove(player);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkInputData inputData = new NetworkInputData();
        //左键
        if (Input.GetMouseButton(0))
        {
            inputData.isPointDown = true;
            Ray mouseDirectionRay = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hitResult = Physics.Raycast(mouseDirectionRay, out hit, Single.PositiveInfinity,
                LayerMask.GetMask("Ground"));
            
            if (hitResult)
            {
                Debug.Log("hit " + hit.point.ToString());
                inputData.moveDestinationPosition = hit.point;
            }
        }
        else
        {
            inputData.isPointDown = false;
        }
        input.Set(inputData);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }
}
