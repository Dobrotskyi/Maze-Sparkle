using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal _connectedPortal;
    [SerializeField] private GameObject _statusOn;
    [SerializeField] private GameObject _statusOff;

    private void OnEnable()
    {
        _statusOn.SetActive(true);
        _statusOff.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Ball>() != null || collision.gameObject.CompareTag("ShadowBall"))
        {
            _connectedPortal.Teleport(collision.transform);
        }
    }

    private void Teleport(Transform objectTransform)
    {
        StartCoroutine(BlockPortalForTime(5f));
        objectTransform.position = transform.position;
        Rigidbody2D rb = objectTransform.GetComponent<Rigidbody2D>();
        float velocity = rb.velocity.normalized.magnitude;
        rb.velocity = Vector3.zero;
        rb.AddForce(_statusOn.transform.up * velocity, ForceMode2D.Impulse);
    }

    private IEnumerator BlockPortalForTime(float timeInSeconds)
    {
        transform.GetComponent<Collider2D>().enabled = false;
        ToggleStatus();
        yield return new WaitForSeconds(timeInSeconds);
        ToggleStatus();
        transform.GetComponent<Collider2D>().enabled = true;
    }

    private void ToggleStatus()
    {
        _statusOff.SetActive(!_statusOff.activeSelf);
        _statusOn.SetActive(!_statusOn.activeSelf);
    }
}
