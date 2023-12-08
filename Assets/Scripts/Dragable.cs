using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Dragable : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;

    public void PositionSet()
    {
        _rb.velocity = Vector2.zero;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void DragTowards(Vector2 point)
    {
        _rb.velocity = Vector2.zero;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Vector2 direction = point - (Vector2)transform.position;
        _rb.MovePosition((Vector2)transform.position + direction * Time.deltaTime * 200f);
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        AbilityFinger ability = FindObjectOfType<AbilityFinger>();
        if (ability != null)
        {
            FindObjectOfType<AbilityFinger>().Started += StartInteractableAnim;
            FindObjectOfType<AbilityFinger>().Finished += StopInteractableAnim;
        }
    }

    private void OnDisable()
    {
        AbilityFinger ability = FindObjectOfType<AbilityFinger>();
        if (ability != null)
        {
            FindObjectOfType<AbilityFinger>().Started -= StartInteractableAnim;
            FindObjectOfType<AbilityFinger>().Finished -= StopInteractableAnim;
        }
    }

    private void StartInteractableAnim() => _animator.SetBool("Interacting", true);
    private void StopInteractableAnim() => _animator.SetBool("Interacting", false);
}
