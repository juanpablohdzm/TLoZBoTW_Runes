using DG.Tweening;
using UnityEngine;

public class IceBlockTarget : MonoBehaviour
{
    private Sequence s;

    private void OnEnable()
    {
        s = DOTween.Sequence();
        s.SetLoops(-1);
        s.Append(transform.DOScale(new Vector3(1.0f, 0.0f, 1.0f),0.0f));
        s.Append(transform.DOScale(Vector3.one, 1.0f));
    }

    private void OnDisable()
    {
        if(s != null && s.active)
            s.Kill(true);
    }
}