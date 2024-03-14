using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillSpawnAtStart : MonoBehaviour
{
    // prefabs to be instantiated on level start
    public GameObject playerPrefab;
    public GameObject uiPrefab;
    void Start()
    {
        Instantiate(playerPrefab, transform.position, transform.rotation);
        // ui gets instantiated once
        if(GameObject.FindGameObjectWithTag("UI") == null)
        {
            Instantiate(uiPrefab);
        }

        // set new informations for current level
        _PlayerManager.spawnpoint = transform.position;
        _LevelManager.maxKeys = GameObject.FindGameObjectsWithTag("Key").Length;
        _LevelManager.currentKeys = _LevelManager.maxKeys;

        // refresh ui, with new key-data
        GameObject.FindGameObjectWithTag("UI").GetComponent<_UIManager>().refreshKey();

        // set doorState, and check if it isn't already open
        if(_LevelManager.currentKeys > 0)
        {
            _LevelManager.isDoorOpen = false;
        }
        else
        {
            _LevelManager.isDoorOpen = true;
        }

        // start Level after everything is ready
        _LevelManager.startLevel();
    }
}
