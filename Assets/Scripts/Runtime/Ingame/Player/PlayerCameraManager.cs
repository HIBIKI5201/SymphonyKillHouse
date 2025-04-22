using Unity.Cinemachine;
using UnityEngine;

namespace KillHouse.Runtime.Ingame
{
    public class PlayerCameraManager : MonoBehaviour
    {
        private CinemachineCamera _camera;

        private void Awake()
        {
            _camera = GetComponent<CinemachineCamera>();
        }

        private void Start()
        {
            var player = FindAnyObjectByType<PlayerManager>();
            if (player)
                _camera.Follow = player.transform;
            else
                Debug.LogWarning("Player manager not found");
        }
    }
}