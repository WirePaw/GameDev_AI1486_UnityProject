using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class Can_be_collected : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(gameObject.tag.Equals("Key"))
            {
                LevelManager.decreaseNeededKeys();
            }
            Destroy(this.gameObject);
        }
    }
}
