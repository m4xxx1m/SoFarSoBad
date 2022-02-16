using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigShot : MonoBehaviour
{
    [SerializeField] private GameObject turret;
    [SerializeField] private GameObject bullet;

    [SerializeField] private Animator animator;

    private float currentTBS = 0f;
    [SerializeField] private float timeBetweenShoots = 0.2f;
    private bool waitAfterShoot = false;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (waitAfterShoot && currentTBS < timeBetweenShoots)
        {
            currentTBS += Time.deltaTime;
            return;
        }
        else
        {
            waitAfterShoot = false;
        }
        if (Time.timeScale > 0f)
        {
            if (Input.GetMouseButton(0))
            {
                currentTBS = 0f;
                waitAfterShoot = true;
                SoundManager soundManager = SoundManager.getInstance();
                soundManager.PlaySound(soundManager.shootClip, 0.2f);
                GameObject shot = Instantiate(bullet, turret.transform.position, turret.transform.rotation);
                animator.SetTrigger("Shoot");
            }
        }
    }
}
