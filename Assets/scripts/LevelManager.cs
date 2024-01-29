using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // level attributes
    public static bool doorIsOpen;
    public static int numberOfKeys;
    public static int numberOfEnemies;

    // player attributes
    public static int life = 3; // lifes of player across every level
    public static Vector3 spawnpoint; // spawnpoint of player, when hit 
    public static bool isActive; // wether player can move or not
    // all attributes are set in "Will_spawn_at_start"

    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        FindFirstObjectByType<AudioManager>().Play("background_level");
    }

    // Entity interaction
    public static void decreaseNeededKeys()
    {
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().foundKey(numberOfKeys);
        numberOfKeys--;
        if (numberOfKeys <= 0)
        {
            doorIsOpen = true;
        }
    }
    public static void loseLife()
    {
        life--;
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().loseHeart();
        if (life <= 0)
        {
            // kill player
            FindFirstObjectByType<AudioManager>().Play("fail_level");
            Destroy(GameObject.FindGameObjectWithTag("Player"));
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = spawnpoint;
        }
    }

    // Level interaction
    //TODO when advancing level, fadeInLoading doesn't work -> no black screen to initiate new scene (loading time is too short to notice)
    //TODO find better way to reference UIManager in LevelManager -> possibly via stati ui-attribute?
    public static void advanceLevel()
    {
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().advanceLevel();
    }
    public static void recedeLevel()
    {
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().recedeLevel();
    }
    public static void restartLevel()
    {
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().restartLevel();
    }
    public static void movetoLevel(int nextIndex)
    {
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().movetoLevel(nextIndex);
    }

    // Scene interaction
    /*
     * - open gate?
     * - open pause menu?
     * - TODO start level (when ready)
     * - 
     */

    public static void startLevel()
    {
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().startLevel();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 40), "Life: " + LevelManager.life);
        GUI.Label(new Rect(10, 35, 300, 20), "doorIsOpen: " + LevelManager.doorIsOpen);
        GUI.Label(new Rect(10, 60, 300, 20), "numberOfKeys: " + LevelManager.numberOfKeys);
        GUI.Label(new Rect(10, 85, 300, 20), "numberOfEnemies: " + LevelManager.numberOfEnemies);
    }
}
