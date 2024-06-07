using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGimmickManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gimmick"))
        {
            Debug.Log("�M�~�b�N�����m");

            if (collision.GetComponent<MapGimmikController>()._GimmickKind == MapGimmikController.GimmickKind.Door)
            {
                Debug.Log("Door�����m");
            }
        }

        Debug.Log("���������m");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
