using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _EnemyManager : MonoBehaviour
{
    //attributes
    private Vector2 lookDirection;
    private Vector2 playerDirection;
    private Vector2 position;
    private RaycastHit2D playerHit, idleHit;
    public bool isIdleing;
    public bool isTurning;
    public float turnSpeed;
    public float sightlength, sightwidth;

    //references
    CanFollowPath CFP;
    LayerMask layerMask;
    public GameObject sprite; // set in editor
    public GameObject sight; // set in editor

    //methods (actions?)

    void LookAt(Vector2 position)
    {
        /*TODO maybe use coroutines and "isTurning"
         * loop, until looking at lookDir
         *      turn towards lookDir
         */

        isTurning = true;
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
                if (playerHit.collider.tag == "Player") // is nothing between player and enemy?
                {
                    if (_PlayerManager.isActive)
                    {
                        _LevelManager.DecreaseLife();
                    }
                }
            }
        }
    }

    public void UpdateLookDirection()
    {
        lookDirection = (((Vector2)sight.transform.position + Vector2.right) - (Vector2)transform.position);
    }

    public IEnumerator Turn(Vector2 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 90) * direction);
        while (sight.transform.rotation != targetRotation)
        {
            //turn here
            sight.transform.rotation = Quaternion.RotateTowards(sight.transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
            UpdateLookDirection();

            if (sight.transform.rotation == targetRotation)
            {
                isTurning = false;
            }
            yield return null;
        }
    }

    //----------------------------------------------------------------------------------

    private void Awake()
    {
        CFP = GetComponentInChildren<CanFollowPath>();
        UpdateLookDirection();
    }

    private void FixedUpdate()
    {
        if(_PlayerManager.position != null)
        {
            FindPlayer();
        }

        CFP.Wait();
        if(CFP.WaitingHasFinished())
        {
            LookAt(CFP.GetNextPosition());
            CFP.MoveToWaypoint();
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