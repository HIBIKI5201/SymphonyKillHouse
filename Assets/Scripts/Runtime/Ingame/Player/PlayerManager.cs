using System;
using KillHouse.Runtime.System;
using SymphonyFrameWork.System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KillHouse.Runtime.Ingame
{ 
    public class PlayerManager : MonoBehaviour 
    {
        private Animator _animator;
        private Vector2 _moveInput = Vector2.zero;
        
        private void Start()
        {
            var inputBuffer = ServiceLocator.GetInstance<InputBuffer>();
            var actions = inputBuffer.PlayerInput.actions;
            actions["Attack"].started += ctx => Debug.Log("Attack");
            
            var moveAction = actions["Move"];
            moveAction.started += Move;
            moveAction.performed += Move;
            moveAction.canceled += Move;
            
            _animator = GetComponentInChildren<Animator>();
        }

        private void Move(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
            if (_animator)
            {
                _animator.SetFloat("MoveX", _moveInput.x);
                _animator.SetFloat("MoveY", _moveInput.y);
            }
        }
    } 
}
