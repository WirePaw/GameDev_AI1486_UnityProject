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
    public static int life = 3;

    // reacts on player collecting keys, and updates state of the ui and the exit
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

    // reacts on player loosing a life, executing a game over, if all their lifes run out
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

    // level interaction, for moving between levels
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
    public static void startLevel()
    {
        _PlayerManager.isActive = true;
    }
}
