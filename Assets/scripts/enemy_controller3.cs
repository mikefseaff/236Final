﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_controller3 : MonoBehaviour
{
    public float health = 31;
    public float speedX;
    public float speedY;
   
    private Rigidbody2D rb;
 
    private float timerBullet;
    private float maxTimerBullet;
    public GameObject bullet;

    public float maxTimerDelay = 30f;
    private float timerDelay = 0f;

    public float timerMin = 5f;
    public float timerMax = 25f;
    public bool canfireBullets = true;

    private bool goingUp = true;

    public GameObject explosion;
    private float timer;
    public AudioSource enraged;
    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speedX, 0);
        timerBullet = 0;
        maxTimerBullet = Random.Range(timerMin, timerMax);


            StartCoroutine("BulletDelay");
            StartCoroutine("BulletDelay2");
            StartCoroutine("MovementDelay");
            StartCoroutine(Die(this.gameObject));

    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main.WorldToViewportPoint(transform.position).x < 0)
        {
            Destroy(this.gameObject);
        }
        if (Camera.main.WorldToViewportPoint(transform.position).x <= .9)
        {


            rb.velocity = new Vector2(0, speedY);
                IsGoingUp();
                if (goingUp)
                {
                    
                    MoveUp();
                }
                else if (goingUp == false)
                {
                    
                    MoveDown();
                }
        }
        
        
    }

    void IsGoingUp()
    {
        if (Camera.main.WorldToViewportPoint(transform.position).y >= .7 || Camera.main.WorldToViewportPoint(transform.position).y == 0)
        {
            goingUp = false;
        }
        else if (Camera.main.WorldToViewportPoint(transform.position).y <= .3)
        {
            goingUp = true; 
        }
    }
    void MoveUp()
    {
        rb.velocity = new Vector2(0, speedY);
    }


    void MoveDown()
    {
        rb.velocity = new Vector2(0, (speedY*-1));
 
    }


    void SpawnBullet1()
    {
        Vector3 spawnPoint = transform.position;
        //spawnPoint.y -= (bullet.GetComponent<Renderer>().bounds.size.y / 2);
        spawnPoint.x -= (2.2f);
        GameObject.Instantiate(bullet, spawnPoint, transform.rotation);

    }
    void SpawnBullet2()
    {
        Vector3 spawnPoint1 = transform.position;
        spawnPoint1.y -= (2.1f);
        spawnPoint1.x -= (2.2f);
        GameObject.Instantiate(bullet, spawnPoint1, transform.rotation);
       

    }
    void SpawnBullet3()
    {
        Vector3 spawnPoint2 = transform.position;
        spawnPoint2.y += (2.1f);
        spawnPoint2.x -= (2.2f);
        GameObject.Instantiate(bullet, spawnPoint2, transform.rotation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "bullet_player")
        {
            health -= 1;
            Debug.Log(health);
        }
        if (health == 0)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            GameObject boom = GameObject.Instantiate(explosion, this.transform.position, new Quaternion(0, 0, 0, 0));
            boom.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y);
            float animationTime = boom.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
            Destroy(boom.gameObject, animationTime);
            StopCoroutine("FireBullet2");
        }

    }

    IEnumerator FireBullet1()
    {
        while (true)
        {
            if (timerBullet >= maxTimerBullet)
            {
                //spawn an enemy
                SpawnBullet1();
                timerBullet = 0;
                maxTimerBullet = Random.Range(timerMin, timerMax);
                
            }

            timerBullet += .01f;
            yield return new WaitForSeconds(.01f);
        }

    }
    IEnumerator FireBullet2()
    {
        StopCoroutine("FireBullet1");
        while (true)
        {
            if (timerBullet >= maxTimerBullet)
            {
                //spawn an enemy
                SpawnBullet1();
                SpawnBullet2();
                SpawnBullet3();
                timerBullet = 0;
                maxTimerBullet = Random.Range(timerMin, timerMax);
            }

            timerBullet += .01f;
            yield return new WaitForSeconds(.01f);
        }

    }
    IEnumerator BulletDelay()
    {
        while (true)
        {
            if (Camera.main.WorldToViewportPoint(transform.position).x <= .9)
            {
                //spawn an enemy
                if (canfireBullets)
                {
                    StartCoroutine("FireBullet1");
                    

                }
                StopCoroutine("BulletDelay");
            }

            yield return new WaitForSeconds(.01f);
        }

    }
    IEnumerator BulletDelay2()
    {
        while (true)
        {
            if (health <= 30)
            {
                //spawn an enemy
                if (canfireBullets)
                {
                    enraged.Play();
                    StartCoroutine("FireBullet2");
                }
                StopCoroutine("BulletDelay2");
            }

         
            yield return new WaitForSeconds(.01f);
        }

    }
    IEnumerator MovementDelay()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        while (true)
        {
            if (timerDelay >= maxTimerDelay)
            {
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                rb.velocity = new Vector2(speedX, 0);
                StopCoroutine("MovementDelay");
            }

            timerDelay += .01f;
            yield return new WaitForSeconds(.01f);
        }

    }
    IEnumerator Die(GameObject Enemy)
    {
        while (true)
        {

            if (Enemy.gameObject.GetComponent<SpriteRenderer>().enabled == false)
            {
                Debug.Log("hi");
                if (timer >= .8f)
                {
                    Debug.Log("bye");
                    GameObject.Destroy(Enemy);
                }
                timer += .8f;
                yield return new WaitForSeconds(.8f);
            }
            yield return null;

        }



    }
}

