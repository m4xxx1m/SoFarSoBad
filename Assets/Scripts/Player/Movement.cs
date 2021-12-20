using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed     = 7.5f;
    
    [SerializeField] private float dashTime  = 0.25f;
    [SerializeField] private float dashSpeed = 30.0f;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer dud;

    private Rigidbody2D rb;
    private Vector2     input;

    private bool        dash      = false;
    private float       dashTimer = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        animator.SetBool("Walk", (input.x != 0 || input.y != 0));

        if(input.x != 0)
        {
            dud.flipX = input.x < 0;
        }
        
        //dash
        if(Input.GetKeyDown(KeyCode.C))
        {
            dashTimer = 0;
            dash = true;
        }
        else if(Input.GetKeyUp(KeyCode.C))
        {
            dash = false;
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + input.normalized * speed * Time.fixedDeltaTime);
        
        if(dash)
        {
            dashTimer += Time.fixedDeltaTime;

            if(dashTimer < dashTime)
                rb.MovePosition(rb.position + input.normalized * dashSpeed * Time.fixedDeltaTime);
        }

        if(MapManager.GetInstance() != null)
            MapManager.GetInstance().SetCoordinates(rb.position);
    }
}
