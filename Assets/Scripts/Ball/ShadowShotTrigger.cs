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
    private DragAndShoot _dragAndShoot;

    private GameObject _objectToHit;
    float _timeToHitObject = 0;

    private void Awake()
    {
        _dragAndShoot = GetComponent<DragAndShoot>();
        _rb = GetComponent<Rigidbody2D>();
        SetUpSideTriggers();
    }

    private void Update()
    {
        RotateSideTriggersBase();

        float magnitudeScale = Mathf.Clamp(_rb.velocity.magnitude, _minMaxClampMagnitude.x, _minMaxClampMagnitude.y);
        List<RaycastHit2D> triggerHits = new(3);

        _collisionTriggers[1].triggerTransform.localPosition = _rb.velocity.normalized * magnitudeScale;
        _collisionTriggers[0].triggerTransform.localPosition = new Vector2(0, _collisionTriggers[1].GetDistance() * 1.6f);
        _collisionTriggers[2].triggerTransform.localPosition = new Vector2(0, _collisionTriggers[1].GetDistance() * 1.6f);
        for (int i = 0; i < _collisionTriggers.Count; i++)
            triggerHits.Add(Physics2D.Raycast(_collisionTriggers[i].baseTransform.position, _collisionTriggers[i].GetDirection(), _collisionTriggers[i].GetDistance(), _layerMask));

        var hittedRays = triggerHits.Where(t => t.collider != null);
        if (hittedRays.Count() != 0)
        {
            float[] distances = new float[3];
            for (int i = 0; i < triggerHits.Count(); i++)
            {
                if (triggerHits[i].collider != null)
                    distances[i] = Vector2.Distance(_collisionTriggers[i].baseTransform.position, triggerHits[i].point);
                else
                    distances[i] = 999;
            }
            int minDistanceIndex = distances.MinValueIndex();

            if (_objectToHit == null || (triggerHits[minDistanceIndex].transform.gameObject != _objectToHit))
            {
                _objectToHit = triggerHits[minDistanceIndex].transform.gameObject;
                _dragAndShoot.CanShootShadow = true;
                float slowMotion = Mathf.Clamp(_dragAndShoot.SlowMotion - 0.05f * magnitudeScale, 0.1f, _dragAndShoot.SlowMotion);
                GameTimeScaler.ChangeTimeScale(slowMotion);
                _timeToHitObject = Time.time;
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

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
