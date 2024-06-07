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
            Debug.Log("ギミックを感知");

            if (collision.GetComponent<MapGimmikController>()._GimmickKind == MapGimmikController.GimmickKind.Door)
            {
                Debug.Log("Doorを感知");
            }
        }

        Debug.Log("何かを感知");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
