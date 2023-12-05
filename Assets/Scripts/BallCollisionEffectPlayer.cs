using UnityEngine;

public class BallCollisionEffectPlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem _wallHitEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(_wallHitEffect, transform.position, Quaternion.identity);
    }
}
