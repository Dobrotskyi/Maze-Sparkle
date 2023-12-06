using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject _openedStatus;
    [SerializeField] private GameObject _closedStatus;
    [SerializeField] private ParticleSystem _effect;

    private int _buttonsToOpen = 0;

    public void AddToOpenButtons()
    {
        _buttonsToOpen++;
    }

    public void ButtonPressed()
    {
        if (_openedStatus.activeSelf)
            return;

        _buttonsToOpen--;
        if (_buttonsToOpen == 0)
            Open();
    }

    private void Open()
    {
        _closedStatus.SetActive(false);
        _openedStatus.SetActive(true);
        _effect.gameObject.SetActive(true);
    }
}
