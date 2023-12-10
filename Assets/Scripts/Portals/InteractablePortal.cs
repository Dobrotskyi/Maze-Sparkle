public class InteractablePortal : Interactable
{
    private bool _interacting = false;
    public override void DestroySelf()
    {
        _interacting = true;
        var connectedPortal = GetComponent<Portal>().ConnectedPortal.GetComponent<InteractablePortal>();
        if (!connectedPortal._interacting)
            connectedPortal.DestroySelf();
        base.DestroySelf();
    }

}
