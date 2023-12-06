using UnityEngine;

public class OnPlayerHitEffectsPlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem _collisionEffectPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(_collisionEffectPrefab, collision.contacts[0].point, Quaternion.identity);
    }
}
