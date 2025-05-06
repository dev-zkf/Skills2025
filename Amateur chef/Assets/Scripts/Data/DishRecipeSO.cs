using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cooking/Recipe")]
public class DishRecipeSO : ScriptableObject {
    [System.Serializable]
    public class Part {
        public IngredientCategory category;
        public IngredientSO expectedIngredient;
        public CookingMethod cookingMethod;
    }

    public string dishName;
    public List<Part> parts;
}