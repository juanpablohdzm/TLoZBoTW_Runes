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

    public bool IsActive { get; private set; } = false;


    #region UnitTestsVariables
    #if UNITY_EDITOR
    public int AmountOfSlots => slots.Length;
    public int Index => previousIndex;
    public UIRuneSlot[] Slots => slots;
    
    #endif
    #endregion

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        PlayEnableAnimation();
    }
    private void OnDisable()
    {
        UnHighlightAllSlots();
        previousIndex = -1;
        IsActive = false;
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
        s.OnComplete(() => IsActive = true);
    }


    private void Update()
    {
        if (!IsActive) return;
        
        var index = GetCorrespondingIndex();
        if (index != previousIndex)
        {
            if (previousIndex != -1)
            {
                slots[previousIndex].UnHighlight();
            }

            slots[index].Highlight();
            previousIndex = index;
        }

        if (PlayerInput.Instance.UIRuneConfirm)
        {
            if (previousIndex != -1)
            {
                slots[previousIndex].UnHighlight();
                selectionSlot.Icon = slots[previousIndex].Image.sprite;
                OnRuneSlotSelected?.Invoke(previousIndex);
            }
        }

    }

    private int GetCorrespondingIndex()
    {
        float angle = PlayerInput.Instance.GetJoyStickAngle(ControllerType.Left);
        
        if (angle >= 198.0f && angle < 270.0f)
            return 0;
        if (angle >= 126.0f && angle < 198.0f)
                return 1;
        if (angle >= 54.0f && angle < 126.0f)
            return 2;
        if (angle >= 270 && angle < 342)
            return 4;
        if(angle>=342 || angle < 54)
            return 3;

        return -1;
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