using Assets.Game.Scripts.Helpers;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private GameState _gameState;
    private AudioSource _audioSource;

    [Tooltip("Урон (в очках здоровья), наносимый пулей")]
    public float damagePower = 1;

    [Tooltip("Скорость движения пули")]
    public float bulletSpeed = 1;

    [Tooltip("Звук попадания пули")]
    public AudioClip bulletPunch;


    void Start()
    {
        _gameState = GameObject.Find("GameState").GetComponent<GameState>();
        _audioSource = gameObject.GetComponent<AudioSource>();        
    }

    void Update()
    {
        //Destroy bullet if out of screen
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.y > Screen.height ||
            screenPosition.y < 0 ||
            screenPosition.x < 0 ||
            screenPosition.x > Screen.width)
        {
            ObjectsPool<Bullet>.DeleteObjectFromScene(this);
        }      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            Zombie zombie = collision.GetComponent<Zombie>();
            zombie.HP -= damagePower;
            if (zombie.HP <= 0)
            {
                _gameState.Score += zombie.Fee;
            }
            BulletDestroy();
        }
    }

    /// <summary>
    /// Bullet destroy routine
    /// </summary>
    /// <remarks>
    /// Play punch sound and destroy
    /// </remarks>
    private void BulletDestroy()
    {
        if (bulletPunch != null)
        {
            _audioSource.loop = false;
            _audioSource.clip = bulletPunch;
            _audioSource.Play();
            
            //TODO: play sound
            ObjectsPool<Bullet>.DeleteObjectFromScene(this,bulletPunch.length);
        }
        else
        {
            ObjectsPool<Bullet>.DeleteObjectFromScene(this);
        }
    }


}
