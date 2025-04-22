using System;
using System.Threading;
using KillHouse.Runtime.System;
using SymphonyFrameWork.System;
using SymphonyFrameWork.Utility;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace KillHouse.Runtime.Ingame
{
    public class PlayerManager : MonoBehaviour, IDisposable
    {
        private static readonly int AnimMoveX = Animator.StringToHash("MoveX");
        private static readonly int AnimMoveY = Animator.StringToHash("MoveY");
        private static readonly int AnimMoveMag = Animator.StringToHash("MoveMag");
        private static readonly int AnimSprint = Animator.StringToHash("Sprint");

        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _dushSpeed = 8f;
        [SerializeField] private float _lookSpeed = 3f;

        private InputBuffer _inputBuffer;
        private Animator _animator;

        private bool _isMove;

        private bool _isSprint;
        private Vector2 _moveInput = Vector2.zero;
        private CancellationTokenSource _moveTaskToken;

        private void Start()
        {
            # region 入力系

            _inputBuffer = ServiceLocator.GetInstance<InputBuffer>();

            //攻撃入力
            _inputBuffer.Attack.started += _ => Debug.Log("Attack");

            //移動入力
            _inputBuffer.Move.started += OnMove;
            _inputBuffer.Move.performed += OnMove;
            _inputBuffer.Move.canceled += OnMove;

            _inputBuffer.Look.started += OnLook;
            _inputBuffer.Look.performed += OnLook;
            _inputBuffer.Look.canceled += OnLook;

            _inputBuffer.Sprint.started += OnSprint;
            _inputBuffer.Sprint.canceled += OnSprint;

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
            {
                var speed = _isSprint && 0.5f < _moveInput.magnitude && 0 < _moveInput.y ?
                    _dushSpeed : _moveSpeed;
                
                var nextMovePos = transform.position
                                  + transform.TransformDirection(new Vector3(_moveInput.x, 0, _moveInput.y)) //プレイヤーの正面方向に合わせる
                                  * (speed * Time.deltaTime);
                
                if (NavMesh.SamplePosition(nextMovePos, out var hit,
                        1.0f, NavMesh.AllAreas))
                {
                    transform.position = hit.position;
                }
            }
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
                        _animator.SetFloat(AnimMoveX, vec.x);
                        _animator.SetFloat(AnimMoveY, vec.y);
                        _animator.SetFloat(AnimMoveMag, vec.magnitude);
                    },
                    _moveInput, 0.3f,
                    token: _moveTaskToken.Token);
            }
        }

        /// <summary>
        ///     視点入力を受け取った時
        /// </summary>
        /// <param name="context"></param>
        private void OnLook(InputAction.CallbackContext context)
        {
            var lookInput = context.ReadValue<Vector2>();

            //キャラを回転させる
            transform.Rotate(Vector3.up, lookInput.x * _lookSpeed * Time.deltaTime);
        }

        /// <summary>
        ///     ダッシュ入力を受け取った時
        /// </summary>
        /// <param name="context"></param>
        private void OnSprint(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    _isSprint = true;
                    break;

                case InputActionPhase.Canceled:
                    _isSprint = false;
                    break;
            }

            _animator.SetBool(AnimSprint, _isSprint);
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            _inputBuffer.Move.started -= OnMove;
            _inputBuffer.Move.performed -= OnMove;
            _inputBuffer.Move.canceled -= OnMove;

            _inputBuffer.Look.started -= OnLook;
            _inputBuffer.Look.performed -= OnLook;
            _inputBuffer.Look.canceled -= OnLook;

            _inputBuffer.Sprint.started -= OnSprint;
            _inputBuffer.Sprint.canceled -= OnSprint;
            
            Debug.Log($"{name}を解放しました。");
        }
    }
}