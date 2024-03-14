using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBeControlled : MonoBehaviour
{
    // attributes
    private Vector2 movingDirection;
    private Animator animator;


    // define direction according to KeyInput
    public void InputMovement()
    {
        movingDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            movingDirection += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            movingDirection += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            movingDirection += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            movingDirection += Vector2.right;
        }
        movingDirection.Normalize();
    }

    // getter for movingDirection
    public Vector2 GetMovingDirection()
    {
        return movingDirection;
    }

    //----------------------------------------------------------------

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // check for KeyInput, if player is allowed to move
    void Update()
    {
        if (_PlayerManager.isActive)
        {
            InputMovement();
            animator.SetFloat("moveX", movingDirection.x);
            animator.SetFloat("moveY", movingDirection.y);
            animator.SetFloat("speed", movingDirection.sqrMagnitude);
        }
    }
}
















/*
    Rigidbody2D rb2d; //kinematic body
    Vector2 direction;
    public float speed;
    public bool isMoving;
    _AnimationManager sprite;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sprite = this.GetComponentInChildren<_AnimationManager>();
    }

    void Update()
    {
        isMoving = false;
        direction = Vector2.zero;
        // get input
        if(_LevelManager.isActive)
        {
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
*/