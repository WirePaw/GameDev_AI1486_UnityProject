using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class _UIManager : MonoBehaviour
{
    // loading screen
    public GameObject loadingMenu; // set in editor
    private CanvasGroup loadingCanvasGroup;
    public float loadingSpeed;
    public bool canLoad;

    // pause menu
    public GameObject pauseMenu; // set in editor
    private List<GameObject> pauseButtons;

    //player status
    public GameObject[] hearts;
    public GameObject[] keys;
    public Sprite keySprite;
    public Sprite lostKeySprite;

    public void MovetoLevel(int buildIndex)
    {
        StartCoroutine(LoadLevel(buildIndex));
    }

    //UI-methods

    // methods to be used for buttons in the pause menu
    public void returnGameButton()
    {
        Time.timeScale = 1f;
        FindFirstObjectByType<_AudioManager>().Play("background_level");
        pauseMenu.SetActive(false);
    }

    public void restartLevelButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        FindFirstObjectByType<_AudioManager>().Play("background_level");
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    public void mainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
        FindFirstObjectByType<_AudioManager>().Play("background_menu");
        pauseMenu.SetActive(false);
    }


    // methods to control the hearts in the ui
    public void loseHeart()
    {
        for (int i = hearts.Length - 1; i >= 0; i--)
        {
            if (hearts[i].activeInHierarchy)
            {
                hearts[i].SetActive(false);
                return;
            }
        }
    }

    public void gainHeart()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (!hearts[i].activeInHierarchy)
            {
                hearts[i].SetActive(true);
                return;
            }
        }
    }

    // methods to control the keys in the ui
    public void foundKey(int numberOfKeys, int maxNumberOfKeys)
    {
        keys[maxNumberOfKeys - numberOfKeys].GetComponent<Image>().overrideSprite = keySprite;
    }

    public void refreshKey()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (i > _LevelManager.currentKeys - 1)
            {
                keys[i].SetActive(false);
                keys[i].GetComponent<Image>().overrideSprite = null;
            }
            else
            {
                keys[i].SetActive(true);
                keys[i].GetComponent<Image>().overrideSprite = lostKeySprite;
            }
        }
    }

    // Coroutine to load a level via the index, while transitioning between levels via the loading screen
    public IEnumerator LoadLevel(int index)
    {
        loadingMenu.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(true));

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return StartCoroutine(FadeLoadingScreen(false));
        loadingMenu.SetActive(false);
    }

    // manipulates the loading screen to fade in and out of a blackscreen
    public IEnumerator FadeLoadingScreen(bool isEntering)
    {
        float startValue = loadingCanvasGroup.alpha;
        float targetValue = 0;
        if (isEntering) targetValue = 1;

        float time = 0;
        float duration = 1;

        while (time < duration)
        {
            loadingCanvasGroup.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;

            yield return null;
        }

        loadingCanvasGroup.alpha = targetValue;
    }

    //------------------------------------------------------

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        loadingCanvasGroup = loadingMenu.GetComponent<CanvasGroup>();
        pauseMenu.SetActive(false);

    }

    // manages keyInput, for entering adn exiting the pause menu
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("timescale: "+ Time.timeScale);
            if (!pauseMenu.activeSelf)
            {
                Time.timeScale = 0f;
                FindFirstObjectByType<_AudioManager>().Pause("background_level");
                pauseMenu.SetActive(true);
                _PlayerManager.isActive = false;
            } else
            {
                Time.timeScale = 1f;                
                FindFirstObjectByType<_AudioManager>().Play("background_level");
                pauseMenu.SetActive(false);
                _PlayerManager.isActive = true;
            }
        }
    }
}
