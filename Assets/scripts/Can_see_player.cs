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
    public GameObject sightCone;

    // sightcone attributes
    public float sightwidth; //TODO change name, so reader knows it calculates width in degree
    public float sightlength;

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
        sightCone = transform.Find("sightcone").gameObject;
    }
    private void FixedUpdate()
    {
        if(body != null)
        {
            transform.eulerAngles = new Vector3 (0f, 0f, body.getAngle()+90);
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

}
