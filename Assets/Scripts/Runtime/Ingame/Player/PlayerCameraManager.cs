using System;
using KillHouse.Runtime.System;
using SymphonyFrameWork.System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KillHouse.Runtime.Ingame
{
    public class PlayerCameraManager : MonoBehaviour, IDisposable
    {
        [SerializeField] private float _lookSpeedY = 5;
        [SerializeField] [Range(-80, 0)] private float _minAngle = -30f;
        [SerializeField] [Range(0, 80)] private float _maxAngle = 80f;
        private GameObject _actor;
        private CinemachineCamera _camera;
        private InputBuffer _inputBuffer;

        private void Awake()
        {
            _camera = GetComponent<CinemachineCamera>();
        }

        private void Start()
        {
            var player = FindAnyObjectByType<PlayerManager>();
            if (player)
            {
                //プレイヤーに追従するアクターを用意し、これを追従する
                _actor = new GameObject("Camera Actor");
                _actor.transform.SetParent(player.transform);
                _actor.transform.localPosition = Vector3.zero;
                _actor.transform.localRotation = Quaternion.identity;

                _camera.Follow = _actor.transform;
            }
            else
            {
                Debug.LogWarning("Player manager not found");
            }

            _inputBuffer = ServiceLocator.GetInstance<InputBuffer>();

            _inputBuffer.Look.started += OnLook;
            _inputBuffer.Look.performed += OnLook;
            _inputBuffer.Look.canceled += OnLook;
        }

        public void Dispose()
        {
            _inputBuffer.Look.started -= OnLook;
            _inputBuffer.Look.performed -= OnLook;
            _inputBuffer.Look.canceled -= OnLook;
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            var lookInput = context.ReadValue<Vector2>();

            //角度範囲内なら上下視点移動する
            var angleX = _actor.transform.localEulerAngles.x; // -180〜180 に補正
            if (angleX > 180f) angleX -= 360f;

            if ((lookInput.y > 0 && angleX < _maxAngle) ||
                (lookInput.y < 0 && _minAngle < angleX))
            {
                _actor.transform.Rotate(Vector3.right, lookInput.y * _lookSpeedY * Time.deltaTime);

                //超過した時に最小、最大の範囲にClampする
                angleX = _actor.transform.localEulerAngles.x;
                if (angleX > 180f) angleX -= 360f;

                if (angleX < _minAngle || _maxAngle < angleX)
                {
                    angleX = Mathf.Clamp(angleX, _minAngle, _maxAngle);

                    //数値を0~360に戻す
                    angleX %= 360f;
                    if (angleX < 0f) angleX += 360f;
                    
                    _actor.transform.localEulerAngles = new Vector3(angleX, 0f, 0f);
                }
            }
        }
    }
}