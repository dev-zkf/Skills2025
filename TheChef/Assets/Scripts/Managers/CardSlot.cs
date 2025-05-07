using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class CardSlot : MonoBehaviour
{
	public int handIndex;
	public bool played;

	public enum Owner { Player, AI}

	public Owner owner;

	private Vector3 scale;
	private int layer;

	public void Start()
	{
		layer = this.GetComponent<Canvas>().sortingOrder;
	}
	public void PlayCard()
	{
		if (played || owner == Owner.AI) return;

		for (int i = 0; i < CardManager.instance.availablePlaceableSlots.Length; i++)
		{
			var cardData = GetCardData();

			if (cardData.ManaCard)
			{
				CastMana();
				if (AudioManager.Instance != null)
					AudioManager.Instance.PlaySoundEffect(MatchManager.instance.Play, this.gameObject);
				return;
			}

			if (CardManager.instance.availablePlaceableSlots[i] && cardData.ManaCost <= MatchManager.instance.playerMana)
			{
				MatchManager.instance.playerMana -= cardData.ManaCost;
				StartCoroutine(MoveToPlayAreaWithDelay(i));
				played = true;
				owner = Owner.Player;
				CardManager.instance.availablePlaceableSlots[i] = false;
				if (AudioManager.Instance != null)
					AudioManager.Instance.PlaySoundEffect(MatchManager.instance.Play, this.gameObject);
				return;
			}
			else if (cardData.ManaCost > MatchManager.instance.playerMana)
			{
				if (AudioManager.Instance != null)
					AudioManager.Instance.PlaySoundEffect(MatchManager.instance.Denied, this.gameObject);
			}
		}
	}
	public void CastMana()
	{
        if (played) return;
        if (owner == Owner.Player)
        {
			MatchManager.instance.playerMana += GetCardData().ManaCost;
            CardManager.instance.availableSlots[handIndex] = true;
            MoveToDiscard();
            Debug.Log($"Played mana card {GetCardData().name}");
        }
        else
        {
			MatchManager.instance.aiMana += GetCardData().ManaCost;
            CardManager.instance.AI_availableSlots[handIndex] = true;
            MoveToDiscard();
            Debug.Log($"AI cast mana from card {GetCardData().name}");
        }
    }

	public void AIPlayCard()
	{
		if (played || owner == Owner.Player || GetCardData().ManaCard) return;

		for (int i = 0; i < CardManager.instance.AI_availablePlaceableSlots.Length; i++)
		{
			if (CardManager.instance.AI_availablePlaceableSlots[i])
			{
				MatchManager.instance.aiMana -= GetCardData().ManaCost;
				MoveToPlayArea(CardManager.instance.AI_placeableSlots[i]);
				CardManager.instance.AI_availablePlaceableSlots[i] = false;
				played = true;
				owner = Owner.AI;
				//StartCoroutine(AiTouchCards());
				Debug.Log($"AI played card {GetCardData().name}");
				return;
			}
		}
	}

	public void MoveToDiscardPile()
	{
		if (played || MatchManager.instance.discards <= 0) return;
        if (GetCardData().ManaCard)
        {
            AudioManager.Instance.PlaySoundEffect(MatchManager.instance.Denied, this.gameObject);
            return;
        }

		MatchManager.instance.discards--;

		if (owner == Owner.Player)
		{
			MatchManager.instance.playerMana += MatchManager.instance.discardMana;
			CardManager.instance.availableSlots[handIndex] = true;
		}
		else
		{
			MatchManager.instance.aiMana += MatchManager.instance.discardMana;
			CardManager.instance.AI_availableSlots[handIndex] = true;
		}

		MoveToDiscard();
	}
	public void AntiSoftLockDiscard()
	{
		if (played) return;

		if (owner == Owner.Player)
		{
			MatchManager.instance.playerMana += MatchManager.instance.discardMana;
			CardManager.instance.availableSlots[handIndex] = true;
			MatchManager.instance.playerHealth -= 2;
        }
		else
		{
			MatchManager.instance.aiMana += MatchManager.instance.discardMana;
			CardManager.instance.AI_availableSlots[handIndex] = true;
            MatchManager.instance.aiHealth -= 2;
        }

		MoveToDiscard();
	}


	private void MoveToPlayArea(Transform playArea)
	{
		if (owner == Owner.Player)
		{
			CardManager.instance.Hand.Remove(this);
            CardManager.instance.availableSlots[handIndex] = true;
        }
		else
		{
			CardManager.instance.aiHand.Remove(this);
            CardManager.instance.AI_availableSlots[handIndex] = true;
		}
		CardManager.instance.playedPile.Add(this);
		transform.SetParent(playArea);
		transform.localPosition = Vector3.zero;
		transform.localScale = playArea.localScale;
	}

    private void MoveToDiscard()
    {
        if (owner == Owner.Player)
        {
            CardManager.instance.Hand.Remove(this);
        }
        else
        {
            CardManager.instance.aiHand.Remove(this);
        }
        CardManager.instance.discardPile.Add(this);
        gameObject.SetActive(false);
    }
	public void DiscardCardsBecauseISaySo()
	{
        CardManager.instance.playedPile.Remove(this);
        CardManager.instance.discardPile.Add(this);
        gameObject.SetActive(false);
    }
    public Cards GetCardData()
	{
		return GetComponent<CardDisplay>().card;
	}

	private IEnumerator MoveToPlayAreaWithDelay(int slotIndex)
	{
		yield return new WaitForSeconds(0.2f);
		MoveToPlayArea(CardManager.instance.placeableSlots[slotIndex]);
	}
}
