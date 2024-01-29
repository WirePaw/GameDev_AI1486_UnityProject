using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Will_spawn_at_start : MonoBehaviour
{
    public GameObject ObjectToSpawn; // set in editor
    void Start()
    {
        Instantiate(ObjectToSpawn, transform.position, transform.rotation);

        // setup new level
        LevelManager.spawnpoint = transform.position;
        LevelManager.numberOfKeys = GameObject.FindGameObjectsWithTag("Key").Length;
        LevelManager.numberOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if(LevelManager.numberOfKeys > 0)
        {
            LevelManager.doorIsOpen = false;
        }
        else
        {
            LevelManager.doorIsOpen = true;
        }

        LevelManager.startLevel();
    }

}
