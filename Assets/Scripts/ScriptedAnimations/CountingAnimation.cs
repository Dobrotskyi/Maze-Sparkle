using System.Collections;
using TMPro;
using UnityEngine;

public class CountingAnimation : MonoBehaviour
{
    public void StartAnimation(TextMeshProUGUI textField, float toNumber, float duration = 1f, bool convertToInt = true)
    {
        StartCoroutine(Animation(textField, toNumber, duration, convertToInt));
    }

    private IEnumerator Animation(TextMeshProUGUI textField, float toNumber, float duration, bool convertToInt)
    {
        float t = 0;
        float amt = 0;

        do
        {
            t += Time.unscaledDeltaTime / duration;

            amt = Mathf.Lerp(0, toNumber, t);

            if (convertToInt)
                textField.text = ((int)amt).ToString();
            else
                textField.text = ((float)amt).ToString();
            yield return new WaitForEndOfFrame();
        } while (amt != toNumber);
    }
}
