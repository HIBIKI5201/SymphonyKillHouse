using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MapGimmikController : MonoBehaviour
{
    [SerializeField] Transform GimmickObjectTransform;
    public bool _onActive = false;

    public GimmickKind _GimmickKind;
    public enum GimmickKind
    {
        Door
    }

    [Header("�h�A�ݒ�")]
    [SerializeField,Tooltip("true�̎��Ɏ��v���")]
    bool _rotateDirection;
    [SerializeField, Tooltip("�e�̐���")]
    ShadowCaster2D DoorShadowCaster;

    void Start()
    {
        
    }

    public void RotateDoor(float doorOpenSpeed)
    {
        _onActive = true;

        Collider2D selfCollider = GetComponent<Collider2D>();
        if (!selfCollider.isTrigger)
        {
            Debug.Log("�h�A���I�[�v��");
            DoorShadowCaster.enabled = false;
            
            if (_rotateDirection)
            {
                GimmickObjectTransform.DORotate(Vector3.up * 90f, doorOpenSpeed)
                    .OnPlay(() => selfCollider.isTrigger = true)
                    .OnComplete(() => _onActive = false);
            } else
            {
                GimmickObjectTransform.DORotate(Vector3.up * -90f, doorOpenSpeed)
                    .OnPlay(() => selfCollider.isTrigger = true)
                    .OnComplete(() => _onActive = false);

            }
        } else
        {
            Debug.Log("�h�A���N���[�Y");
            DoorShadowCaster.enabled = true;

            Tween MoveDoor = GimmickObjectTransform.DORotate(Vector3.up * 0, doorOpenSpeed)
                .OnPlay(() => selfCollider.isTrigger = false)
                .OnComplete(() => _onActive = false);
        }
    }
}
