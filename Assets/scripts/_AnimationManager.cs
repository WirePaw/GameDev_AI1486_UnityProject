using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class _AnimationManager : MonoBehaviour
{
    public string type; //wether if player- or enemy-sprites should be used
    public int state;

    public Animator anim2;

    Vector2 movement;
    void Start()
    {
        type = transform.tag;
        state = 0;
        anim2 = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
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
