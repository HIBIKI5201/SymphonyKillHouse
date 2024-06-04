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
        Wait,
        Walk,
        Running,
        Crouching
    }

    void Start()
    {
        _playerMode = PlayerMode.Wait;
    }


    void Update()
    {
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

                if (_playerMode == PlayerMode.Running)
                {
                    playerAnimator.SetBool("Run", true);
                }
            } else
            {
                playerAnimator.SetBool("MoveBack", true);
                playerAnimator.SetBool("Move", false);
            }
        }
        else
        {
            playerRB.velocity = new Vector2(0, playerRB.velocity.y);

            playerAnimator.SetBool("Move", false);
            playerAnimator.SetBool("MoveBack", false);
            playerAnimator.SetBool("Run", false);
        }

        Camera.transform.position = Player.transform.position + _cameraPosition;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) && _playerMode != PlayerMode.Crouching)
        {
            if (Mathf.Sign(horizontal) == Mathf.Sign(transform.localScale.x) && Input.GetKey(KeyCode.LeftShift))
            {
                _playerMode = PlayerMode.Running;

                Debug.Log("StartRun");
            }

            if (_playerMode != PlayerMode.Running)
            {
                _playerMode = PlayerMode.Walk;
                Debug.Log("StartWalk");
            }
        } else
        {
            _playerMode = PlayerMode.Wait;
        }

        if (Input.GetKey(KeyCode.S))
        {
            _playerMode = PlayerMode.Crouching;
        }
    }
}
