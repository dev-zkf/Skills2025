using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemData : MonoBehaviour
{
    [Header("Item Data")]
    public IngredientSO currentSlotItemData;

    [Header("UI References")]
    [SerializeField] public Image slotIconImage;

    public int slotIndex;

    public bool clickable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (currentSlotItemData != null)
        {
            SetData(currentSlotItemData);
        }
    }
    public void SetData(IngredientSO newItem)
    {
        if (newItem == null)
        {
            slotIconImage.sprite = null;
            currentSlotItemData = null;
            slotIconImage.gameObject.SetActive(false);
            return;
        }
		
        currentSlotItemData = newItem;
        slotIconImage.sprite = newItem.sprite;
        slotIconImage.gameObject.SetActive(true);
    }
}