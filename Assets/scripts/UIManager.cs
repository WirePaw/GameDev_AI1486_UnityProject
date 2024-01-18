using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject loadingMenu;
    private CanvasGroup loadingCanvasGroup;
    public float loadingSpeed;

    public GameObject pauseMenu;
    private List<GameObject> pauseButtons;
    public bool canLoad;
    public void Start()
    {
        loadingCanvasGroup = loadingMenu.GetComponent<CanvasGroup>();

        pauseButtons = new List<GameObject>();
        foreach (Transform child in pauseMenu.transform)
        {
            if(child.CompareTag("MenuButton"))
            {
                pauseButtons.Add(child.gameObject);
            }
        }
        pauseMenu.SetActive(false);
        
        if(loadingMenu.activeInHierarchy)
        {
            StartCoroutine(fadeOutLoading());
        }

    }

    public IEnumerator fadeInLoading()
    {
        loadingCanvasGroup.alpha = 0;
        loadingMenu.SetActive(true);
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
}
