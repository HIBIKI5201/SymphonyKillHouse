using System;
using KillHouse.Runtime.Ingame;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    private CinemachineCamera _camera;

    private void Awake()
    {
        _camera = GetComponent<CinemachineCamera>();
    }

    void Start()
    {
        var player = FindAnyObjectByType<PlayerManager>();
        if (player)
        {
            _camera.Follow = player.transform;
        }
        else
        {
            Debug.LogWarning("Player manager not found");
        }
    }
}
