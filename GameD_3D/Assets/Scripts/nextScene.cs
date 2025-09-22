using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class nextScene : MonoBehaviour
{
    public void LoadSceneByName()
    {
        SceneManager.LoadScene("Intro");
        SceneManager.LoadScene("Game Scene");
       // SceneManager.LoadScene("");

    }

    public void LoadNextInBuild()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
