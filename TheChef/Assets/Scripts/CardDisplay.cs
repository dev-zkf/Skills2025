using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [Expandable]
    public Cards card;

	public TMP_Text NameText;
    public Image ArtworkImage; 
    public TMP_Text StatsText;
	public TMP_Text ManaText;

    public bool isMana = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        try
        {
            NameText.text = card.Name;

            ArtworkImage.sprite = card.Artwork;

            ManaText.text = card.ManaCost.ToString();
            StatsText.text = $"{card.Attack.ToString()}/{card.Health.ToString()}";
        }
        catch(System.Exception e)
        {
            Debug.Log($"{e}: bruh fine dont work then");
        }
    }
}
