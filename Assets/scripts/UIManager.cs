using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        loadingCanvasGroup = loadingMenu.GetComponent<CanvasGroup>();

        pauseButtons = new List<GameObject>();
        foreach (Transform child in pauseMenu.transform)
        {
            if (child.CompareTag("MenuButton"))
            {
                pauseButtons.Add(child.gameObject);
            }
        }
        pauseMenu.SetActive(false);

        if (loadingMenu.activeInHierarchy)
        {
            StartCoroutine(fadeOutLoading());
        }
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
        while(!canLoad)
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
    }
}
