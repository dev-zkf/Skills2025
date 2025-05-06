using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance { get; private set; }
    
    public List<IngredientSO> selectedIngredients;
    public List<EquipmentSO> selectedCookingMethods;
    
    public IngredientSO Meat;

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

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    void Setup()
    {
        // Set up at start of day
        CookingManager.Instance.ChooseDaySetup(selectedIngredients, selectedCookingMethods);
    }
    void AddIngredient(IngredientSO ingredient, EquipmentSO equipment)
    {
        // During cooking phase
        CookingManager.Instance.AddToDish(ingredient, equipment);
    }

    [Button]
    void RateFood()
    {
        // At submit
        int score = CookingManager.Instance.EvaluateDish();
    }
}
