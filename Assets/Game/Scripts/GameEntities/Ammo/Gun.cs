using Assets.Game.Scripts.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    private Transform _firePoint;
    private Player _player;
    private ObjectsPool<Bullet> _buletsPool;

    [Tooltip("Число выстрелов в секунду")]
    public float rateOfFile = 1;

    [Tooltip("Префаб пули")]
    public Bullet bulletPrefub;

    [Tooltip("Точка спавна пуль")]
    public GameObject firePoint;
   

    // Start is called before the first frame update
    void Start()
    {
        _firePoint = firePoint.transform;
        _player = gameObject.GetComponentInParent<Player>();

        _buletsPool = new ObjectsPool<Bullet>(bulletPrefub, 20);

        InvokeRepeating(nameof(Fire), 0, 1/rateOfFile);
    }

    void Fire()
    {
        var bullet = _buletsPool.InstantiateToScene(_firePoint.position, _firePoint.rotation);

        bullet.GetComponent<Rigidbody2D>().velocity = transform.up * bullet.bulletSpeed;

        _player.Shoot();
    }

    
}
