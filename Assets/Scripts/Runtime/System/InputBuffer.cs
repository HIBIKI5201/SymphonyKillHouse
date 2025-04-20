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
        public PlayerInput PlayerInput => _playerInput;
        
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
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