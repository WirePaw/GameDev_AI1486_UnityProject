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
        direction = Vector2.zero;
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
                sprite.setState(direction);

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

    public Vector3 getPosition()
    {
        return transform.position;
    }

    public Vector3 getDirection()
    {
        return direction;
    }
}
