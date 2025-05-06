using NaughtyAttributes;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "Inv/New Inventory", order = 1)]
public class InventoryData : ScriptableObject
{
    public List<IngredientSO> items;
}

[System.Serializable]
public class InventoryItem
{
    public IngredientSO itemData;

    public InventoryItem(IngredientSO itemData)
    {
        this.itemData = itemData;
    }
}