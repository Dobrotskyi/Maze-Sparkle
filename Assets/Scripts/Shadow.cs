using TMPro;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bouncesLeftField;
    [SerializeField] private ParticleSystem _destroyingEffect;
    private int _maxTouches = 5;
    public int TouchesLeft { get; private set; }

    private void OnEnable()
    {
        TouchesLeft = _maxTouches;
        _bouncesLeftField.text = TouchesLeft.ToString();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        TouchesLeft--;
        if (TouchesLeft < 0)
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
