using UnityEngine;

namespace OneEyedJoe.Components
{
    public class DoInteractableComponent : MonoBehaviour
    {
        public void DoInteraction(GameObject go)
        {
            var interactable = go.GetComponent<InteractebleComponent>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}
