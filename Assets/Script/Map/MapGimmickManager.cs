using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEditor.PackageManager;

public class MapGimmickManager : MonoBehaviour
{
    [Header("���C�L���X�g�ݒ�")]
    [SerializeField,Tooltip("���C�L���X�g�𔭎˂���I�u�W�F�N�g�̈ʒu���")]
    Transform MainPlayerPos;
    [SerializeField,Tooltip("���C�L���X�g�̔򋗗�")]
    float _raycastDistance;
    [Tooltip("���C�L���X�g�ɓ��������I�u�W�F�N�g�̏��")]
    RaycastHit2D _hitInfo;

    [Header("�h�A")]
    [SerializeField,Tooltip("�h�A���J���܂ł̎���(�b)")]
    float _doorOpenSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        _hitInfo = Physics2D.Raycast(MainPlayerPos.position, Vector2.right, _raycastDistance);

        if (_hitInfo)
        {
            Debug.Log($"���C�L���X�g�ɔ�������{_hitInfo.transform.gameObject.name}��{_hitInfo.distance}m");

            if (Input.GetKeyDown(KeyCode.C) && _hitInfo.transform.gameObject.CompareTag("Gimmick"))
            {
                MapGimmikController targetGimmikController = _hitInfo.transform.gameObject.GetComponent<MapGimmikController>();
                Debug.Log("�A�N�V�������s");

                if (targetGimmikController._GimmickKind == MapGimmikController.GimmickKind.Door && !targetGimmikController._onActive)
                {
                    Debug.Log("Door���s");
                    targetGimmikController.RotateDoor();

                }
            }
        } else
        {
            Debug.Log("���C�L���X�g�ɔ����Ȃ�");
        }
    }
}
