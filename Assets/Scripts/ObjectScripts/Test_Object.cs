using UnityEngine;

public class Test_Object : MonoBehaviour, IInteractable
{
    public void Interact(IInteractor interactor)
    {
        Debug.Log("Objeto interactuado");
    }
 
}

