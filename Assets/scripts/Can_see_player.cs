using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Can_see_player : MonoBehaviour
{
    public RaycastHit2D hit;
    public LayerMask layerMask;
    public Can_follow_path entityWalker; // needs better name!!!
    private void Start()
    {
        entityWalker = GetComponentInParent<Can_follow_path>();
    }
    private void FixedUpdate()
    {
        if(entityWalker != null)
        {
            transform.eulerAngles = new Vector3 (0f, 0f, entityWalker.getAngle()+90);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

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
