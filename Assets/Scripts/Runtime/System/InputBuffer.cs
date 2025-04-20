using System;
using UnityEngine;
using UnityEngine.InputSystem;
using SymphonyFrameWork.System;

namespace KillHouse.Runtime.System
{
    /// <summary>
    /// 入力を受け取るシステム
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class InputBuffer : MonoBehaviour
    {
        private PlayerInput _playerInput;
        
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }
    }
}