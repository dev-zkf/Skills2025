using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookBook : MonoBehaviour
{
    public Transform CardHolder;
    public GameObject prefab;
    
    public List<UI_ItemData> Itemdata;


    public Slider categorySlider;
    public TMP_Text categoryLabel; // Optional, if you want to show the current category name

    public IngredientCategory currentCategory;
    public int catIndex;

    void Start()
    {
        // Setup slider
        IngredientCategory[] categories = (IngredientCategory[])System.Enum.GetValues(typeof(IngredientCategory));
        categorySlider.minValue = 0;
        categorySlider.maxValue = categories.Length - 1;
        categorySlider.wholeNumbers = true;
        categorySlider.onValueChanged.AddListener(OnSliderChanged);

        categorySlider.value = catIndex;
        Setup();
    }

    public void OnSliderChanged(float value)
    {
        catIndex = (int)value;
        IngredientCategory[] categories = (IngredientCategory[])System.Enum.GetValues(typeof(IngredientCategory));
        currentCategory = categories[catIndex];
        Setup();
    }
    
    [Button]
    public void Setup()
    {
        List<IngredientSO> ingredients = CookingManager.Instance.allIngredients
            .FindAll(i => i.category == currentCategory);

        int cardAmount = ingredients.Count;

        // Make sure we have enough UI objects
        for (int i = Itemdata.Count; i < cardAmount; i++)
        {
            GameObject obj = Instantiate(prefab, CardHolder);
            obj.SetActive(false); // default to inactive
            Itemdata.Add(obj.GetComponent<UI_ItemData>());
        }

        // Activate and update only what’s needed
        for (int i = 0; i < Itemdata.Count; i++)
        {
            if (i < cardAmount)
            {
                Itemdata[i].gameObject.SetActive(true);
                Itemdata[i].SetData(ingredients[i]); // assuming you have a SetData method
            }
            else
            {
                Itemdata[i].gameObject.SetActive(false);
            }
        }
        categoryLabel.text = currentCategory.ToString();
    }
    public void ChangeCategory(int direction)
    {
        IngredientCategory[] categories = (IngredientCategory[])System.Enum.GetValues(typeof(IngredientCategory));
        catIndex = (catIndex + direction + categories.Length) % categories.Length;
        currentCategory = categories[catIndex];
        Setup();
    }
    [Button]
    public void NextCategory()
    {
        ChangeCategory(1);
    }
    
    [Button]
    public void PreviousCategory()
    {
        ChangeCategory(-1);
    }

}
