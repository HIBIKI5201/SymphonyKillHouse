using KillHouse.Runtime.System;
using SymphonyFrameWork.System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace KillHouse.Runtime.Ingame
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;

        private Animator _animator;
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveY = Animator.StringToHash("MoveY");

        private bool _isMove;
        private Vector2 _moveInput = Vector2.zero;

        private void Start()
        {
            # region 入力系

            var inputBuffer = ServiceLocator.GetInstance<InputBuffer>();

            //攻撃入力
            inputBuffer.Attack.Action.started += _ => Debug.Log("Attack");

            //移動入力
            inputBuffer.Move.Invoked += OnMove;

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
            {
                //NavMeshから移動場所を選定
                if (NavMesh.SamplePosition(transform.position
                                           + new Vector3(_moveInput.x, 0, _moveInput.y) * (_moveSpeed * Time.deltaTime),
                        out var hit, 1.0f, NavMesh.AllAreas))
                    transform.position = hit.position;
            }
        }

        /// <summary>
        /// 入力を受け取り
        /// </summary>
        /// <param name="context"></param>
        private void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();

            switch (context.phase)
            {
                case InputActionPhase.Started:
                    _isMove = true;
                    break;

                case InputActionPhase.Canceled:
                    _isMove = false;
                    break;
            }

            //アニメーションにパラメータを代入
            if (_animator)
            {
                _animator.SetFloat(MoveX, _moveInput.x);
                _animator.SetFloat(MoveY, _moveInput.y);
            }
        }
    }
}