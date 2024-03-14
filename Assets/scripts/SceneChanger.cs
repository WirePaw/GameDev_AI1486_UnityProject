using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    public float changeTime;
    public string sceneToChange;

    private void Update()
    {
        changeTime -= Time.deltaTime;
        if (changeTime <= 0)
        {
            SceneManager.LoadScene(sceneToChange);
            FindFirstObjectByType<_AudioManager>().Play("background_level");
        }
        
    }
}
