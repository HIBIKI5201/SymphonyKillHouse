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

    void Start()
    {
        
    }

    public void RotateDoor()
    {
        _onActive = true;

        Collider2D selfCollider = GetComponent<Collider2D>();
        if (!selfCollider.isTrigger)
        {
            Debug.Log("ドアをオープン");
            
            GimmickObjectTransform.DORotate(Vector3.up * -90f, 2f)
                .OnPlay(() => selfCollider.isTrigger = true)
                .OnComplete(() => _onActive = false);
        } else
        {
            Debug.Log("ドアをクローズ");

            Tween MoveDoor = GimmickObjectTransform.DORotate(Vector3.up * 0, 2f)
                .OnPlay(() => selfCollider.isTrigger = false)
                .OnComplete(() => _onActive = false);
        }
    }
}
