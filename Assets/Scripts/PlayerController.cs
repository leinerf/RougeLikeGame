using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;//make sure theres only one instance of this class
    public float moveSpeed;//allow unity editor to see this variable
    private Vector2 moveInput;//x and y axis
    // Start is called before the first frame update
    public Rigidbody2D theRB;
    public Transform gunHand;

    private Camera theCam;

    public Animator anim;

    public GameObject bulletToFire;
    public Transform firePoint;

    public float timeBetweenShots;
    private float shotCounter;
    public SpriteRenderer bodySR;
    private float activeMoveSpeed;
    public float dashSpeed = 10f, dashLength = 0.5f, dashCooldown = 1f, dashInvincibility = 0.5f;
    private float dashCoolCounter;
    [HideInInspector]
    public float dashCounter;
    private void Awake()
    {
        //before start
        instance = this;
    }

    void Start()
    {
        theCam = Camera.main;//avoids looking for the main camera all the time
        activeMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();
        //delta time moves the character in respect to the frame rate
        //transform.position += new Vector3(moveInput.x * Time.deltaTime * moveSpeed, moveInput.y * Time.deltaTime * moveSpeed, 0);//has x,y,z
        //deals with rigid body to not jitter when colliding with other objects
        //rigid body calculate movement per second move rigid body and takes care of collision
        theRB.velocity = moveInput * activeMoveSpeed;

        Vector3 mousePos = Input.mousePosition;
        Vector3 screenPos = theCam.WorldToScreenPoint(transform.localPosition);

        //check where the mouse is
        if (mousePos.x < screenPos.x)
        {
            //change the scaling to move the character
            transform.localScale = new Vector3(-1f, 1f, 1f);
            gunHand.localScale = new Vector3(-1f, -1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            gunHand.localScale = new Vector3(1f, 1f, 1f);
        }


        //rotate gun arm
        Vector2 offset = new Vector2(mousePos.x - screenPos.x, mousePos.y - screenPos.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        //quanternion math for rotations not important to know
        gunHand.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetMouseButtonDown(0))
        {
            //0 is the left button on the mouse
            Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
            shotCounter = timeBetweenShots;
        }

        if (Input.GetMouseButton(0))
        {
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                shotCounter = timeBetweenShots;
            }
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(dashCoolCounter <= 0 && dashCounter <= 0){
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
                anim.SetTrigger("dash");
                PlayerHealthController.instance.MakeInvincible(dashInvincibility);
            }
            
        }

        if(dashCounter > 0){
                dashCounter -= Time.deltaTime;
                if(dashCounter <= 0){
                    activeMoveSpeed = moveSpeed;
                    dashCoolCounter = dashCooldown;
                }
        }
        
        if(dashCoolCounter > 0){
            dashCoolCounter -= Time.deltaTime;
        }

        if (moveInput != Vector2.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }
}
