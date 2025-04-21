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

        public class InputActionContext : IDisposable
        {
            private InputAction _action;
            public InputAction Action => _action;
            public InputActionContext(InputAction action)
            {
                _action = action;
                _action.started += ActionInvoke;
                _action.performed += ActionInvoke;
                _action.canceled += ActionInvoke;
            }

            public void Dispose()
            {
                _action.started -= ActionInvoke;
                _action.performed -= ActionInvoke;
                _action.canceled -= ActionInvoke;
            }

            private void ActionInvoke(InputAction.CallbackContext context) => Invoked?.Invoke(context);

            public Action<InputAction.CallbackContext> Invoked;
        }
        
        public InputActionContext Move => _move;
        private InputActionContext _move;

        public InputActionContext Attack => _attack;
        private InputActionContext _attack;
        
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _move = new InputActionContext( _playerInput.actions["Move"]);
            _attack = new InputActionContext( _playerInput.actions["Attack"]);
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