using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Can_see_player : MonoBehaviour
{
    public RaycastHit2D hit;
    public LayerMask layerMask;
    public Can_follow_path body;
    public Can_be_controlled player;
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

    private void OnDrawGizmosSelected()
    {
        //length of sightcone
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * sightlength));

        //width of sightcone
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * sightlength * Mathf.Cos(sightwidth/2 * Mathf.Deg2Rad)) + (Vector3.right * sightlength * Mathf.Sin(sightwidth/2 * Mathf.Deg2Rad)));
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * sightlength * Mathf.Cos(sightwidth/2 * Mathf.Deg2Rad)) + (Vector3.left * sightlength * Mathf.Sin(sightwidth/2 * Mathf.Deg2Rad)));
    }

    private void Start()
    {
        body = GetComponentInParent<Can_follow_path>();
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
                angleOfSight = Vector3.Angle(bodyDir, Vector3.up);
                if (angleOfSight < 0) angleOfSight = 360 - angleOfSight * -1;

                transform.rotation = Quaternion.Euler(0, 0, angleOfSight);
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
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Can_be_controlled>();
            }
        }
    }

    public void setDirection(Vector3 direction)
    {
        this.bodyDir = direction;
    }
}
