using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMoveManager : MonoBehaviour
{
    [Header("コンポーネント")]
    [SerializeField] Transform GunPosition;
    [SerializeField] Transform ShoulderPosition;
    [Space]
    [SerializeField] Transform LeftHand;

    [Header("ポジション調整")]
    [SerializeField] float maxAngle;
    [SerializeField] float minAngle;

    public float mouseAngle;

    float GunDistance;

    void Start()
    {
        GunDistance = Vector2.Distance(GunPosition.position, ShoulderPosition.position);
    }


    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseAngle = Mathf.Clamp(Mathf.Atan2(mousePosition.y - ShoulderPosition.position.y, mousePosition.x - ShoulderPosition.position.x) * Mathf.Rad2Deg, minAngle, maxAngle);
        float mouseAngleRadians = mouseAngle * Mathf.Deg2Rad;

        GunPosition.position = ShoulderPosition.position + new Vector3(Mathf.Cos(mouseAngleRadians) * GunDistance, Mathf.Sin(mouseAngleRadians) * GunDistance, 0f);
        GunPosition.eulerAngles = new Vector3(0f, 0f, mouseAngle);

        LeftHand.eulerAngles = new Vector3(0f, 0f, mouseAngle);
    }
}
