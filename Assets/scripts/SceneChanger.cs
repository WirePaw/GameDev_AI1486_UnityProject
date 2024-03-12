using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public float changeTime;
    public string sceneToChange;
    public int cutscene;

    // Update is called once per frame
    private void Update()
    {
        changeTime -= Time.deltaTime;
        if (changeTime <= 0)
        {
            SceneManager.LoadScene(sceneToChange);
            switch (cutscene)
            {
                case 0:
                    FindFirstObjectByType<_AudioManager>().Play("background_level");
                    break;
                case 1:
                    FindFirstObjectByType<_AudioManager>().Play("background_menu");
                    break;
            }
            
        }
        
    }
}
