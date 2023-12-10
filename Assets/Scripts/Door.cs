using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject _openedStatus;
    [SerializeField] private GameObject _closedStatus;
    [SerializeField] private ParticleSystem _effect;

    protected int _buttonsToOpen = 0;

    public bool IsOpened { private set; get; }

    public void AddToOpenButtons()
    {
        _buttonsToOpen++;
    }

    public void ButtonPressed()
    {
        if (_openedStatus == null || _closedStatus == null) return;

        if (_openedStatus.activeSelf)
            return;

        _buttonsToOpen--;
        if (_buttonsToOpen == 0)
            Open();
    }

    protected void Open()
    {
        IsOpened = true;
        _closedStatus.SetActive(false);
        _openedStatus.SetActive(true);
        _effect.gameObject.SetActive(true);
    }
}
