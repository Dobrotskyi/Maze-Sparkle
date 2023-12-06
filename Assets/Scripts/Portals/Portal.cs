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
        if (collision.CompareTag("Ball"))
        {
            _connectedPortal.Teleport(collision.transform);
        }
    }

    private void Teleport(Transform objectTransform)
    {
        StartCoroutine(BlockPortalForTime(5f));
        objectTransform.position = transform.position;
        Rigidbody2D rb = objectTransform.GetComponent<Rigidbody2D>();
        rb.velocity *= transform.right;
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
        Debug.Log("ToggleStatus");
        _statusOff.SetActive(!_statusOff.activeSelf);
        _statusOn.SetActive(!_statusOn.activeSelf);
    }
}
