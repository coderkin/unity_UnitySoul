using UnityEngine;
using UnityEngine.EventSystems;

public delegate void OnTouchPad(Vector2 onTouch);

public class TouchPad : MonoBehaviour,IDragHandler,IEndDragHandler
{
    private Vector2 currentPos;
    private bool endDrag = true;
    public OnTouchPad onTouchPad;

    public void OnDrag(PointerEventData eventData)
    {
        if (endDrag)
        {
            endDrag = false;
        }else
        {
            Vector2 onTouch = eventData.position - currentPos;
            if(onTouch.x > 1)
            {
                onTouch.x = 1;
            }

            if(onTouch.x < -1)
            {
                onTouch.x = -1;
            }

            if (onTouch.y > 1)
            {
                onTouch.y = 1;
            }

            if (onTouch.y < -1)
            {
                onTouch.y = -1;
            }
            onTouchPad?.Invoke(onTouch);
        }
        currentPos = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        endDrag = true;
        onTouchPad?.Invoke(Vector2.zero);
    }

    private void Start()
    {

    }
}
