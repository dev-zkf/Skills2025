using UnityEngine;

public class CardChooser : MonoBehaviour
{
    public IngredientSO ingredient;
    public EquipmentSO cookingMethod;
    public bool isIngredient;
    public void SelectItem()
    {
        if (isIngredient) {
            GameManager.Instance.selectedIngredients.Add(ingredient);
            Debug.Log("selectedIngredient: "+ ingredient.name);
        } else {
            GameManager.Instance.selectedCookingMethods.Add(cookingMethod);
            Debug.Log("selectedCookingMethod: "+ cookingMethod);
        }
        
        this.gameObject.SetActive(false);
    }
}
