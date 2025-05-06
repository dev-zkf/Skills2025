using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuilder : MonoBehaviour
{
    public Transform CardHolder;
    public GameObject prefab;
    
    public int cardAmount = 3;
    private IngredientSO bread;
    public List<CardChooser> choosers;
    void Start()
    {
       Setup(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    void RerollCards(bool isIngredient = true)
    {
        foreach (CardChooser c in choosers)
        {
            c.isIngredient = isIngredient;
            
            if (isIngredient)
            {
                IngredientSO randomIngredient = CookingManager.Instance.allIngredients[UnityEngine.Random.Range(0, CookingManager.Instance.allIngredients.Count)];
                c.GetComponent<Image>().sprite = randomIngredient.sprite;
                c.ingredient = randomIngredient;
            }
            else
            {
                EquipmentSO randomMethod = CookingManager.Instance.allEquipment[UnityEngine.Random.Range(0, CookingManager.Instance.allEquipment.Count)];
                c.cookingMethod = randomMethod;
                c.GetComponent<Image>().sprite = randomMethod.sprite;
            }
        }

    }

    [Button]
    public void Setup(bool isIngredient = true)
    {
        int count = choosers.Count;
        if (count < cardAmount) {
            for (int i = 0; i < cardAmount - count; i++)
            {
                GameObject obj = Instantiate(prefab, CardHolder);
                choosers.Add(obj.GetComponent<CardChooser>());
            }
        }
        else if (count > cardAmount) {
            for (int i = 0; i < count - cardAmount; i++)
            {
                Destroy(choosers[i].gameObject);
                choosers.RemoveAt(i);
            }
        }

        RerollCards(isIngredient);
    }
    
}
