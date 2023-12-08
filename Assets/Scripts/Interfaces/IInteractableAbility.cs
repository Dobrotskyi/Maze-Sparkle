using System;

public interface IInteractableAbility
{
    public event Action Started;
    public event Action Finished;
}
