using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
public enum CookingMethod { Raw, Boiled, Fried, Steamed, Baked, Cooked, Overcooked }

[CreateAssetMenu(menuName = "Cooking/Equipment")]
public class EquipmentSO : ScriptableObject
{
    public Sprite sprite;
    public string toolName;
    public CookingMethod Method;
}
