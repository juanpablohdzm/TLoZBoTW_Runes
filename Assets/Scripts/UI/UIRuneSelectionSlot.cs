using UnityEngine;
using UnityEngine.UI;

public class UIRuneSelectionSlot : MonoBehaviour
{
    [SerializeField] private Image image;
    public Sprite Icon
    {
        get => image.sprite;
        set
        {
            if (value == null)
                image.enabled = false;
            else
            {
                image.enabled = true;
                image.sprite = value;
            }
        }
    }
}