using Assets.Game.Scripts.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Zombie : MonoBehaviour
{

    private GameObject _target;
    private bool _isDead = false;
    private AudioSource _audioSource;
    private Animator _animator;
    private Dictionary<SpriteRenderer,int> _sprites = new Dictionary<SpriteRenderer, int>();

    #region Zombie characteristic
    [Header("Zombie characteristic")]

    [Tooltip("Скорость перемещения")]
    public float Speed = 1.0f;

    [Tooltip("Число очков здоровья")]
    public float HP = 1.0f;

    [Tooltip("Награда за убийство")]
    public int Fee = 1;
    #endregion

    #region Zombie sound
    [Header("Zombie sound")]

    [Tooltip("Звук убитого зомби")]
    public AudioClip deadSound;

    [Tooltip("Звуки, издаваемые зомби")]
    public AudioClip[] moanSounds;

    [Tooltip("Минимальное время повторения звука")]
    public float minMoanInterval = 1;

    [Tooltip("Максимальное время повторения звука")]
    public float maxMoanInterval = 3;
    #endregion

    #region Prefubs...
    [Space()]
    [Tooltip("Префаб убитого зомби")]
    public GameObject deadPrefub;
    #endregion

    void Start()
    {
        _target = GameObject.Find("Player");
        _audioSource = GetComponent<AudioSource>(); 
        _animator = GetComponent<Animator>();

        foreach (var sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            _sprites.Add(sprite, sprite.sortingOrder);
        }

        _animator.speed = Speed;
    }

    void Update()
    {
        
        if (HP <= 0)
        {
            _isDead = true;
            Dead();            
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, Speed * Time.deltaTime);
            SetSpriteSorting();
        }
    }


    //Start playing moan sounds when zombie is became visible
    private void OnBecameVisible()
    {
        if (moanSounds.Length > 0)
        {
            StartCoroutine(nameof(Moan));
        }
    }

    /// <summary>
    /// Moan routine while zombie is alive
    /// </summary>
    /// <remarks>
    /// Play moan sounds
    /// </remarks>
    /// <returns></returns>
    private IEnumerator Moan()
    {
        while (!_isDead)
        {
            int moanId = Random.Range(0, moanSounds.Length);
            float moanInterval = Random.Range(minMoanInterval, maxMoanInterval);
            AudioClip currMoan = moanSounds[moanId];

            _audioSource.loop = false;
            _audioSource.clip = currMoan;
            _audioSource.Play();
            yield return new WaitForSeconds(currMoan.length + moanInterval);
        }
    }

    /// <summary>
    /// Dead routine
    /// </summary>
    /// <remarks>
    /// Play dead sound and destroy object
    /// </remarks>
    private void Dead()
    {

        if (deadPrefub != null)
        {
            Instantiate(deadPrefub, transform.position, transform.rotation);
        }
        

        if (deadSound != null)
        {
            _audioSource.loop = false;
            _audioSource.clip = deadSound;
            _audioSource.Play();

            ObjectsPool<Zombie>.DeleteObjectFromScene(this, deadSound.length);
        }
        else
        {
            ObjectsPool<Zombie>.DeleteObjectFromScene(this);
        }
    }

    private void SetSpriteSorting()
    {
        //int baseSortLayer = (int)(3000.0f - Vector2.Distance(transform.position, _target.transform.position));
        //foreach (var sprite in _sprites)
        //{
        //    sprite.Key.sortingOrder = baseSortLayer + sprite.Value;
        //}
    }

    public void Restore(Zombie prefub)
    {
        _isDead= false;
        HP = prefub.HP;
    }
}
