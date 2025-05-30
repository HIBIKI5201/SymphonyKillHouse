using System;
using System.Threading;
using KillHouse.Runtime.System;
using SymphonyFrameWork.System;
using SymphonyFrameWork.Utility;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace KillHouse.Runtime.Ingame
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerManager : MonoBehaviour, IDisposable
    {
        private static readonly int AnimMoveX = Animator.StringToHash("MoveX");
        private static readonly int AnimMoveY = Animator.StringToHash("MoveY");
        private static readonly int AnimSprint = Animator.StringToHash("Sprint");
        private static readonly int AnimJump = Animator.StringToHash("Jump");
        private static readonly int AnimOnGround = Animator.StringToHash("OnGround");

        [SerializeField] private float _moveAcceleration = 3;
        [FormerlySerializedAs("_dushAcceleration")] [SerializeField] private float _dashAcceleration = 3;
        [SerializeField] private float _moveMaxSpeed = 5f;
        [SerializeField] private float _dushMaxSpeed = 8f;
        [SerializeField] private float _jumpPower = 8f;

        [Space] [SerializeField] private float _lookSpeed = 3f;

        private Animator _animator;
        private Rigidbody _rigidbody;

        private InputBuffer _inputBuffer;

        private bool _isMove;

        private bool _isSprint;
        private Vector2 _moveInput = Vector2.zero;
        private CancellationTokenSource _moveTaskToken;

        private bool _jump;

        private bool _onGround;
        private byte _collisionGroundCount;
        

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            
            _onGround = true;
        }

        private void Start()
        {
            # region 入力系

            _inputBuffer = ServiceLocator.GetInstance<InputBuffer>();

            //移動入力
            _inputBuffer.Move.started += OnMove;
            _inputBuffer.Move.performed += OnMove;
            _inputBuffer.Move.canceled += OnMove;

            //攻撃入力
            _inputBuffer.Attack.started += OnAttack;

            _inputBuffer.Look.started += OnLook;
            _inputBuffer.Look.performed += OnLook;
            _inputBuffer.Look.canceled += OnLook;

            _inputBuffer.Jump.started += OnJump;

            _inputBuffer.Sprint.started += OnSprint;
            _inputBuffer.Sprint.canceled += OnSprint;

            #endregion

            _animator = GetComponentInChildren<Animator>();
        }

        private void FixedUpdate()
        {
            MoveFixedUpdate();
            JumpFixedUpdate();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(TagsEnum.Ground.ToString()))
            {
                _collisionGroundCount++;
                if (0 < _collisionGroundCount)
                {
                    _onGround = true;
                    _animator.SetBool(AnimOnGround, true);
                }
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag(TagsEnum.Ground.ToString()))
            {
                _collisionGroundCount--;
                if (_collisionGroundCount <= 0)
                {
                    _onGround = false;
                    _animator.SetBool(AnimOnGround, false);
                }
            }
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

        private void MoveFixedUpdate()
        {
            if (!_isMove) return;

            var isDash = _isSprint && 0.7071f < _moveInput.y && _onGround;
            
            //水平方向に加速を与える
            var acceleration = isDash ? _dashAcceleration : _moveAcceleration;
            var force = transform.TransformDirection(
                new Vector3(_moveInput.x, 0, _moveInput.y)) * acceleration;
            _rigidbody.AddForce(force, ForceMode.Force);
            
            //水平の速度がMaxSpeedを超えた場合に制限する
            var maxVelocity = isDash ? _dushMaxSpeed : _moveMaxSpeed;
            var velocityXZ = new Vector2(_rigidbody.linearVelocity.x, _rigidbody.linearVelocity.z);
            
            if (velocityXZ.sqrMagnitude > maxVelocity * maxVelocity)
            {
                velocityXZ = velocityXZ.normalized * maxVelocity;
                _rigidbody.linearVelocity = new Vector3(velocityXZ.x, _rigidbody.linearVelocity.y, velocityXZ.y);
            }
        }

        /// <summary>
        ///     移動入力を受け取った時
        /// </summary>
        /// <param name="context"></param>
        private void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();

            //isMoveを変更
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

                //現在のパラメータを取得
                var lastInput = new Vector2(
                    _animator.GetFloat(AnimMoveX),
                    _animator.GetFloat(AnimMoveY));
                
                //徐々にパラメータを目標値まで上げる
                SymphonyTween.PausableTweening(lastInput,
                    vec =>
                    {
                        _animator.SetFloat(AnimMoveX, vec.x);
                        _animator.SetFloat(AnimMoveY, vec.y);
                    },
                    _moveInput, 0.3f,
                    token: _moveTaskToken.Token);
            }
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            Debug.Log("Attack");
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
        ///     ジャンプ入力を受け取った時
        /// </summary>
        /// <param name="context"></param>
        private async void OnJump(InputAction.CallbackContext context)
        {
            if (!_onGround) return;
            
            _jump = true;
        }

        private void JumpFixedUpdate()
        {
            if (!_jump) return;
            _jump = false;

            _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, 0, _rigidbody.linearVelocity.z);
            _rigidbody.AddForce(transform.up * _jumpPower, ForceMode.Impulse);
            
            _animator.SetTrigger(AnimJump);
        }

        /// <summary>
        ///     ダッシュ入力を受け取った時
        /// </summary>
        /// <param name="context"></param>
        private void OnSprint(InputAction.CallbackContext context)
        {
            //isSprintを変更
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

        private void OnGUI()
        {
            float velocityXZ = new Vector2(_rigidbody.linearVelocity.x, _rigidbody.linearVelocity.z).magnitude;
             GUI.Label(new Rect(0,0,100,300), velocityXZ.ToString());
        }
    }
}