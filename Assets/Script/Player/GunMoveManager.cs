using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMoveManager : MonoBehaviour
{
    [Header("�v���C���[���")]
    [SerializeField]
    GameObject Player;
    [SerializeField]
    PlayerController Controller;

    [Header("�R���|�[�l���g")]
    [SerializeField]
    Transform GunPosition;
    [SerializeField]
    Transform ShoulderPosition;

    [Space]
    [SerializeField]
    Transform LeftHand;

    [Header("�|�W�V��������")]

    [SerializeField, Tooltip("���̈ʒu�̔�����")]
    Vector3 _shoulderOffset;
    [SerializeField,Tooltip("�A���O���̔�����")]
    float _angleOffset;

    [Space]
    [SerializeField,Tooltip("������ő�ƍŏ��̊p�x")]
    float _AngleLimit;

    [HideInInspector,Tooltip("���𒆐S�Ƃ����}�E�X�̕���(�x���@)")]
    public float _mouseAngle;
    [Tooltip("������e�̒��S�܂ł̋���(��]�̔��a)")]
    float _gunDistance;

    [Header("�ׂ�������")]

    [SerializeField,Tooltip("��b�Ԃɏe�̈ړ������s������")]
    float _gunHoldLimit;
    [Tooltip("GunMove�����s�����C���^�[�o���̃^�C�}�[")]
    float frameCounter;

    [Tooltip("�v���C���[�̃X�P�[����ۑ����Ă����ϐ�")]
    float _playerScale;
    [Tooltip("�e�̃X�P�[����ۑ����Ă����ϐ�")]
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
