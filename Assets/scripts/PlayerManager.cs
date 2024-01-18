using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Vector2 spawnpoint;
    void Start()
    {
        spawnpoint = transform.position;
    }
    public void loseLife()
    {
        LevelManager.life--;
        if(LevelManager.life <= 0 )
        {
            // kill player
            Destroy(this.gameObject);
        }
        else
        {
            transform.position = spawnpoint;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 40), "Life: "+ LevelManager.life);
        GUI.Label(new Rect(10, 35, 300, 20), "doorIsOpen: " + LevelManager.doorIsOpen);
        GUI.Label(new Rect(10, 60, 300, 20), "numberOfKeys: " + LevelManager.numberOfKeys);
        GUI.Label(new Rect(10, 85, 300, 20), "numberOfEnemies: " + LevelManager.numberOfEnemies);
    }
}
