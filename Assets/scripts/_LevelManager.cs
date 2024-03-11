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

    // player attribute
    public static int life = 3; // lifes of player across every level

    // Entity interaction
    public static void decreaseNeededKeys()
    {
        GameObject.FindGameObjectWithTag("UI").GetComponent<_UIManager>().foundKey(currentKeys, maxKeys);
        currentKeys--;
        print("key collected: "+ currentKeys);
        if (currentKeys <= 0)
        {
            print("all keys collected");
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
            print("respawn");
            GameObject.FindGameObjectWithTag("Player").GetComponent<_PlayerManager>().Respawn();
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
        _PlayerManager.isActive = true;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 40), "Life: " + _LevelManager.life);
        GUI.Label(new Rect(10, 35, 300, 20), "doorIsOpen: " + _LevelManager.isDoorOpen);
        GUI.Label(new Rect(10, 60, 300, 20), "numberOfKeys: " + _LevelManager.currentKeys);
    }
}
