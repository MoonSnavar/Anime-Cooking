using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeHandler : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    [SerializeField] private CameraTransform cameraTransform;


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y) && Mathf.Abs(eventData.delta.x) > 3f)
        {
            cameraTransform.SetX(-eventData.delta.x);            
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
