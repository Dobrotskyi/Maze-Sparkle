using UnityEngine;

public class GuidePage : MonoBehaviour
{
    public static bool Shown { private set; get; }

    private void OnEnable()
    {
        if (Shown == true)
            gameObject.SetActive(false);
        Shown = true;

    }
}
