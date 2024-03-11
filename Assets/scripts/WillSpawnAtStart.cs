using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillSpawnAtStart : MonoBehaviour
{
    public GameObject playerPrefab; // set in editor
    public GameObject uiPrefab;
    void Start()
    {
        Instantiate(playerPrefab, transform.position, transform.rotation);
        if(GameObject.FindGameObjectWithTag("UI") == null)
        {
            Instantiate(uiPrefab);
        }

        _PlayerManager.spawnpoint = transform.position;

        _LevelManager.maxKeys = GameObject.FindGameObjectsWithTag("Key").Length;
        _LevelManager.currentKeys = _LevelManager.maxKeys;
        GameObject.FindGameObjectWithTag("UI").GetComponent<_UIManager>().refreshKey();

        print("maxKeys: "+ _LevelManager.maxKeys +" currentKeys: "+ _LevelManager.currentKeys);

        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Key"))
        {
            print(go.name);
        }
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
