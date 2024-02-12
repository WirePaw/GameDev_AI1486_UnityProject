using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // loading screen
    public GameObject loadingMenu; // set in editor
    private CanvasGroup loadingCanvasGroup;
    public float loadingSpeed;

    // pause menu
    public GameObject pauseMenu; // set in editor
    private List<GameObject> pauseButtons;
    public bool canLoad;

    //player status
    public GameObject[] hearts;
    public GameObject[] keys;
    public Sprite keySprite;
    public Sprite lostKeySprite;


    public void Awake()
    {
        loadingCanvasGroup = loadingMenu.GetComponent<CanvasGroup>();

        /*pauseButtons = new List<GameObject>();
        foreach (Transform child in pauseMenu.transform)
        {
            if (child.CompareTag("MenuButton"))
            {
                pauseButtons.Add(child.gameObject);
            }
        }*/
        pauseMenu.SetActive(false);

        if (loadingMenu.activeInHierarchy)
        {
            StartCoroutine(fadeOutLoading());
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenu.activeSelf)
            {
                Time.timeScale = 0f;
                FindFirstObjectByType<AudioManager>().Pause("background_level");
                pauseMenu.SetActive(true);
            } else
            {
                Time.timeScale = 1f;                
                FindFirstObjectByType<AudioManager>().Play("background_level");
                pauseMenu.SetActive(false);
            }
        }
    }

    public void returnGameButton()
    {
        Time.timeScale = 1f;
        FindFirstObjectByType<AudioManager>().Play("background_level");
        pauseMenu.SetActive(false);
    }

    public void restartLevelButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        FindFirstObjectByType<AudioManager>().Play("background_level");
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    public void mainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
        FindFirstObjectByType<AudioManager>().Play("background_menu");
        pauseMenu.SetActive(false);
    }


    public void advanceLevel()
    {
        StartCoroutine(AsyncLoadNextScene(SceneManager.GetActiveScene().buildIndex + 1));
    }
    public void recedeLevel()
    {
        StartCoroutine(AsyncLoadNextScene(SceneManager.GetActiveScene().buildIndex - 1));
    }
    public void restartLevel()
    {
        StartCoroutine(AsyncLoadNextScene(SceneManager.GetActiveScene().buildIndex));
    }
    public void movetoLevel(int nextIndex)
    {
        StartCoroutine(AsyncLoadNextScene(nextIndex));
    }

    public void startLevel() 
    { 
        StartCoroutine(fadeOutLoading());
        LevelManager.isActive = true;
    }

    public IEnumerator fadeInLoading()
    {
        loadingCanvasGroup.alpha = 0;
        loadingMenu.SetActive(true);
        LevelManager.isActive = false;
        while(loadingCanvasGroup.alpha < 1)
        {
            loadingCanvasGroup.alpha += loadingSpeed * Time.deltaTime;
            if(loadingCanvasGroup.alpha >= 1)
            {
                canLoad = true;
            }
            yield return null;
        }
    }
    public IEnumerator fadeOutLoading()
    {
        canLoad = false;
        while(loadingCanvasGroup.alpha > 0)
        {
            loadingCanvasGroup.alpha -= loadingSpeed * Time.deltaTime;
            if(loadingCanvasGroup.alpha <= 0)
            {
                loadingMenu.SetActive(false);
            }
            yield return null;
        }
    }
    public IEnumerator AsyncLoadNextScene(int nextIndex)
    {
        while (!canLoad)
        {
            print("x");
            StartCoroutine(fadeInLoading());
            yield return null;
        }
        if (nextIndex <= SceneManager.sceneCountInBuildSettings)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextIndex);
            while (!asyncLoad.isDone)
            {
                print("y");
                print("loading");
                yield return null;
            }
            /*
             * while(asyncLoad still loading)
             *  - give information about progression
             */
        }
        this.refreshKey();
    }

    //heart control
    public void loseHeart()
    {
        // move backwards from hearts[] -> check if current entity is enabled -> if yes, disable and end loop -> if no, move to next entity
        for(int i = hearts.Length - 1; i >= 0; i--)
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
        for(int i = 0; i < hearts.Length; i++)
        {
            if (!hearts[i].activeInHierarchy)
            {
                hearts[i].SetActive(true);
                return;
            }
        }
    }

    //key control
    public void foundKey(int numberOfKeys, int maxNumberOfKeys)
    {
        keys[maxNumberOfKeys - numberOfKeys].GetComponent<Image>().overrideSprite = keySprite;
    }

    public void refreshKey()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (i > LevelManager.numberOfKeys - 1)
            {
                keys[i].SetActive(false);
                keys[i].GetComponent<Image>().overrideSprite = null;
            } else
            {
                keys[i].SetActive(true);
                keys[i].GetComponent<Image>().overrideSprite = lostKeySprite;
            }
        }
    }
}
