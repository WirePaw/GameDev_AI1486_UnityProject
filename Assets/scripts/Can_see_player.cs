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
    public float angle;

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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Can_be_controlled>();
        sightcone = transform.Find("sightcone").gameObject;

        bodyDir = Vector3.down;
    }
    private void FixedUpdate()
    {
        if(body != null)
        {
            transform.rotation = Quaternion.LookRotation(bodyDir, Vector3.up);
            //transform.rot

            if(player != null)
            {
                distance = (player.getPosition() - transform.position).magnitude;
                if (distance < sightlength) //code optimisation -> square-root-operation costs more time than multiplication
                {
                    playerDir = (player.getPosition() - transform.position).normalized;
                    angle = Vector3.Angle(bodyDir, playerDir);
                    if (Vector3.Angle(bodyDir, playerDir) < sightwidth/2)
                    {
                        //TODO insert enemy action
                        print("player found");
                    }
                }
            }
            else
            {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Can_be_controlled>();
            }

            //get body.dir and player.dir -> get angle between (with body.dir as medium) -> compare angle -> decide if player inside sightcone
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //TODO needs to be replaced by new "sightlength and sightwidth"-system
        if (collision.CompareTag("Player"))
        {
            hit = Physics2D.Raycast(transform.position, (collision.transform.position - transform.position).normalized, 20f, layerMask);
            
            if(hit.collider != null)
            {
                Debug.DrawLine(transform.position, hit.transform.position);
                switch (hit.collider.tag)
                {
                    case "Player":
                        hit.collider.transform.gameObject.GetComponent<PlayerManager>().loseLife();
                        break;
                }
            }
        }
    }

    public void setDirection(Vector3 direction)
    {
        this.bodyDir = direction;
    }
}
