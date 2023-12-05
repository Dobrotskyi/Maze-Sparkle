using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject _openedStatus;
    [SerializeField] private GameObject _closedStatus;
    [SerializeField] private ParticleSystem _effect;

    public void Open()
    {
        _closedStatus.SetActive(false);
        _openedStatus.SetActive(true);
        _effect.gameObject.SetActive(true);
    }
}
