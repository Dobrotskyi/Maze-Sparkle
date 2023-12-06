using TMPro;
using UnityEngine;

public class DestroyAfterHits : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bouncesLeftField;
    [SerializeField] private ParticleSystem _destroyingEffect;
    [SerializeField] private int _maxTouches = 5;
    [SerializeField] private bool _destroyAfter0;

    public int TouchesLeft { get; private set; }

    private void OnEnable()
    {
        TouchesLeft = _maxTouches;
        _bouncesLeftField.text = TouchesLeft.ToString();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
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
