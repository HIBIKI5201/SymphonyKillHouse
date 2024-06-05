using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShootManager : MonoBehaviour
{
    [Header("�e�ۃX�e�[�^�X")]
    [SerializeField] GameObject Bullet;
    [Space]
    [SerializeField] float _bulletSpeed;
    [Header("�}�Y���|�W�V����")]
    [SerializeField] Transform MuzzlePos;

    void Start()
    {

    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(Bullet, MuzzlePos.position, MuzzlePos.rotation);

        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * _bulletSpeed;
    }



    void Update()
    {
        
    }
}
