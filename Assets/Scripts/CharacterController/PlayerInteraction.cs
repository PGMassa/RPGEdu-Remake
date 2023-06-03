using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// This interface is implemented by anything the player can interact with, be it an object or a character
public interface IInteractable
{
    void Interact();
    void DisplayInteractionPrompt(); //Display UI elements to let the player know this is interactable
    void HideInteractionPrompt(); //Hide UI elements when object is not interactable anymore
}

/*
 * This class is responsible for handling interactions with any character/object that implements
 * the IInteractable interface.
 */
[RequireComponent(typeof(Rigidbody))]
public class PlayerInteraction : MonoBehaviour
{
    private List<IInteractable> interactableList; // keeps track of all the interactable objects in interaction range to the player

    private void Awake()
    {
        interactableList = new List<IInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out IInteractable interactableObject))
        {
            interactableList.Add(interactableObject); // keeping track of interactables nearby
            interactableObject.DisplayInteractionPrompt(); // let the player know they are near an interactable
        }
        else
        {
            Debug.LogWarning("The following object have an trigger collider but does not implement IInteractable: " + other.gameObject.name);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.TryGetComponent(out IInteractable interactableObject))
        {
            interactableList.Remove(interactableObject); // keeping track of interactables nearby
            interactableObject.HideInteractionPrompt(); // let the player know the object is no longer interactable
        }
    }

    //This function is called every time the player executes the InteractAction
    public void Interact()
    {
        if (interactableList.Count == 0)
        {
            Debug.Log("No interactable objects in range");
            return;
        }

        interactableList.Last().Interact();
    }
}
