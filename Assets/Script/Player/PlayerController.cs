using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤー")]
    [SerializeField] GameObject Player;

    [Header("カメラ")]
    [SerializeField] GameObject Camera;
    [SerializeField] Vector3 _cameraPosition;

    [Header("プレイヤーコンポーネント")]
    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] Animator playerAnimator;

    [Header("ステータス")]
    [SerializeField] float _moveSpeed;
    [SerializeField] float _runSpeed;

    public PlayerMode _playerMode;
    public enum PlayerMode
    {
        Wait,
        Walk,
        Running,
        Crouching
    }

    [Header("操作コンポーネント")]
    [SerializeField] GunShootManager gunShootManager;

    void Start()
    {
        _playerMode = PlayerMode.Wait;
    }


    void Update()
    {
        Camera.transform.position = Player.transform.position + _cameraPosition;

        float horizontal = Input.GetAxisRaw("Horizontal");
        if (_playerMode == PlayerMode.Walk || _playerMode == PlayerMode.Running)
        {
            if (_playerMode == PlayerMode.Walk)
            {
                playerRB.velocity = new Vector2(_moveSpeed * horizontal, playerRB.velocity.y);
            }
            if (_playerMode == PlayerMode.Running)
            {
                playerRB.velocity = new Vector2(_moveSpeed * horizontal * _runSpeed, playerRB.velocity.y);
            }


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

            if (_playerMode == PlayerMode.Running)
            {
                playerAnimator.SetBool("Run", true);
            }
            else
            {
                playerAnimator.SetBool("Run", false);
            }
        }
        else
        {
            playerRB.velocity = new Vector2(0, playerRB.velocity.y);

            playerAnimator.SetBool("Move", false);
            playerAnimator.SetBool("MoveBack", false);
            playerAnimator.SetBool("Run", false);
        }
        
        if (_playerMode == PlayerMode.Crouching)
        {
            playerAnimator.SetBool("Crouching", true);
        } else
        {
            playerAnimator.SetBool("Crouching", false);
        }


        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) && _playerMode != PlayerMode.Crouching)
        {
            if (Mathf.Sign(horizontal) == Mathf.Sign(transform.localScale.x) && Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (_playerMode != PlayerMode.Running)
                {
                    _playerMode = PlayerMode.Running;
                } else
                {
                    _playerMode = PlayerMode.Walk;
                }
            }

            if (_playerMode != PlayerMode.Running)
            {
                _playerMode = PlayerMode.Walk;
            }
        } else
        {
            _playerMode = PlayerMode.Wait;
        }

        if (Input.GetKey(KeyCode.S))
        {
            _playerMode = PlayerMode.Crouching;
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(gunShootManager.Shoot());
        }
    }
}
