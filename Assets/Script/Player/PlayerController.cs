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

    [Header("コンポーネント")]
    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] Animator playerAnimator;

    [Header("ステータス")]
    [SerializeField] float _moveSpeed;
    [SerializeField] float _runSpeed;

    public PlayerMode _playerMode;
    public enum PlayerMode
    {
        Normal,
        Running
    }

    void Start()
    {
        _playerMode = PlayerMode.Normal;
    }


    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal != 0)
        {
            if (_playerMode == PlayerMode.Normal)
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

                if (_playerMode == PlayerMode.Running)
                {
                    playerAnimator.SetBool("Run", true);
                }
            } else
            {
                playerAnimator.SetBool("MoveBack", true);
                playerAnimator.SetBool("Move", false);

                if (_playerMode == PlayerMode.Running)
                {
                    playerAnimator.SetBool("Run", false);
                    _playerMode = PlayerMode.Normal;
                }
            }
        }
        else
        {
            playerRB.velocity = new Vector2(0, playerRB.velocity.y);

            playerAnimator.SetBool("Move", false);
            playerAnimator.SetBool("MoveBack", false);
            playerAnimator.SetBool("Run", false);
            _playerMode = PlayerMode.Normal;
        }

        Camera.transform.position = Player.transform.position + _cameraPosition;

        if (Input.GetKeyDown(KeyCode.LeftShift) && Mathf.Sign(horizontal) == Mathf.Sign(transform.localScale.x))
        {
            _playerMode = PlayerMode.Running;
        }
    }
}
