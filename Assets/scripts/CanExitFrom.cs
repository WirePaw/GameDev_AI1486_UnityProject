using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanExitFrom : MonoBehaviour
{
    BoxCollider2D col;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        col.isTrigger = _LevelManager.isDoorOpen;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _PlayerManager.isActive = false;
            _LevelManager.AdvanceLevel();
        }
    }
    
}
