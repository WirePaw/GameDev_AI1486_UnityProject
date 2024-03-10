using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class _AnimationManager : MonoBehaviour
{
    //attributes


    //references


    //methods (actions?)



    public string type; //wether if player- or enemy-sprites should be used
    public int state;
    //needs to be reworked
    public Sprite up, down, left, right;

    private Animator anim;
    public Animator anim2;
    void Start()
    {
        anim = GetComponent<Animator>();
        type = transform.tag;
        state = 0;
        if (anim2 == null )
        {
            anim2 = anim;
        }
    }

    void FixedUpdate()
    {
        //do animation here
        switch(state)
        {
            case 0: //stehend
                anim.SetBool("isWalkingBack", false);
                anim.SetBool("isWalkingLeft", false);
                anim.SetBool("isWalkingRight", false);
                anim.SetBool("isWalkingForward", false);
                break;
            case 1: //rechts
                anim.SetBool("isWalkingBack", false);
                anim.SetBool("isWalkingLeft", false);
                anim.SetBool("isWalkingRight", true);
                anim.SetBool("isWalkingForward", false);
                break;
            case 4: //hoch
                anim.SetBool("isWalkingBack", true);
                anim.SetBool("isWalkingLeft", false);
                anim.SetBool("isWalkingRight", false);
                anim.SetBool("isWalkingForward", false);
                break;
            case 3: //links
                anim.SetBool("isWalkingBack", false);
                anim.SetBool("isWalkingLeft", true);
                anim.SetBool("isWalkingRight", false);
                anim.SetBool("isWalkingForward", false);
                break;
            case 2: //runter
                anim.SetBool("isWalkingBack", false);
                anim.SetBool("isWalkingLeft", false);
                anim.SetBool("isWalkingRight", false);
                anim.SetBool("isWalkingForward", true);
                break;
        }
        state = 0;

        if (_LevelManager.isDoorOpen)
        {
            anim2.SetBool("DoorIsOpen", true);
        }
    }
    public void setState(Vector3 direction)
    {
        var angle = Vector2.Angle(Vector2.left, direction);
        angle = direction.y > 0f ? angle : -angle;
        state = ((int)angle / 90) + 3;
        
        if (direction == Vector3.zero)
        {
            state = 0;
        }
    }
}
