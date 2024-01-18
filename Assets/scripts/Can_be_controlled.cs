using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can_be_controlled : MonoBehaviour
{
    Rigidbody2D rb2d; //kinematic body
    Vector2 direction;
    public float speed;
    public bool isMoving;
    AnimationManager sprite;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sprite = this.GetComponentInChildren<AnimationManager>();
    }

    void Update()
    {
        isMoving = false;
        // get input
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            isMoving = true;
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            isMoving = true;
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            isMoving = true;
            direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            isMoving = true;
            direction += Vector2.right;
        }
        if (isMoving)
        {
            direction.Normalize();
            if(sprite != null)
            {
                if (Vector2.Angle(direction, Vector2.up) > 90)
                {
                    sprite.setAngle(-Vector2.Angle(Vector2.right, direction));
                }
                else
                {
                    sprite.setAngle(Vector2.Angle(Vector2.right, direction));
                }

            }
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            //print(dir);
            rb2d.MovePosition(rb2d.position + direction * speed * Time.fixedDeltaTime);
        }
    }
}
