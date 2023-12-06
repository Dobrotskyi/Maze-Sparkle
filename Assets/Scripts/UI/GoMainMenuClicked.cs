using UnityEngine;

public class GoMainMenuClicked : MonoBehaviour
{
    public void GoMainMenu()
    {
        LevelLoader.LoadScene("MainMenu");
    }
}
