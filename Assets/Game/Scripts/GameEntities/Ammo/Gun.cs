using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    private Transform _firePoint;
    private Player _player;

    [Tooltip("����� ��������� � �������")]
    public float rateOfFile = 1;

    [Tooltip("������ ����")]
    public GameObject bulletPrefub;

    [Tooltip("����� ������ ����")]
    public GameObject firePoint;
   

    // Start is called before the first frame update
    void Start()
    {
        _firePoint = firePoint.transform;
        _player = gameObject.GetComponentInParent<Player>();

        InvokeRepeating(nameof(Fire), 0, 1/rateOfFile);
    }

    void Fire()
    {
        GameObject newBullet = Instantiate(bulletPrefub, _firePoint.position, _firePoint.rotation);
        _player.Shoot();
    }

    
}
