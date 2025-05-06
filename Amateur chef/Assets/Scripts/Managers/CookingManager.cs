using System;
using System.Collections.Generic;
using UnityEngine;
public class CookingManager : MonoBehaviour {
    public static CookingManager Instance { get; private set; }
    [Header("Prep Phase")]
    public List<IngredientSO> allIngredients;
    public List<IngredientSO> chosenIngredients; // set at game start
    public List<EquipmentSO> allEquipment;   // player-selected per day

    [Header("Client Order")]
    public DishRecipeSO currentOrder;

    [System.Serializable]
    public class CraftedIngredient {
        public IngredientSO baseIngredient;
        public CookingMethod methodUsed;
    }

    public List<CraftedIngredient> playerDish = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: if you want to persist between scenes
    }
    public void ChooseDaySetup(List<IngredientSO> selectedIngredients, List<EquipmentSO> selectedEquipment) {
        chosenIngredients = selectedIngredients;
        allEquipment = selectedEquipment;
    }

    public void AddToDish(IngredientSO ingredient, EquipmentSO equipment) {
        if (!chosenIngredients.Contains(ingredient)) {
            Debug.LogWarning("Ingredient not available today");
            return;
        }

        
        if (equipment.Method != CookingMethod.Overcooked && !allEquipment.Contains(equipment)) {
            Debug.LogWarning("Cooking method not allowed today");
            return;
        }

        playerDish.Add(new CraftedIngredient {
            baseIngredient = ingredient,
            methodUsed = equipment.Method,
        });
    }

    public int EvaluateDish() {
        int score = 0;

        foreach (var part in currentOrder.parts) {
            var crafted = playerDish.Find(c => c.baseIngredient.category == part.category);
            if (crafted == null) {
                Debug.Log($"Missing part: {part.category}");
                score -= 3;
                continue;
            }

            bool exactIngredient = crafted.baseIngredient == part.expectedIngredient;
            bool sameCategory = crafted.baseIngredient.category == part.category;
            bool cookingMatch = part.cookingMethod == crafted.methodUsed;
            
            bool seasoningMatch = part.category == IngredientCategory.Seasoning && part.expectedIngredient.tasteProfile == crafted.baseIngredient.tasteProfile;

            int methodScore = 0;

            if (exactIngredient) score += 1;
            else if (sameCategory) score -= 1;
            else score -= 1;

            if (cookingMatch) score += 1;
            else if (!cookingMatch && part.cookingMethod == CookingMethod.Overcooked) score -= 3;
            
            if (seasoningMatch) score += 1;
            else if (part.category == IngredientCategory.Seasoning) score -= 1;
            else score -= 2;
            
            score += methodScore;

            Debug.Log($"[{part.category}] Ingredient: {crafted.baseIngredient.name} | Method: {crafted.methodUsed} | +{(exactIngredient ? 3 : sameCategory ? 2 : 0)} +Taste({cookingMatch}) +Cook({methodScore})");
        }
        
        Debug.Log($"Final Dish Score: {score}");
        return score;
    }

    public void ClearDish() {
        playerDish.Clear();
    }
}
