using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public void LoadScene(int index)
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(index, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void Quit()
    {
        Time.timeScale = 1;
        Application.Quit();
    }
}