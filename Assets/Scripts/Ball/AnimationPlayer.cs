using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    [SerializeField] private Transform _spriteTransform;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _spriteTransform.up = (collision.contacts[0].point - (Vector2)_spriteTransform.position).normalized;
        _animator.SetTrigger("Bounce");
    }
}
