using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        FindFirstObjectByType<AudioManager>().Play("background_menu");
    }

    public void startGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        FindFirstObjectByType<AudioManager>().Stop("background_menu");
    }

    public void changeVolume(float volume)
    {
        FindFirstObjectByType<AudioManager>().setVolume(volume);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void debugGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        //FindFirstObjectByType<UIManager>().refreshKey();
        FindFirstObjectByType<AudioManager>().Stop("background_menu");
        FindFirstObjectByType<AudioManager>().Play("background_level");
    }
}
