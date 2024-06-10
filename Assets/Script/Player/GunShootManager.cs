using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShootManager : MonoBehaviour
{
    [Header("弾丸ステータス")]
    [SerializeField]
    GameObject Bullet;
    [Space]

    [SerializeField]
    float _bulletSpeed;

    [Header("ライフルステータス")]
    [SerializeField]
    float _magazineSize;
    public float _remainBullets;

    [Header("マズルポジション")]
    [SerializeField]
    Transform MuzzlePos;
    [Header("マズルフラッシュ")]
    [SerializeField]
    GameObject MazzleFlashLight;
    [SerializeField]
    GameObject MazzleFlashSprite;

    void Start()
    {
        _remainBullets = _magazineSize;

        MazzleFlashSprite.gameObject.SetActive(false);
        MazzleFlashLight.gameObject.SetActive(false);
    }

    public IEnumerator Shoot()
    {
        GameObject bullet = Instantiate(Bullet, MuzzlePos.position, MuzzlePos.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * _bulletSpeed;

        _remainBullets--;

        Debug.Log(_remainBullets);

        MazzleFlashLight.gameObject.SetActive(true);
        MazzleFlashSprite.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        MazzleFlashSprite.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        MazzleFlashLight.gameObject.SetActive(false);
    }
}
