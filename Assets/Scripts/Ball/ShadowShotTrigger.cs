using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShadowShotTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Vector2 _minMaxClampMagnitude;
    [SerializeField] List<HitTrigger> _collisionTriggers = new(3);
    [SerializeField] private Transform _sideTriggersParent;
    private Vector2 _minMaxSecondsAfterHit = new(1, 2);

    [Serializable]
    public struct HitTrigger
    {
        public Transform baseTransform;
        public Transform triggerTransform;

        public Vector2 GetDirection()
        {
            return triggerTransform.position - baseTransform.position;
        }

        public float GetDistance()
        {
            return Vector2.Distance(triggerTransform.position, baseTransform.position);
        }
    }

    private Rigidbody2D _rb;
    private Ball _playerBall;

    private GameObject _objectToHit;
    float _timeToHitObject = 0;

    private void Awake()
    {
        _playerBall = GetComponent<Ball>();
        _rb = GetComponent<Rigidbody2D>();
        SetUpSideTriggers();
    }

    private void Update()
    {
        RotateSideTriggersBase();
        if (_playerBall.ShadowWasShot)
        {
            return;
        }

        float magnitudeScale = Mathf.Clamp(_rb.velocity.magnitude, _minMaxClampMagnitude.x, _minMaxClampMagnitude.y);
        List<RaycastHit2D> raycastHits = new(3);

        _collisionTriggers[1].triggerTransform.localPosition = _rb.velocity.normalized * magnitudeScale;
        _collisionTriggers[0].triggerTransform.localPosition = new Vector2(0, _collisionTriggers[1].GetDistance() * 1.6f);
        _collisionTriggers[2].triggerTransform.localPosition = new Vector2(0, _collisionTriggers[1].GetDistance() * 1.6f);
        for (int i = 0; i < _collisionTriggers.Count; i++)
            raycastHits.Add(Physics2D.Raycast(_collisionTriggers[i].baseTransform.position, _collisionTriggers[i].GetDirection(), _collisionTriggers[i].GetDistance(), _layerMask));

        var hittedRays = raycastHits.Where(t => t.collider != null);
        if (hittedRays.Count() != 0)
        {
            float[] distances = new float[3];
            for (int i = 0; i < raycastHits.Count(); i++)
            {
                if (raycastHits[i].collider != null)
                    distances[i] = Vector2.Distance(_collisionTriggers[i].baseTransform.position, raycastHits[i].point);
                else
                    distances[i] = 999;
            }
            int minDistanceIndex = distances.MinValueIndex();

            if (_objectToHit == null || (raycastHits[minDistanceIndex].transform.gameObject != _objectToHit))
            {
                _objectToHit = raycastHits[minDistanceIndex].transform.gameObject;
                StopAllCoroutines();
                _playerBall.CanShootShadow = true;
                float slowMotion = Mathf.Clamp(_playerBall.SlowMotion - 0.05f * magnitudeScale, 0.1f, _playerBall.SlowMotion);
                _timeToHitObject = Time.time;
                GameTimeScaler.ChangeTimeScale(slowMotion);
            }
        }
    }

    private void SetUpSideTriggers()
    {
        Vector2 sizeOffest = transform.right * GetComponent<CircleCollider2D>().radius;
        _collisionTriggers[0].baseTransform.localPosition = (Vector2)_collisionTriggers[0].baseTransform.localPosition - sizeOffest;
        _collisionTriggers[2].baseTransform.localPosition = (Vector2)_collisionTriggers[2].baseTransform.localPosition + sizeOffest;
    }

    private void RotateSideTriggersBase()
    {
        _sideTriggersParent.up = _rb.velocity.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _rb.AddForce(-(collision.contacts[0].point - (Vector2)transform.position).normalized * 0.02f, ForceMode2D.Impulse);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (gameObject.activeSelf && _objectToHit != null && _playerBall.CanShootShadow)
        {
            StartCoroutine(DisableShadowShooting());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObject.activeSelf && _objectToHit != null && _playerBall.CanShootShadow)
        {
            StartCoroutine(DisableShadowShooting());
        }
    }

    private IEnumerator DisableShadowShooting()
    {
        yield return new WaitForSecondsRealtime(Mathf.Clamp(Time.time + 0.1f - _timeToHitObject, _minMaxSecondsAfterHit.x, _minMaxSecondsAfterHit.y));
        GameTimeScaler.ResetTimeScale();
        _playerBall.CanShootShadow = false;
        _objectToHit = null;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
