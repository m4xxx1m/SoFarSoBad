using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 7.5f;

    [SerializeField] private float dashTime = 0.3f;
    [SerializeField] private float dashSpeed = 20.0f;

    [SerializeField] private float timeBetweenDashes = 0.3f;
    private bool waitAfterDash = false;
    private float waitDashTimer = 0f;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer dud;

    private Rigidbody2D rb;
    private Vector2 input;

    public bool dash = false;
    public float dashTimer = 0f;

    [SerializeField] FixedJoystick joystick;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        input = new Vector2(joystick.Horizontal, joystick.Vertical);

        if (Time.timeScale > 0f)
        {
            animator.SetBool("Walk", (input.x != 0 || input.y != 0));

            if (input.x != 0)
            {
                dud.flipX = input.x < 0;
            }
        }
        if (waitAfterDash)
        {
            dash = false;
            waitDashTimer += Time.deltaTime;
            if (waitDashTimer >= timeBetweenDashes)
            {
                waitAfterDash = false;
                waitDashTimer = 0f;
            }
            else
            {
                return;
            }
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + input.normalized * speed * Time.fixedDeltaTime);

        if (dash)
        {
            dashTimer += Time.fixedDeltaTime;

            if (dashTimer < dashTime)
            {
                rb.MovePosition(rb.position + input.normalized * dashSpeed * Time.fixedDeltaTime);
            }
            else
            {
                dash = false;
                waitAfterDash = true;
            }
        }

        if (MapManager.GetInstance() != null) MapManager.GetInstance().SetCoordinates(rb.position);
    }
}
