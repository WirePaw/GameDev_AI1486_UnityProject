using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Can_exit_from : MonoBehaviour
{
    BoxCollider2D col;
    UIManager ui;

    private void Start()
    {
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
        col = GetComponent<BoxCollider2D>();
        LevelManager.doorIsOpen = false;
        LevelManager.numberOfKeys = GameObject.FindGameObjectsWithTag("Key").Length;
        LevelManager.numberOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
    private void Update()
    {
        col.isTrigger = LevelManager.doorIsOpen;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ui.fadeInLoading());
            if(ui.canLoad)
            {
                StartCoroutine(AsyncLoadNextScene());
            }
        }
    }
    IEnumerator AsyncLoadNextScene()
    {
        var nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if(nextIndex <= SceneManager.sceneCountInBuildSettings)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextIndex);
            while(!asyncLoad.isDone)
            {
                print("loading");
                yield return null;
            }
            /*
             * while(asyncLoad still loading)
             *  
             */
        }
    }
}
