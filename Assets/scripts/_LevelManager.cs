using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _LevelManager : MonoBehaviour
{
    // level attributes
    public static bool isDoorOpen;
    public static int currentKeys;
    public static int maxKeys;

    // player attributes
    public static int life = 3; // lifes of player across every level
    public static Vector3 spawnpoint; // spawnpoint of player, when hit 
    public static bool isActive; // wether player can move or not
    // all attributes are set in "Will_spawn_at_start"

    // Entity interaction
    public static void decreaseNeededKeys()
    {
        GameObject.FindGameObjectWithTag("UI").GetComponent<_UIManager>().foundKey(currentKeys, maxKeys);
        currentKeys--;
        if (currentKeys <= 0)
        {
            FindFirstObjectByType<_AudioManager>().Play("clear_level");
            isDoorOpen = true;
        }
    }
    public static void loseLife()
    {
        life--;

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
    public static void AdvanceLevel()
    {
        MovetoLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public static void RecedeLevel()
    {
        MovetoLevel(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public static void RestartLevel()
    {
        MovetoLevel(SceneManager.GetActiveScene().buildIndex);
    }
    public static void MovetoLevel(int nextIndex)
    {
        GameObject.FindGameObjectWithTag("UI").GetComponent<_UIManager>().MovetoLevel(nextIndex);
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
        GUI.Label(new Rect(10, 35, 300, 20), "doorIsOpen: " + _LevelManager.isDoorOpen);
        GUI.Label(new Rect(10, 60, 300, 20), "numberOfKeys: " + _LevelManager.currentKeys);
    }
}
