using UnityEngine;

public class InteractablePortal : Interactable
{
    [SerializeField] private bool _interactable = true;
    private bool _interacting = false;
    public override void DestroySelf()
    {
        _interacting = true;
        var connectedPortal = GetComponent<Portal>().ConnectedPortal.GetComponent<InteractablePortal>();
        if (!connectedPortal._interacting)
            connectedPortal.DestroySelf();
        base.DestroySelf();
    }

    public override void DragTowards(Vector2 point)
    {
        if (_interactable)
            base.DragTowards(point);
    }

    protected override void OnEnable()
    {
        if (_interactable)
            base.OnEnable();
    }

    protected override void OnDisable()
    {
        if (_interactable)
            base.OnDisable();
    }
}
