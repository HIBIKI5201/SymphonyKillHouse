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
    [SerializeField] GameObject MazzleFlash;

    void Start()
    {

    }

    public IEnumerator Shoot()
    {
        GameObject bullet = Instantiate(Bullet, MuzzlePos.position, MuzzlePos.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * _bulletSpeed;

        MazzleFlash.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        MazzleFlash.gameObject.SetActive(false);
    }



    void Update()
    {
        
    }
}
