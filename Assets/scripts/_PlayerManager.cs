using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerManager : MonoBehaviour
{
    /*TODO
     * - implement sprite -> sprite needs player direction 
     */

    //attributes
    public static Vector2 position;
    public static bool isActive;
    public static Vector2 spawnpoint;

    public float walkingSpeed;

    //references
    private CanBeControlled CBCon;
    private _AnimationManager AM;
    private Rigidbody2D body;

    public GameObject sprite; // set in editor

    //methods (actions?)

    private void Walk()
    {
        body.MovePosition((Vector2)transform.position + CBCon.GetMovingDirection() * walkingSpeed * Time.fixedDeltaTime);
        if (sprite != null)
        {
            AM.setState(CBCon.GetMovingDirection());

        }

        UpdatePosition();
    }

    public void Respawn()
    {
        isActive = false;
        StartCoroutine(MoveToSpawnpoint());
    }

    public void UpdatePosition()
    {
        position = transform.position;
    }

    public IEnumerator MoveToSpawnpoint()
    {
        while (position != spawnpoint)
        {
            //body.MovePosition();
            transform.position = Vector2.MoveTowards(position, spawnpoint, 8.7f * Time.fixedDeltaTime);
            UpdatePosition();
            if (position == spawnpoint)
            {
                isActive = true;
            }
            yield return null;
        }
    }

    //-------------------------------------------------------------

    private void Awake()
    {
        CBCon = GetComponent<CanBeControlled>();
        AM = sprite.GetComponent<_AnimationManager>();
        body = GetComponent<Rigidbody2D>();

        UpdatePosition();
        spawnpoint = transform.position;
    }

    private void FixedUpdate()
    {
        if(isActive)
        {
            Walk();
        }
    }
}
