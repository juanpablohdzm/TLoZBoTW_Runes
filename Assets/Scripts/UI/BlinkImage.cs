using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BlinkImage : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private AnimationCurve ease;
    
    // Start is called before the first frame update
    void Start()
    {
        Sequence s = DOTween.Sequence();
        s.Append(DOTween.ToAlpha(() => image.color, x => image.color = x, 0, 1).SetEase(ease));
        s.Append(DOTween.ToAlpha(() => image.color, x => image.color = x, 1, 1).SetEase(ease));
        s.SetLoops(-1);
    }
    
}
