using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _LevelManager : MonoBehaviour
{
    //attributes
    public static bool isDoorOpen;
    public static int currentKeys;
    public static int maxKeys;
    public static int playerLifes;

    //references
    _PlayerManager player;

    //methods (actions?)
    public static void DecreaseKeys()
    {

    }

    public static void DecreaseLife()
    {

    }

    public static void MoveToLevel(int levelID)
    {

    }

    public static void AdvanceLevel()
    {
        MoveToLevel(SceneManager.GetActiveScene().buildIndex+1);
    }

    public static void RecedeLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex > 0)
        {
            MoveToLevel(SceneManager.GetActiveScene().buildIndex-1);
        }
    }

    public static void RestartLevel()
    {
        MoveToLevel(SceneManager.GetActiveScene().buildIndex);
    }




























    // level attributes
    public static bool doorIsOpen;
    public static int numberOfKeys;
    public static int maxNumberOfKeys;
    public static int numberOfEnemies;

    // player attributes
    public static int life = 3; // lifes of player across every level
    public static Vector3 spawnpoint; // spawnpoint of player, when hit 
    public static bool isActive; // wether player can move or not
    // all attributes are set in "Will_spawn_at_start"

    // Entity interaction
    public static void decreaseNeededKeys()
    {
        GameObject.FindGameObjectWithTag("UI").GetComponent<_UIManager>().foundKey(numberOfKeys, maxNumberOfKeys);
        numberOfKeys--;
        if (numberOfKeys <= 0)
        {
            FindFirstObjectByType<_AudioManager>().Play("clear_level");
            doorIsOpen = true;
        }
    }
    public static void loseLife()
    {
        life--;
        _LevelManager.isActive = false;
        GameObject.FindGameObjectWithTag("UI").GetComponent<_UIManager>().loseHeart();
        if (life <= 0)
        {
            // kill player
            FindFirstObjectByType<_AudioManager>().Play("fail_level");
            SceneManager.LoadScene("XX_EndCutscene");
        }
        else
        {
            //TODO move player to spawnpoint
            GameObject.FindGameObjectWithTag("Player").transform.position = spawnpoint;
        }
    }

    // Level interaction
    //TODO when advancing level, fadeInLoading doesn't work -> no black screen to initiate new scene (loading time is too short to notice)
    //TODO find better way to reference UIManager in LevelManager -> possibly via stati ui-attribute?
    public static void advanceLevel()
    {
        GameObject.FindGameObjectWithTag("UI").GetComponent<_UIManager>().advanceLevel();
    }
    public static void recedeLevel()
    {
        GameObject.FindGameObjectWithTag("UI").GetComponent<_UIManager>().recedeLevel();
    }
    public static void restartLevel()
    {
        GameObject.FindGameObjectWithTag("UI").GetComponent<_UIManager>().restartLevel();
    }
    public static void movetoLevel(int nextIndex)
    {
        GameObject.FindGameObjectWithTag("UI").GetComponent<_UIManager>().movetoLevel(nextIndex);
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
        GameObject.FindGameObjectWithTag("UI").GetComponent<_UIManager>().startLevel();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 40), "Life: " + _LevelManager.life);
        GUI.Label(new Rect(10, 35, 300, 20), "doorIsOpen: " + _LevelManager.doorIsOpen);
        GUI.Label(new Rect(10, 60, 300, 20), "numberOfKeys: " + _LevelManager.numberOfKeys);
        GUI.Label(new Rect(10, 85, 300, 20), "numberOfEnemies: " + _LevelManager.numberOfEnemies);
    }
}
