using UnityEngine;

public class Test_Object : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Interacted with Test_Object");
    }
    public string GetInteractionText()
    {
        return "Press E to interact with Test_Object";
    }
}

