using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Interactable : MonoBehaviour
{
    [SerializeField] private ParticleSystem _onDestroyEffect;
    [SerializeField] private bool _enableDragging = true;
    private Rigidbody2D _rb;
    private Animator _animator;
    private bool _triggerParam;

    public void PositionSet()
    {
        if (!GetComponent<Collider2D>().isTrigger)
            GetComponent<Collider2D>().isTrigger = _triggerParam;

        _rb.velocity = Vector2.zero;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public virtual void DragTowards(Vector2 point)
    {
        if (GetComponent<Collider2D>().isTrigger)
        {
            _triggerParam = true;
            GetComponent<Collider2D>().isTrigger = false;
        }

        _rb.velocity = Vector2.zero;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Vector2 direction = point - (Vector2)transform.position;
        _rb.MovePosition((Vector2)transform.position + direction * Time.deltaTime * 200f);
    }

    public virtual void DestroySelf()
    {
        Instantiate(_onDestroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _rb.gravityScale = 0;
    }

    protected virtual void OnEnable()
    {
        List<IInteractableAbility> abilities = FindObjectsOfType<MonoBehaviour>(true).OfType<IInteractableAbility>().ToList();
        foreach (var ability in abilities)
        {
            if (ability is AbilityFinger && !_enableDragging)
                continue;
            ability.Started += StartInteractableAnim;
            ability.Finished += StopInteractableAnim;
        }
    }

    protected virtual void OnDisable()
    {
        List<IInteractableAbility> abilities = FindObjectsOfType<MonoBehaviour>(true).OfType<IInteractableAbility>().ToList();
        foreach (var ability in abilities)
        {
            if (ability is AbilityFinger && !_enableDragging)
                continue;
            ability.Started -= StartInteractableAnim;
            ability.Finished -= StopInteractableAnim;
        }
    }

    private void StartInteractableAnim() => _animator.SetBool("Interacting", true);
    private void StopInteractableAnim() => _animator.SetBool("Interacting", false);
}
