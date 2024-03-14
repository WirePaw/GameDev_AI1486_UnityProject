using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerManager : MonoBehaviour
{

    //attributes
    public static Vector2 position;
    public static bool isActive;
    public static Vector2 spawnpoint;
    public float walkingSpeed;

    //references
    private CanBeControlled CBCon;
    private Rigidbody2D body;
    public GameObject sprite; // set in editor

    // move to position, given by "CanBeControlled"
    private void Walk()
    {
        body.MovePosition((Vector2)transform.position + CBCon.GetMovingDirection() * walkingSpeed * Time.fixedDeltaTime);
        UpdatePosition();
    }

    // respawn by moving towards spawnpoint, set at start of level
    public void Respawn()
    {
        isActive = false;
        StartCoroutine(MoveToSpawnpoint());
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

    // player's position, accessable for every other script
    public void UpdatePosition()
    {
        position = transform.position;
    }

    //-------------------------------------------------------------

    // set attributes, aswell as spawnpoint
    private void Awake()
    {
        CBCon = GetComponent<CanBeControlled>();
        body = GetComponent<Rigidbody2D>();

        UpdatePosition();
        spawnpoint = transform.position;
    }

    // walk if player is allowed to
    private void FixedUpdate()
    {
        if(isActive)
        {
            Walk();
        }
    }
}
