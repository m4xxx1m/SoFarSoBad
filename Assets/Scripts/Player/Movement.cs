using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private static Vector2 Up    = new Vector2(0,  1);
    private static Vector2 Down  = new Vector2(0, -1);
    private static Vector2 Right = new Vector2(1,  0);
    private static Vector2 Left  = new Vector2(-1, 0);

    [SerializeField] private float speed     = 7.5f;
    
    [SerializeField] private float dashTime  = 0.25f;
    [SerializeField] private float dashSpeed = 30.0f;

    private Rigidbody2D rb;
    private Vector2     input;
    private Vector2     dir = Down;

    private bool        dash      = false;
    private float       dashTimer = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameObject.SendMessage("SetDirection", dir, SendMessageOptions.DontRequireReceiver);
    }

    private void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if(input.x != 0)
            dir = (input.x > 0 ? Right : Left);
        else if(input.y != 0)
            dir = (input.y > 0 ? Up : Down);

        gameObject.SendMessage("SetDirection", dir, SendMessageOptions.DontRequireReceiver);

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

        MapManager.GetInstance().SetCoordinates(rb.position);
    }
}
