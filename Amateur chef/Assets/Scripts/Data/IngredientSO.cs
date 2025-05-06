using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public enum IngredientCategory { Bread, Grain, Meat, Vegetable, Herb, Pasta, Egg, Seasoning }
public enum TasteTag {
    None,
    Salty,
    Sweet,
    Sour,
    Bitter,
    Umami
}

[CreateAssetMenu(menuName = "Cooking/Ingredient")]
public class IngredientSO : ScriptableObject
{
    public Sprite sprite;
    public string ingredientName;
    public IngredientCategory category;
    [EnableIf(nameof(isSeasoning))] public TasteTag tasteProfile;
    private bool isSeasoning => category == IngredientCategory.Seasoning;
}
