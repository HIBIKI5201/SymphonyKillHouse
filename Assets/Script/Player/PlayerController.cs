using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    SymphonyInputSystem _inputSystem;
    Rigidbody2D _playerRB;
    Animator _playerAnimator;

    [Header("ステータス")]
    [SerializeField, Tooltip("移動速度")]
    float _moveSpeed;
    [SerializeField, Tooltip("ダッシュ時の移動速度倍率")]
    float _runSpeed;
    bool _stopTrigger;
    [Tooltip("移動方向")]
    Vector2 _inputAxis;

    [Tooltip("プレイヤーの状態を表す")]
    public PlayerMode _playerMode;
    public enum PlayerMode
    {
        Wait,
        Walk,
        Running,
        Crouching
    }

    [Header("操作コンポーネント")]
    [SerializeField]
    GunShootManager gunShootManager;

    float targetMoveBlend;
    float currentMoveBlend;

    void Start()
    {
        _playerRB = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();

        _playerMode = PlayerMode.Wait;
    }

    void Update()
    {
        Move();
        Animation();
    }

    private void Move()
    {
        if (_playerMode == PlayerMode.Walk)
        {
            _playerRB.velocity = new Vector2(_moveSpeed * _inputAxis.x, _playerRB.velocity.y);
        }
        else if (_playerMode == PlayerMode.Running)
        {
            _playerRB.velocity = new Vector2(_moveSpeed * _inputAxis.x * _runSpeed, _playerRB.velocity.y);
        }

        if (_stopTrigger)
        {

            _stopTrigger = false;
        }
    }

    void Animation()
    {
        targetMoveBlend = _playerRB.velocity.x * Mathf.Sign(transform.localScale.x);
        currentMoveBlend = targetMoveBlend > currentMoveBlend ? Mathf.Lerp(currentMoveBlend, targetMoveBlend, Time.deltaTime * 3f) : Mathf.Lerp(currentMoveBlend, targetMoveBlend, Time.deltaTime * 6f);

        _playerAnimator.SetFloat("MoveBlend", currentMoveBlend);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _inputAxis = context.ReadValue<Vector2>();

        if (_inputAxis == Vector2.zero)
        {
            _playerMode = PlayerMode.Wait;
        }
        else
        {
            _playerMode = PlayerMode.Walk;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && _playerMode == PlayerMode.Walk && _inputAxis.x == Mathf.Sign(transform.localScale.x))
        {
            Debug.Log("おん");
            _playerMode = PlayerMode.Running;
        }
        else if (context.phase == InputActionPhase.Canceled && _playerMode == PlayerMode.Running)
        {
            Debug.Log("おふ");
            _playerMode = PlayerMode.Walk;
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (_playerMode != PlayerMode.Running && context.phase == InputActionPhase.Started)
        {
            if (gunShootManager._remainBullets > 0)
            {
                StartCoroutine(gunShootManager.Shoot());
            }
            else
            {
                Debug.Log("リロード");
                gunShootManager._remainBullets = 30;
            }
        }
    }
}
