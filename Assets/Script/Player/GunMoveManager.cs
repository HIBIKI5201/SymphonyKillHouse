using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMoveManager : MonoBehaviour
{
    [Header("プレイヤー情報")]
    [SerializeField]
    GameObject Player;
    [SerializeField]
    PlayerController Controller;

    [Header("コンポーネント")]
    [SerializeField]
    Transform GunPosition;
    [SerializeField]
    Transform ShoulderPosition;

    [Space]
    [SerializeField]
    Transform LeftHand;

    [Header("ポジション調整")]

    [SerializeField, Tooltip("肩の位置の微調整")]
    Vector3 _shoulderOffset;
    [SerializeField,Tooltip("アングルの微調整")]
    float _angleOffset;

    [Space]
    [SerializeField,Tooltip("向ける最大と最小の角度")]
    float _AngleLimit;

    [HideInInspector,Tooltip("肩を中心としたマウスの方向(度数法)")]
    public float _mouseAngle;
    [Tooltip("肩から銃の中心までの距離(回転の半径)")]
    float _gunDistance;

    [Header("細かい調整")]

    [SerializeField,Tooltip("一秒間に銃の移動が実行される回数")]
    float _gunHoldLimit;
    [Tooltip("GunMoveが実行されるインターバルのタイマー")]
    float frameCounter;

    [Tooltip("プレイヤーのスケールを保存しておく変数")]
    float _playerScale;
    [Tooltip("銃のスケールを保存しておく変数")]
    Vector2 _gunScale;


    void Start()
    {
        _gunDistance = Vector2.Distance(GunPosition.position, ShoulderPosition.position);
        _playerScale = Player.transform.lossyScale.x;
        _gunScale = GunPosition.localScale;
    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        frameCounter = (frameCounter + 1) % _gunHoldLimit;
        if (frameCounter == 0)
        {
            GunHold(mousePosition);
        }

        if (Controller._playerMode != PlayerController.PlayerMode.Running)
        {
            flip(mousePosition);
        }
    }

    float AngleMath(float mouseAngle)
    {
        if (Mathf.Abs(mouseAngle) > 180 - _AngleLimit)
        {
            return mouseAngle;
        }
        else if (Mathf.Abs(mouseAngle) > _AngleLimit)
        {
            if (Mathf.Abs(mouseAngle) < 90)
            {
                return mouseAngle > 0 ? _AngleLimit : -_AngleLimit;
            }
            else
            {
                return mouseAngle > 0 ? 180 - _AngleLimit : _AngleLimit - 180;
            }

        }
        else
        {
            return mouseAngle;
        }
    }

    void GunHold(Vector3 mousePosition)
    {
        if ( Controller._playerMode != PlayerController.PlayerMode.Running)
        {
        _mouseAngle = Mathf.Atan2(mousePosition.y - ShoulderPosition.position.y, mousePosition.x - ShoulderPosition.position.x) * Mathf.Rad2Deg;
        _mouseAngle = AngleMath(_mouseAngle);
        } else
        {
            _mouseAngle = transform.localScale.x > 0 ? -20 : -160;
        }
        
        
        float mouseAngleRadians = _mouseAngle * Mathf.Deg2Rad;

        GunPosition.position = ShoulderPosition.position + _shoulderOffset + new Vector3(Mathf.Cos(mouseAngleRadians) * _gunDistance, Mathf.Sin(mouseAngleRadians) * _gunDistance, 0f);

        float angleOffsetDelta = transform.localScale.x > 0 ? _angleOffset : -_angleOffset;
        GunPosition.eulerAngles = new Vector3(0f, 0f, _mouseAngle + angleOffsetDelta);
        LeftHand.eulerAngles = new Vector3(0f, 0f, transform.localScale.x > 0 ? _mouseAngle : (_mouseAngle + 180) + angleOffsetDelta);
    }

    void flip(Vector3 mousePosition)
    {
        if (mousePosition.x - ShoulderPosition.position.x < 0)
        {
            Player.transform.localScale = new Vector3(-_playerScale, Player.transform.localScale.y);
            GunPosition.localScale = new Vector2(-_gunScale.x, -_gunScale.y);
        }
        else
        {
            Player.transform.localScale = new Vector3(_playerScale, Player.transform.localScale.y);
            GunPosition.localScale = new Vector2(_gunScale.x, _gunScale.y);
        }
    }
        
}
