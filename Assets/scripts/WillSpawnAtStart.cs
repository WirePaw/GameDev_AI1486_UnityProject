using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillSpawnAtStart : MonoBehaviour
{
    public GameObject ObjectToSpawn; // set in editor
    void Start()
    {
        Instantiate(ObjectToSpawn, transform.position, transform.rotation);

        _PlayerManager.spawnpoint = transform.position;

        _LevelManager.maxKeys = GameObject.FindGameObjectsWithTag("Key").Length;
        _LevelManager.currentKeys = _LevelManager.maxKeys;
        if(_LevelManager.currentKeys > 0)
        {
            _LevelManager.isDoorOpen = false;
        }
        else
        {
            _LevelManager.isDoorOpen = true;
        }

        _LevelManager.startLevel();
    }
}
