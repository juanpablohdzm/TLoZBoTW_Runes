using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UIRuneSlotHolder : MonoBehaviour
{
    public event Action<int> OnRuneSlotSelected;
    
    [SerializeField] private UIRuneSlot[] slots;
    [SerializeField] private AnimationCurve easeCurve;
    [SerializeField] private AudioClip onEnableClip;
   
    private AudioSource audioSource;
    private int previousIndex = -1;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Sequence s = DOTween.Sequence();
        transform.localScale = new Vector3(0.7f, 1.0f, 0.7f);
        s.AppendCallback(() => { audioSource.PlayOneShot(onEnableClip); });
        s.Join(transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 1.0f).SetEase(easeCurve));
    }

    private void OnDisable()
    {
        previousIndex = -1;
    }

    private void FixedUpdate()
    {
        Vector2 direction = PlayerInput.Instance.LeftThumbStick;
        var index = GetCorrespondingIndex(direction);
        if (index != previousIndex && previousIndex != -1)
        {
            slots[previousIndex].UnHighlight();
            slots[index].Highlight();
            previousIndex = index;
        }

        if (PlayerInput.Instance.RuneConfirm)
        {
            if (previousIndex != -1)
            {
                slots[previousIndex].UnHighlight();
                OnRuneSlotSelected?.Invoke(previousIndex);
            }
        }

    }

    private int GetCorrespondingIndex(Vector3 dir)
    {
        float angle = 0;
        switch (angle)
        {
            case 0 when dir.x > 0 && dir.y >=0:
                angle = Mathf.Atan(dir.y / dir.x)* Mathf.Rad2Deg;
                break;
            case 1 when dir.x == Mathf.Epsilon && dir.y >0:
                angle = 90.0f;
                break;
            case 2 when dir.x < 0:
                angle = Mathf.Atan(dir.y / dir.x) * Mathf.Rad2Deg + 180.0f;
                break;
            case 3 when dir.x == Mathf.Epsilon && dir.y< 0:
                angle = 270;
                break;
            case 4 when dir.x > 0 && dir.y< 0:
                angle = Mathf.Atan(dir.y / dir.x) * Mathf.Rad2Deg + 360.0f;
                break;
        }

        switch (angle)
        {
            case 0 when angle >= 180.0f && angle < 270.0f:
                return 0;
            case 1 when angle >= 90.0f && angle < 180.0f:
                return 1;
            case 2 when angle >= 0.0f && angle < 90.0f:
                return 2;
            case 3 when angle >= 270.0f && angle < 360.0f:
                return 3;
            default:
                throw new NotSupportedException($"Not supported angle {angle}");
        }
    }

    public int? FindOpenSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty)
                return i;
        }

        return null;
    }

    public void SetSlot(Rune rune, int index)
    {
        slots[index].SetSlot(rune,index);
    }
}