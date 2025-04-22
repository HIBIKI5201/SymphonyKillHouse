using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KillHouse.Runtime.System
{
    /// <summary>
    /// 入力を受け取るシステム
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class InputBuffer : MonoBehaviour
    {
        private PlayerInput _playerInput;
        
        public InputAction Move => _move;
        private InputAction _move;

        public InputAction Attack => _attack;
        private InputAction _attack;

        public InputAction Look => _look;
        private InputAction _look;
        
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _move =  _playerInput.actions["Move"];
            _attack =  _playerInput.actions["Attack"];
            _look =  _playerInput.actions["Look"];
            
            Cursor.lockState = CursorLockMode.Locked;
        }

        /// <summary>
        ///     アクションマップを変更する
        /// </summary>
        /// <param name="kind"></param>
        public void ChangeActionMap(ActionMapEnum kind)
        {
            if (kind == ActionMapEnum.None)
            {
                Debug.LogWarning("No action map defined");
                return;
            }
            
            _playerInput.SwitchCurrentActionMap(kind.ToString());
        }

        public enum ActionMapEnum
        {
            None = 0,
            Player = 1,
            UI = 2,
        }
    }
}