using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGimmikController : MonoBehaviour
{
    [SerializeField] Transform GimmickObjectTransform;
    public bool _onActive = false;

    public GimmickKind _GimmickKind;
    public enum GimmickKind
    {
        Door
    }

    [Header("ドア設定")]
    [SerializeField,Tooltip("trueの時に時計回り")]
    bool _rotateDirection;

    void Start()
    {
        
    }

    public void RotateDoor(float doorOpenSpeed)
    {
        _onActive = true;

        Collider2D selfCollider = GetComponent<Collider2D>();
        if (!selfCollider.isTrigger)
        {
            Debug.Log("ドアをオープン");
            
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
            Debug.Log("ドアをクローズ");

            Tween MoveDoor = GimmickObjectTransform.DORotate(Vector3.up * 0, doorOpenSpeed)
                .OnPlay(() => selfCollider.isTrigger = false)
                .OnComplete(() => _onActive = false);
        }
    }
}
