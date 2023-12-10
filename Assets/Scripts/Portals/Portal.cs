using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal _connectedPortal;
    [SerializeField] private GameObject _statusOn;
    [SerializeField] private GameObject _statusOff;
    private bool _blocked;

    public Portal ConnectedPortal => _connectedPortal;

    private void OnEnable()
    {
        _statusOn.SetActive(true);
        _statusOff.SetActive(false);

        var main = GetComponentInChildren<ParticleSystem>(true).main;
        var newStartRotation = main.startRotation;
        newStartRotation.constant = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        main.startRotation = newStartRotation;
    }

    private void OnDestroy()
    {
        if (_connectedPortal != null)
            Destroy(_connectedPortal.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Ball>() != null || collision.gameObject.CompareTag("ShadowBall"))
        {
            if (!_blocked && !_connectedPortal._blocked)
                _connectedPortal.Teleport(collision.transform);
        }
    }

    private void Teleport(Transform objectTransform)
    {
        StartCoroutine(BlockPortalsForTime(5f));
        objectTransform.position = transform.position;
        Rigidbody2D rb = objectTransform.GetComponent<Rigidbody2D>();
        float velocity = rb.velocity.normalized.magnitude;
        rb.velocity = Vector3.zero;
        rb.AddForce(_statusOn.transform.up * velocity, ForceMode2D.Impulse);
    }

    private IEnumerator BlockPortalsForTime(float timeInSeconds)
    {
        ToggleStatus();
        transform.GetComponent<Collider2D>().enabled = false;
        _connectedPortal.ToggleStatus();
        yield return new WaitForSeconds(timeInSeconds);
        ToggleStatus();
        _connectedPortal.ToggleStatus();
        transform.GetComponent<Collider2D>().enabled = true;
    }

    private void ToggleStatus()
    {
        _blocked = !_blocked;
        _statusOff.SetActive(!_statusOff.activeSelf);
        _statusOn.SetActive(!_statusOn.activeSelf);
    }
}
