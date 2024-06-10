using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerController : MonoBehaviour
{
    [Header("�v���C���[")]
    [SerializeField]
    GameObject Player;

    [Header("�v���C���[�R���|�[�l���g")]
    [SerializeField]
    Rigidbody2D playerRB;
    [SerializeField]
    Animator playerAnimator;

    [Header("�X�e�[�^�X")]
    [SerializeField,Tooltip("�ړ����x")]
    float _moveSpeed;
    [SerializeField,Tooltip("�_�b�V�����̈ړ����x�{��")]
    float _runSpeed;
    [Tooltip("���͂���Ă��鐅���̕���")]
    float horizontal;

    [Tooltip("�v���C���[�̏�Ԃ�\��")]
    public PlayerMode _playerMode;
    public enum PlayerMode
    {
        Wait,
        Walk,
        Running,
        Crouching
    }

    [Header("����R���|�[�l���g")]
    [SerializeField]
    GunShootManager gunShootManager;

    void Start()
    {
        _playerMode = PlayerMode.Wait;
    }

    void Update()
    {

        horizontal = Input.GetAxisRaw("Horizontal");

        ChangePlayerMode();

        Move();
        Animation();
        Action();
    }

    void ChangePlayerMode()
    {
        if (horizontal != 0 && _playerMode != PlayerMode.Crouching)
        {
            if (Mathf.Sign(horizontal) == Mathf.Sign(transform.localScale.x) && Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (_playerMode != PlayerMode.Running)
                {
                    _playerMode = PlayerMode.Running;
                }
                else
                {
                    _playerMode = PlayerMode.Walk;
                }
            }

            if (_playerMode != PlayerMode.Running)
            {
                _playerMode = PlayerMode.Walk;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _playerMode = PlayerMode.Crouching;
        } else
        {
            _playerMode = PlayerMode.Wait;
        }
    }

    void Move()
    {
        if (horizontal != 0)
        {
            if (_playerMode == PlayerMode.Walk)
            {
                playerRB.velocity = new Vector2(_moveSpeed * horizontal, playerRB.velocity.y);
            }
            else if (_playerMode == PlayerMode.Running)
            {
                playerRB.velocity = new Vector2(_moveSpeed * horizontal * _runSpeed, playerRB.velocity.y);
            }
        }
        else
        {
            playerRB.velocity = new Vector2(0, playerRB.velocity.y);
        }
    }

    void Animation()
    {
        if (_playerMode == PlayerMode.Walk)
        {
            if (Mathf.Sign(horizontal) == Mathf.Sign(transform.localScale.x))
            {
                playerAnimator.SetBool("Move", true);

                playerAnimator.SetBool("MoveBack", false);
            }
            else
            {
                playerAnimator.SetBool("MoveBack", true);

                playerAnimator.SetBool("Move", false);
            }
            playerAnimator.SetBool("Run", false);
        }
        else if (_playerMode == PlayerMode.Running)
        {
            playerAnimator.SetBool("Run", true);
        }
        else if (_playerMode == PlayerMode.Crouching)
        {
            playerAnimator.SetBool("Crouching", true);

            playerAnimator.SetBool("Move", false);
            playerAnimator.SetBool("MoveBack", false);
            playerAnimator.SetBool("Run", false);
            }
        else if (_playerMode == PlayerMode.Wait)
        {
            playerAnimator.SetBool("Move", false);
            playerAnimator.SetBool("MoveBack", false);
            playerAnimator.SetBool("Run", false);
            playerAnimator.SetBool("Crouching", false);
        }
    }

    void Action()
    {



        if (Input.GetMouseButtonDown(0) && _playerMode != PlayerMode.Running)
        {
            if (gunShootManager._remainBullets > 0)
            {
                StartCoroutine(gunShootManager.Shoot());
            }
            else
            {
                Debug.Log("�����[�h");
                gunShootManager._remainBullets = 30;
            }
        }
    }
}
