using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float bulletSpeed = 7.5f;
    public Rigidbody2D theRB;
    public GameObject impactEffect;
    public int damageToGive = 50;
    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = transform.right * bulletSpeed;//move to the right of the object in respect to the bullet so up can be right
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        AudioManager.instance.PlaySFX(4);
        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);//destroy game object that this thing is attached to
        if(other.tag == "Enemy"){
            other.GetComponent<EnemyController>().DamageEnemy(damageToGive);
        }
        
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
