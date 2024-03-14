using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class _EnemyManager : MonoBehaviour
{
    //attributes
    private Vector2 lookDirection;
    private Vector2 playerDirection;
    private Vector2 position;
    private RaycastHit2D playerHit, idleHit;
    private bool isIdleing; //not yet used
    private bool isTurning;
    private bool hasFinishedCycle;
    public float turnSpeed;
    public float sightlength, sightwidth;

    private Animator animator;

    //references
    CanFollowPath CFP;
    _AnimationManager AM;
    public LayerMask layerMask;
    public GameObject sprite; // set in editor
    public GameObject sight; // set in editor

    // method to check if enemy can see player
    void FindPlayer()
    {
        playerDirection = (_PlayerManager.position - (Vector2)transform.position);
        if (playerDirection.sqrMagnitude < sightlength * sightlength) // is player inside view-range of enemy?
        {
            if (Vector2.Angle(lookDirection, playerDirection) < sightwidth) // is player in line of sight of enemy?
            {
                playerHit = Physics2D.Raycast(transform.position, playerDirection, sightlength, layerMask);
                if (playerHit.collider != null)
                {
                    if (playerHit.collider.tag == "Player") // is nothing between player and enemy?
                    {
                        if (_PlayerManager.isActive)
                        {
                            _LevelManager.loseLife();
                        }
                    }
                }
            }
        }
    }

    // makes enemy look at a given position, used to look towards the next waypoint
    void LookAt(Vector2 position)
    {
        isTurning = true;
        position.x += 0.001f;
        StartCoroutine(Turn(position - (Vector2)transform.position));
    }
    public void UpdateLookDirection()
    {
        lookDirection = sight.transform.rotation * Vector2.up;
    }
    public IEnumerator Turn(Vector2 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        while (sight.transform.rotation != targetRotation)
        {
            sight.transform.rotation = Quaternion.RotateTowards(sight.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            UpdateLookDirection();

            if (sight.transform.rotation == targetRotation)
            {
                isTurning = false;
            }
            yield return new WaitUntil(() => Time.timeScale > 0);
        }
    }

    // enemy's behaviour -> look at next waypoint -> move towards waypoint -> wait until moving to next waypoint -> repeat
    public IEnumerator WaitTurnMoveCycle()
    {
        hasFinishedCycle = false;

        LookAt(CFP.GetWaypointPosition());
        while(isTurning)
        {
            yield return null;
        }
        CFP.MoveToWaypoint();
        while(CFP.isMoving)
        {
            yield return null;
        }
        CFP.Wait();
        while (CFP.WaitingHasFinished())
        {
            yield return null;
        }
        CFP.AdvanceToNextWaypoint();
        hasFinishedCycle = true;
    }

    // lines drawn ONLY in the editor, to check for sight-data
    private void OnDrawGizmosSelected()
    {
        //length of sightcone
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * sightlength));

        //width of sightcone
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * sightlength * Mathf.Cos(sightwidth / 2 * Mathf.Deg2Rad)) + (Vector3.right * sightlength * Mathf.Sin(sightwidth / 2 * Mathf.Deg2Rad)));
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * sightlength * Mathf.Cos(sightwidth / 2 * Mathf.Deg2Rad)) + (Vector3.left * sightlength * Mathf.Sin(sightwidth / 2 * Mathf.Deg2Rad)));

        //enemy to player
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)playerDirection.normalized*3);

        //enemy sight direction
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)lookDirection.normalized*3);

    }

    //----------------------------------------------------------------------------------

    // set attributes, aswell as get sightdata from spotlight of enemy's sight-object
    private void Awake()
    {
        CFP = GetComponentInChildren<CanFollowPath>();
        AM = sprite.GetComponent<_AnimationManager>();
        hasFinishedCycle = true;
        UpdateLookDirection();

        Light2D spotlight = sight.transform.GetChild(0).GetComponent<Light2D>();
        if ( spotlight != null )
        {
            sightlength = (spotlight.pointLightInnerRadius + spotlight.pointLightOuterRadius) / 2;
            sightwidth = (spotlight.pointLightInnerAngle + spotlight.pointLightOuterAngle) / 2;
        }
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {

        if(_PlayerManager.position != null)
        {
            FindPlayer();
        }

        if (CFP.isMoving)
        {
            animator.SetFloat("moveX", lookDirection.x);
            animator.SetFloat("moveY", lookDirection.y);
            animator.SetFloat("speed", 1f);
        } else
        {
            animator.SetFloat("speed", 0f);
        }

        if(hasFinishedCycle)
        {
            StartCoroutine(WaitTurnMoveCycle());
        }
    }
}