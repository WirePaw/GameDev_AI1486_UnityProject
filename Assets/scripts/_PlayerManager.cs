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
    private Rigidbody2D body;

    public GameObject sprite; // set in editor

    //methods (actions?)

    private void Walk()
    {
        body.MovePosition(body.position + CBCon.GetMovingDirection() * walkingSpeed * Time.fixedDeltaTime);
        UpdatePosition();
    }

    public void Respawn()
    {
        isActive = false;
        StartCoroutine(MoveToSpawnpoint());
    }

    public void UpdatePosition()
    {
        position = body.position;
    }

    public IEnumerator MoveToSpawnpoint()
    {
        while (position != spawnpoint)
        {
            body.position = Vector2.MoveTowards(position, spawnpoint, 10 * Time.fixedDeltaTime);
            if(position == spawnpoint)
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
        body = GetComponent<Rigidbody2D>();

        UpdatePosition();
        spawnpoint = transform.position;
        //isActive = false;
    }

    private void FixedUpdate()
    {
        if(isActive)
        {
            Walk();
        }
    }
}
