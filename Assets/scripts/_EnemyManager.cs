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

    //methods (actions?)

    void LookAt(Vector2 position)
    {
        isTurning = true;
        position.x += 0.001f;
        StartCoroutine(Turn(position - (Vector2)transform.position));
    }

    void BeIdle() // do it later
    {
        /*
         * if isIdling, then:
         *      loop until random dir is gotten
         *          if random dir is not looking at wall, then:
         *              save random dir and leave loop
         *          else: 
         *              get new random dir
         *      lookAt(randomDir)
         *    
         */

        /*
        isIdleing = true;
        float random = Random.value * 360;
        Vector2 randomDirection = new Vector2(Mathf.Cos(random), Mathf.Cos(random));

        while (true) // checks if enemy would stare at a wall
        {
            idleHit = Physics2D.Raycast(transform.position, randomDirection, sightlength, layerMask);
            if (idleHit.collider == null)
            {
                break;
            }
        }

        LookAt(randomDirection); // <- should be position!
        */
    }

    void FindPlayer()
    {
        playerDirection = (_PlayerManager.position - (Vector2)transform.position);
        if (playerDirection.sqrMagnitude < sightlength * sightlength) // is player inside view-range of enemy?
        {
            if (Vector2.Angle(lookDirection, playerDirection) < sightwidth) // is player in line of sight of enemy?
            {
                playerHit = Physics2D.Raycast(transform.position, playerDirection, sightlength, layerMask);
                if(playerHit.collider != null)
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

/*
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


// THIS IS REDUNDANT - DELETE WHEN POSSIBLE




public class CanSeePlayer : MonoBehaviour
{
    public RaycastHit2D hit;
    public LayerMask layerMask;
    public CanFollowPath body;
    public CanBeControlled player;
    public GameObject sightcone;

    // sightcone attributes
    public float sightwidth; //TODO change name, so reader knows it calculates width in degree
    public float sightlength;

    //debug
    public float distance;
    public Vector3 bodyDir;
    public Vector3 playerDir;
    public float angleToPlayer;
    public float angleOfSight;

    public float angleMidCalc;

    private void OnDrawGizmosSelected()
    {
        //length of sightcone
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * sightlength));

        //width of sightcone
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * sightlength * Mathf.Cos(sightwidth/2 * Mathf.Deg2Rad)) + (Vector3.right * sightlength * Mathf.Sin(sightwidth/2 * Mathf.Deg2Rad)));
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * sightlength * Mathf.Cos(sightwidth/2 * Mathf.Deg2Rad)) + (Vector3.left * sightlength * Mathf.Sin(sightwidth/2 * Mathf.Deg2Rad)));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + bodyDir);
    }

    private void Start()
    {
        body = GetComponentInParent<CanFollowPath>();
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Can_be_controlled>();
        sightcone = transform.Find("sightcone").gameObject;

        bodyDir = Vector3.down;
    }
    private void FixedUpdate()
    {
        if(body != null)
        {
            if(bodyDir != Vector3.zero)
            {
                //TODO angleOfSight doesn't work properly -> look into Vector3.up and if-clause for negative correction

                //angleOfSight = Vector3.Angle(bodyDir, Vector3.up);
                //var difference = (transform.position + bodyDir) - transform.position;
                //angleOfSight = ((Mathf.Atan2(difference.x, difference.y) + 360) % 360) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.LookRotation(bodyDir, Vector3.right);
                //transform.rotation = Quaternion.Euler(0, 0, angleOfSight);
            }
            if(player != null)
            {
                distance = (player.getPosition() - transform.position).magnitude;
                if (distance < sightlength) //code optimisation -> square-root-operation costs more time than multiplication
                {
                    playerDir = (player.getPosition() - transform.position).normalized;
                    angleToPlayer = Vector3.Angle(bodyDir, playerDir);

                    if (angleToPlayer < sightwidth/2)
                    {
                        //TODO insert enemy action
                        //LevelManager.loseLife();
                        print("player found");
                    }
                }
            }
            else
            {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<CanBeControlled>();
            }
        }
    }

    public void setDirection(Vector3 direction)
    {
        this.bodyDir = direction;
    }
}

 */