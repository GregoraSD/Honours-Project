using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public void LoadScene(int index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }

    public void Quit()
    {
        Application.Quit();
    }
}