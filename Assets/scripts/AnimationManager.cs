using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public string type; //wether if player- or enemy-sprites should be used
    private int state;
    public float angle;
    //needs to be reworked
    public Sprite up, down, left, right;

    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        type = transform.tag;
        angle = 0;
        state = 0;
    }

    void FixedUpdate()
    {
        //do animation here
        getState();
        switch(state)
        {
            case 0: //rechts
                anim.SetBool("isWalkingBack", false);
                anim.SetBool("isWalkingLeft", false);
                anim.SetBool("isWalkingRight", true);
                anim.SetBool("isWalkingForward", false);
                break;
            case 1: //hoch
                anim.SetBool("isWalkingBack", true);
                anim.SetBool("isWalkingLeft", false);
                anim.SetBool("isWalkingRight", false);
                anim.SetBool("isWalkingForward", false);
                break;
            case 2: //links
                anim.SetBool("isWalkingBack", false);
                anim.SetBool("isWalkingLeft", true);
                anim.SetBool("isWalkingRight", false);
                anim.SetBool("isWalkingForward", false);
                break;
            case 3: //runter
                anim.SetBool("isWalkingBack", false);
                anim.SetBool("isWalkingLeft", false);
                anim.SetBool("isWalkingRight", false);
                anim.SetBool("isWalkingForward", true);
                break;
        }
    }

    public void setAngle(float angle)
    {
        if (angle > 360) angle = angle % 360;
        if (angle < 0) angle = 360 + angle;
        this.angle = angle;
    }

    private int getState()
    {
        /*
         * state:
         * 0 = looking right
         * 1 = looking up
         * 2 = looking left
         * 3 = looking down
         */
        //angle += 45;
        state = (int)angle / 90;
        return state;
    }
}
