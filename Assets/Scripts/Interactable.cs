using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Interactable : MonoBehaviour
{
    [SerializeField] private ParticleSystem _onDestroyEffect;
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

    public void DestroySelf()
    {
        Instantiate(_onDestroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        List<IInteractableAbility> abilities = FindObjectsOfType<MonoBehaviour>().OfType<IInteractableAbility>().ToList();
        foreach (var ability in abilities)
        {
            ability.Started += StartInteractableAnim;
            ability.Finished += StopInteractableAnim;
        }
    }

    private void OnDisable()
    {
        List<IInteractableAbility> abilities = FindObjectsOfType<MonoBehaviour>().OfType<IInteractableAbility>().ToList();
        foreach (var ability in abilities)
        {
            ability.Started -= StartInteractableAnim;
            ability.Finished -= StopInteractableAnim;
        }
    }

    private void StartInteractableAnim() => _animator.SetBool("Interacting", true);
    private void StopInteractableAnim() => _animator.SetBool("Interacting", false);
}
