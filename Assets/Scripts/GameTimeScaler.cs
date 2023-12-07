using UnityEngine;

public static class GameTimeScaler
{
    public static void ChangeTimeScale(float scale)
    {
        if (Time.timeScale != 1)
            return;
        Time.timeScale = scale;
    }

    public static void ResetTimeScale()
    {
        if (Time.timeScale != 1f)
            Time.timeScale = 1;
    }

    public static void PauseTime()
    {
        Time.timeScale = 0;
    }
}
