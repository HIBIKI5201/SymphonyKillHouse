using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEditor.PackageManager;

public class MapGimmickManager : MonoBehaviour
{
    [Header("レイキャスト設定")]
    [SerializeField,Tooltip("レイキャストを発射するオブジェクトの位置情報")]
    Transform MainPlayerPos;
    [SerializeField,Tooltip("レイキャストの飛距離")]
    float _raycastDistance;
    [Tooltip("レイキャストに当たったオブジェクトの情報")]
    RaycastHit2D _hitInfo;

    [Header("ドア")]
    [SerializeField,Tooltip("ドアが開くまでの時間(秒)")]
    float _doorOpenSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        _hitInfo = Physics2D.Raycast(MainPlayerPos.position, Vector2.right, _raycastDistance);

        if (_hitInfo)
        {
            Debug.Log($"レイキャストに反応中の{_hitInfo.transform.gameObject.name}は{_hitInfo.distance}m");

            if (Input.GetKeyDown(KeyCode.C) && _hitInfo.transform.gameObject.CompareTag("Gimmick"))
            {
                MapGimmikController targetGimmikController = _hitInfo.transform.gameObject.GetComponent<MapGimmikController>();
                Debug.Log("アクション実行");

                if (targetGimmikController._GimmickKind == MapGimmikController.GimmickKind.Door && !targetGimmikController._onActive)
                {
                    Debug.Log("Door実行");
                    targetGimmikController.RotateDoor();

                }
            }
        } else
        {
            Debug.Log("レイキャストに反応なし");
        }
    }
}
