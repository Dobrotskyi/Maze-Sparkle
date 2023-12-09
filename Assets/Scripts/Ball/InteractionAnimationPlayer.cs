using UnityEngine;

public class InteractionAnimationPlayer : MonoBehaviour
{
    private TeleportationAbility _ability;
    private Animator _animator;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _ability = FindObjectOfType<TeleportationAbility>();
        _ability.Started += StartInteractionAnim;
        _ability.Finished += StopInteractionAnim;
    }

    private void StartInteractionAnim()
    {
        _animator.SetBool("Interaction", true);
    }

    private void StopInteractionAnim()
    {
        _animator.SetBool("Interaction", false);
    }
}
