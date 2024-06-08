using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] float _destroyTime;
    void Start()
    {
        Invoke("Destroy", _destroyTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy (this.gameObject);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
