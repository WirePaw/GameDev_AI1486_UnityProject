using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class CanBeCollected : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            if(gameObject.tag.Equals("Key"))
            {
                _LevelManager.decreaseNeededKeys();
                FindFirstObjectByType<_AudioManager>().Play("key");
            }
            Destroy(this.gameObject);
        }
    }
}
