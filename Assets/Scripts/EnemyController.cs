using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Rigidbody2D theRB;
    public float moveSpeed;
    public float rangeToChasePlayer;
    private Vector3 moveDirection;

    public Animator anim;
    // Start is called before the first frame update
    public GameObject[] deathSplatters;
    public int health = 150;
    public GameObject hitEffect;

    public bool shouldShoot;

    public GameObject bullet;
    public Transform firePoint;
    public float fireRate;
    private float fireCounter;

    public SpriteRenderer theBody;

    public float shootRange;
    void Start() { }
    // Update is called once per frame
    void Update()
    {
        //active in the scene activeInHeirarchy
        if (theBody.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer)
            {

                moveDirection = PlayerController.instance.transform.position - transform.position;

            }
            else
            {
                moveDirection = Vector3.zero;

            }

            moveDirection.Normalize();
            theRB.velocity = moveDirection * moveSpeed;

            if (shouldShoot && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < shootRange)
            {
                fireCounter -= Time.deltaTime;

                if (fireCounter <= 0)
                {
                    fireCounter = fireRate;
                    Instantiate(bullet, firePoint.position, firePoint.rotation);
                    AudioManager.instance.PlaySFX(13);
                }
            }

        }
        else
        {
            theRB.velocity = Vector2.zero;
        }
        if (moveDirection != Vector3.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    public void DamageEnemy(int damage)
    {
        Instantiate(hitEffect, transform.position, transform.rotation);
        AudioManager.instance.PlaySFX(2);
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(1);
            int index = Random.Range(0, deathSplatters.Length);
            int rotation = Random.Range(0, 4);
            Instantiate(deathSplatters[index], transform.position, Quaternion.Euler(0f, 0f, rotation * 90));
        }
    }
}
