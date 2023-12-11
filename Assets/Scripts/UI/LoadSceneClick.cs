using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneClick : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        LevelLoader.LoadScene(sceneName);
    }

    public void LoadNextLevel()
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        if (buildIndex == SceneManager.sceneCountInBuildSettings - 1)
            LevelLoader.LoadScene("MainMenu");
        else
            LevelLoader.LoadScene("Level_" + (PlayerInfoHolder.PassedLevels + 1));
    }

    public void ReloadCurrent()
    {
        LevelLoader.ReloadCurrent();
    }
}
