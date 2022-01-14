using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigShot : MonoBehaviour
{
    [SerializeField] private GameObject turret;
    [SerializeField] private GameObject bullet;

    [SerializeField] private Animator animator;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Time.timeScale > 0f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SoundManager soundManager = SoundManager.getInstance();
                soundManager.PlaySound(soundManager.shootClip, 0.2f);
                GameObject shot = Instantiate(bullet, turret.transform.position, turret.transform.rotation);
                animator.SetTrigger("Shoot");
            }
        }
    }
}
