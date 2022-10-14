using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public delegate void OnHoldButton(bool hold);
public class EventButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public OnHoldButton holdButton;

    public void OnPointerDown(PointerEventData eventData)
    {
        holdButton?.Invoke(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        holdButton?.Invoke(false);
    }

    void Start()
    {
        
    }
}
