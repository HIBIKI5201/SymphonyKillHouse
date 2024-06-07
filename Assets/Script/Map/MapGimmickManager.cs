using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class MapGimmickManager : MonoBehaviour
{
    [Header("�h�A")]
    [SerializeField] float _doorOpenSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gimmick"))
        {
            MapGimmikController mapGimmikController = collision.GetComponent<MapGimmikController>();

            if (mapGimmikController._GimmickKind == MapGimmikController.GimmickKind.Door)
            {
                Debug.Log("Door�����m");

                if (Input.GetKey(KeyCode.Space) && !mapGimmikController._onActive)
                {
                    OpenDoor(collision.transform, mapGimmikController);
                }
            }
        }
    }

    void OpenDoor(Transform DoorTransform, MapGimmikController mapGimmikController)
    {
        Debug.Log("�h�A���I�[�v��");
        mapGimmikController._onActive = true;
        DoorTransform.DORotate(Vector3.up * -90f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
