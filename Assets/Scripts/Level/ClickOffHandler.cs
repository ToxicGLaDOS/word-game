using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickOffHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent clickOffCallback;
    bool inContext;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !inContext) {
            clickOffCallback.Invoke();
        }
    }

    // I think this method will break down if we want to be able to click children
    // of this gameObject that are outside the bounds of this gameObject
    public void OnPointerEnter(PointerEventData eventData) {
        inContext = true;
    }
 
    public void OnPointerExit(PointerEventData eventData) {
        inContext = false;
    }
}