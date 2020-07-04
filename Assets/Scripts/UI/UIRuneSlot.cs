using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIRuneSlot : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private AnimationCurve easeCurve;
    [SerializeField] private Color highlightColor;

    public int Index { get; private set; } = -1;
    public bool IsEmpty => Index < 0;

    private Color startColor;

    private void Awake()
    {
        startColor = backgroundImage.color;
    }

    public void SetSlot(Rune rune, int index)
    {
        Index = index;
        image.sprite = rune.Profile.Icon;
        image.enabled = true;
    }

    public void Highlight()
    {
        transform.DOScale(new Vector3(1.3f, 1.0f, 1.3f), 1.0f).SetEase(easeCurve);
        backgroundImage.color = highlightColor;
    }

    public void UnHighlight()
    {
        transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 1.0f);
        backgroundImage.color = startColor;
    }
}
