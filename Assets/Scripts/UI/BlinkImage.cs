using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BlinkImage : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private AnimationCurve ease;

    private Sequence s;
    // Start is called before the first frame update
    void Start()
    {
        s = DOTween.Sequence();
        s.Append(DOTween.ToAlpha(() => image.color, x => image.color = x, 0, 1).SetEase(ease));
        s.Append(DOTween.ToAlpha(() => image.color, x => image.color = x, 1, 1).SetEase(ease));
        s.SetLoops(-1);
    }

    private void OnDestroy()
    {
        s.Kill(true);
    }
}
