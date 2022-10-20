using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public delegate void JoystickDrag(Vector2 drag,bool isDrag);

public class Dragme : MonoBehaviour, IDragHandler,IPointerDownHandler,IPointerUpHandler,IEndDragHandler
{
    public GameObject dragbar;
    private Transform bar;
    private float radius;
    public float radiusOffset;
    public JoystickDrag joystickDrag;

    #region EventSystem
    public void OnPointerDown(PointerEventData eventData)
    {
        dragbar.SetActive(true);

        Vector2 localPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            dragbar.transform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPosition
            );

        dragbar.transform.localPosition = localPosition;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(dragbar.transform.DOScale(1.1f,0.125f));
        sequence.Append(dragbar.transform.DOScale(1.03f, 0.125f));
        sequence.Append(dragbar.transform.DOScale(1.07f,0.125f));
        sequence.Append(dragbar.transform.DOScale(1.0f,0.125f));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("point up");
        dragbar.SetActive(false);
        bar.transform.localPosition = Vector2.zero;

        joystickDrag?.Invoke(Vector2.zero,false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            bar.transform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPosition
            );

        if(localPosition.magnitude > radius)
        {
            localPosition = localPosition.normalized * radius;
        }

        bar.transform.localPosition = localPosition;
        joystickDrag?.Invoke(localPosition.normalized, true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("end drag");

    }
    #endregion

    private void Start()
    {
        bar = dragbar.transform.Find("Bar");
        radius = dragbar.GetComponent<RectTransform>().rect.width / 2 - radiusOffset;
        dragbar.SetActive(false);

        /*
         * TestExtends
         * Test
         * TestInterface Walk
         */
        //Test t = new Test();
        //t.Talk();
        //t.Walk();
        //t.Jump();

        /*
         * TestExtends
         * Test
         */
        TestExtends t1 = new Test();
        t1.Talk();
        t1.Jump();

        /*
         * TestExtends
         * Test
         * TestInterface Walk
         */
        //TestInterface t2 = new Test();
        //t2.Talk();
        //t2.Walk();

        //Test t3 = new Test();
        //t3.Walk();
    }


}
