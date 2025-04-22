using System.Threading;
using KillHouse.Runtime.System;
using SymphonyFrameWork.System;
using SymphonyFrameWork.Utility;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace KillHouse.Runtime.Ingame
{
    public class PlayerManager : MonoBehaviour
    {
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveY = Animator.StringToHash("MoveY");
        
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _lookSpeed = 3f;

        private Animator _animator;

        private bool _isMove;
        private Vector2 _moveInput = Vector2.zero;
        private CancellationTokenSource _moveTaskToken;

        private void Start()
        {
            # region 入力系

            var inputBuffer = ServiceLocator.GetInstance<InputBuffer>();

            //攻撃入力
            inputBuffer.Attack.started += _ => Debug.Log("Attack");

            //移動入力
            inputBuffer.Move.started += OnMove;
            inputBuffer.Move.performed += OnMove;
            inputBuffer.Move.canceled += OnMove;
            
            inputBuffer.Look.started += OnLook;
            inputBuffer.Look.performed += OnLook;
            inputBuffer.Look.canceled += OnLook;

            #endregion

            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            if (_isMove)
                //NavMeshから移動場所を選定
                if (NavMesh.SamplePosition(transform.position
                                           + transform.TransformDirection(new Vector3(_moveInput.x, 0, _moveInput.y)) //プレイヤーの正面方向に合わせる
                                           * (_moveSpeed * Time.deltaTime),
                        out var hit, 1.0f, NavMesh.AllAreas))
                    transform.position = hit.position;
        }

        /// <summary>
        ///     移動入力を受け取った時
        /// </summary>
        /// <param name="context"></param>
        private void OnMove(InputAction.CallbackContext context)
        {
            var lastInput = _moveInput;
            _moveInput = context.ReadValue<Vector2>();

            switch (context.phase)
            {
                case InputActionPhase.Started:
                    _isMove = true;
                    return;

                case InputActionPhase.Canceled:
                    _isMove = false;
                    break;
            }

            //アニメーションにパラメータを代入
            if (_animator)
            {
                _moveTaskToken?.Cancel(); //前のTweeningをキャンセル
                _moveTaskToken = new CancellationTokenSource();

                //徐々にパラメータを目標値まで上げる
                SymphonyTween.PausableTweening(lastInput,
                    vec =>
                    {
                        _animator.SetFloat(MoveX, vec.x);
                        _animator.SetFloat(MoveY, vec.y);
                    },
                    _moveInput, 0.3f,
                    token: _moveTaskToken.Token);
            }
        }

        /// <summary>
        /// 視点入力を受け取った時
        /// </summary>
        /// <param name="context"></param>
        private void OnLook(InputAction.CallbackContext context)
        {
            var lookInput = context.ReadValue<Vector2>();
            
            transform.Rotate(Vector3.up, lookInput.x * _lookSpeed * Time.deltaTime);
        }
    }
}