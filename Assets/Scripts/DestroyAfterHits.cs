using TMPro;
using UnityEngine;

public class DestroyAfterHits : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bouncesLeftField;
    [SerializeField] private ParticleSystem _destroyingEffect;
    [SerializeField] private int _maxTouches = 5;
    [SerializeField] private bool _destroyAfter0;

    public int TouchesLeft { get; private set; }

    private void Awake()
    {
        TouchesLeft = _maxTouches;
    }

    private void OnEnable()
    {
        _bouncesLeftField.text = TouchesLeft.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!enabled)
            return;
        TouchesLeft--;

        if ((!_destroyAfter0 && TouchesLeft == 0) || TouchesLeft < 0)
        {
            DestroySelf();
            return;
        }
        _bouncesLeftField.text = TouchesLeft.ToString();
    }

    private void DestroySelf()
    {
        Instantiate(_destroyingEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
