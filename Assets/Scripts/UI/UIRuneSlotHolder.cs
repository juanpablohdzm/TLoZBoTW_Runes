using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UIRuneSlotHolder : MonoBehaviour
{
    public event Action<int> OnRuneSlotSelected;
    
    [SerializeField] private UIRuneSlot[] slots;
    [SerializeField] private UIRuneSelectionSlot selectionSlot;
    [SerializeField] private AnimationCurve easeCurve;
    [SerializeField] private AudioClip onEnableClip;
    
   
    private AudioSource audioSource;
    private int previousIndex = -1;
    private bool isActive = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        UnHighlightAllSlots();
        PlayEnableAnimation();
    }
    private void OnDisable()
    {
        previousIndex = -1;
        isActive = false;
    }

    private void UnHighlightAllSlots()
    {
        foreach (var slot in slots)
        {
            slot.UnHighlight();
        }
    }

    private void PlayEnableAnimation()
    {
        Sequence s = DOTween.Sequence();
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        s.AppendCallback(() => { audioSource.PlayOneShot(onEnableClip); });
        s.Join(transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 1.0f).SetEase(easeCurve));
        s.OnComplete(() => isActive = true);
    }


    private void Update()
    {
        if (!isActive) return;
        
        Vector2 direction = PlayerInput.Instance.LeftThumbStick;
        var index = GetCorrespondingIndex(direction);
        if (index != previousIndex)
        {
            if (previousIndex != -1)
            {
                slots[previousIndex].UnHighlight();
            }

            slots[index].Highlight();
            previousIndex = index;
        }

        if (PlayerInput.Instance.RuneConfirm)
        {
            if (previousIndex != -1)
            {
                slots[previousIndex].UnHighlight();
                selectionSlot.Icon = slots[previousIndex].Image.sprite;
                OnRuneSlotSelected?.Invoke(previousIndex);
            }
        }

    }

    private int GetCorrespondingIndex(Vector3 dir)
    {
        
        float angle = 0;
        if (dir.x > 0 && dir.y >= 0)
            angle = Mathf.Atan(dir.y / dir.x) * Mathf.Rad2Deg;
        else 
        if (dir.x == Mathf.Epsilon && dir.y > 0)
            angle = 90.0f;
        else 
        if (dir.x < 0)
            angle = Mathf.Atan(dir.y / dir.x) * Mathf.Rad2Deg + 180.0f;
        else
        if (dir.x == Mathf.Epsilon && dir.y < 0)
            angle = 270;
        else 
        if (dir.x > 0 && dir.y < 0) 
            angle = Mathf.Atan(dir.y / dir.x) * Mathf.Rad2Deg + 360.0f;


        if (angle >= 198.0f && angle < 270.0f)
            return 0;
        if (angle >= 126.0f && angle < 198.0f)
                return 1;
        if (angle >= 54.0f && angle < 126.0f)
            return 2;
        if(angle>=342 && angle < 54)
            return 3;
        return 4;
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