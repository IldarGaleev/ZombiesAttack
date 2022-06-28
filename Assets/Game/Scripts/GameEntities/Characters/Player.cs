using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator _animator;

    [Tooltip("Скорость разворота игрока")]
    public float Speed = 1;



    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }


    Quaternion newRotate;
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Camera cam = Camera.main;
            Vector3 worldPos = cam.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Began)
            {
                Vector3 direction = (worldPos - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                newRotate = Quaternion.AngleAxis(angle-90, Vector3.forward);                
            }
        }       
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotate, Time.deltaTime * Speed);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            FindObjectOfType<GameState>().GameOver();
        }
    }

    public void Shoot()
    {
        _animator.SetTrigger("Shoot");
    }
}
