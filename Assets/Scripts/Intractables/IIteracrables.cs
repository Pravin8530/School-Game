using UnityEngine;

public interface IInteractables
{

    int Priority { get; }
    public void Interact();

   public void Drop();

}