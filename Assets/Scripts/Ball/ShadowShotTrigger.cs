using System.Collections;
using UnityEngine;

public class ShadowShotTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _front;
    [SerializeField] private Transform _back;
    [SerializeField] private Vector2 _minMaxClampMagnitude;

    private Rigidbody2D _rb;
    private DragAndShoot _dragAndShoot;

    private GameObject _objectToHit;

    float _timeToHitObject = 0;

    private void Awake()
    {
        _dragAndShoot = GetComponent<DragAndShoot>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float magnitudeScale = Mathf.Clamp(_rb.velocity.magnitude, _minMaxClampMagnitude.x, _minMaxClampMagnitude.y);

        _front.localPosition = _rb.velocity.normalized * magnitudeScale;
        _back.localPosition = -_rb.velocity.normalized * magnitudeScale;

        RaycastHit2D frontHit = Physics2D.Raycast(transform.position, (Vector2)_front.position - (Vector2)transform.position, Vector2.Distance(transform.position, _front.position), _layerMask);
        if (frontHit.collider != null)
        {
            if (_objectToHit == null || (frontHit.transform.gameObject != _objectToHit))
            {
                _objectToHit = frontHit.transform.gameObject;
                _dragAndShoot.CanShootShadow = true;

                float slowMotion = Mathf.Clamp(_dragAndShoot.SlowMotion - 0.05f * magnitudeScale, 0.1f, _dragAndShoot.SlowMotion);
                GameTimeScaler.ChangeTimeScale(slowMotion);

                _timeToHitObject = Time.time;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        StartCoroutine(DisableShadowShooting());
    }

    private IEnumerator DisableShadowShooting()
    {
        yield return new WaitForSeconds(Time.time - _timeToHitObject);
        _dragAndShoot.CanShootShadow = false;
        _objectToHit = null;
        GameTimeScaler.ResetTimeScale();
    }
}
