using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Can_exit_from : MonoBehaviour
{
    BoxCollider2D col;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        col.isTrigger = LevelManager.doorIsOpen;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LevelManager.advanceLevel();
        }
    }
    
}
