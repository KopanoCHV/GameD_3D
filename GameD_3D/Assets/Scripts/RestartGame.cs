using UnityEngine;

public class RestartGame : MonoBehaviour
{
    public void LoadCurrentScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game Scene");
        Time.timeScale = 1;
    }
    public void Update()
    {
        if (Input.GetKeyDown("o"))
        {
            LoadCurrentScene();
        }


    }
}
