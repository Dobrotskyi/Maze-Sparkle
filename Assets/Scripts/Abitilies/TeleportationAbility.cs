using System.Collections;
using UnityEngine;

public class TeleportationAbility : Ability
{
    [SerializeField] private ParticleSystem _effect;
    protected override Abilities _abilityType => Abilities.Teleportation;
    private Ball _playerBall;

    protected override IEnumerator Use()
    {
        InvokeStarted();
        while (AbilityInUse)
        {
            if (TouchInputs.TouchBegan() && !TouchInputs.OverUINotClickthrough())
            {
                Vector2 worldClickPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.touches[0].position);
                if (CanTeleportInPoint(worldClickPos))
                {
                    Instantiate(_effect, _playerBall.transform.position, Quaternion.identity);
                    _playerBall.transform.position = worldClickPos;
                    Instantiate(_effect, _playerBall.transform.position, Quaternion.identity);
                    InvokeFinished();
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _playerBall = FindObjectOfType<Ball>();
    }

    private bool CanTeleportInPoint(Vector2 point)
    {
        if (Input.touches.Length == 0) return false;

        return !Physics2D.OverlapCircle(point,
                                         _playerBall.GetComponent<CircleCollider2D>().radius * _playerBall.transform.localScale.x);
    }
}
