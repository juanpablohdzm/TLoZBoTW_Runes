using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Color fadeColor = Color.black;
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private AnimationCurve ease;
    
    // Start is called before the first frame update
    private void Awake()
    {
        image.gameObject.SetActive(true);
        image.color = fadeColor;
    }

    void Start()
    {
        Sequence s = DOTween.Sequence();
        s.Append(DOTween.To(() => image.color.a, x =>
        {
            Color color = image.color;
            color.a = x;
            image.color = color;
        }, 0.0f, fadeDuration).SetEase(ease));
        s.OnComplete(() => { image.gameObject.SetActive(false); });
    }
}
