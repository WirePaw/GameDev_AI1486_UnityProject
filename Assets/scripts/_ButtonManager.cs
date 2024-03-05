using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _ButtonManager : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        FindFirstObjectByType<_AudioManager>().Play("background_menu");
    }

    public void startGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        FindFirstObjectByType<_AudioManager>().Stop("background_menu");
    }

    public void changeVolume(float volume)
    {
        FindFirstObjectByType<_AudioManager>().setVolume(volume);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void debugGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        //SceneManager.LoadScene("XX_EndCutscene");
        //FindFirstObjectByType<UIManager>().refreshKey();
        FindFirstObjectByType<_AudioManager>().Stop("background_menu");
        FindFirstObjectByType<_AudioManager>().Play("background_level");
    }
}
