using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject prefab;
    private InteractableObject interactableObject;

    public void OnPointerDown(PointerEventData eventData)
    {
        
        interactableObject = Instantiate(prefab, transform.position, transform.rotation).GetComponent<InteractableObject>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (interactableObject != null)
            interactableObject.mouseDown = false;
    }
}
