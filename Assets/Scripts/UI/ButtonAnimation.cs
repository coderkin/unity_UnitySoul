using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private Button btn;

    public void OnPointerEnter(PointerEventData eventData)
    {
        btn.transform.DOScale(new Vector3(1.2f,1.2f,0),0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        btn.transform.DOScale(Vector3.one, 0.2f);
    }

    void Start()
    {
        btn = GetComponent<Button>();
    }
}
