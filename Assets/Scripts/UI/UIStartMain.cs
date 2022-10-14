using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIStartMain : MonoBehaviour
{
    public Button reset;
    public RectTransform left;
    public RectTransform right;
    public Image exBar;

    private Vector3 defaultLeft;
    private Vector3 defaultRight;
    private float defaultFillAmount;
    void Start()
    {
        defaultLeft = left.localPosition;
        defaultRight = right.localPosition;
        defaultFillAmount = exBar.fillAmount;
    }

    public void ReSetClick()
    {
        UIAnimation();
        string s = Application.persistentDataPath;
    }

    private void UIAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        right.localPosition = defaultRight;
        sequence.Append(right.DOAnchorPosX(-50, 0.75f)).SetEase(Ease.OutQuad);

        left.localPosition = defaultLeft;
        sequence.Insert(0.35f,left.DOAnchorPosX(50, 0.4f)).SetEase(Ease.OutQuad);

        exBar.fillAmount = 0;
        sequence.Insert(0.25f, exBar.DOFillAmount(defaultFillAmount, 0.5f)).SetEase(Ease.OutQuad);
    }
}
