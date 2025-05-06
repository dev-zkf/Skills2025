using System;
using NaughtyAttributes;
using Unity.Cinemachine;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "AUD/Create New Item", order = 1)]
public class ItemData : ScriptableObject
{
	public string itemName;
	public Sprite itemIcon;
	[ShowIf(nameof(isWeapon))] public Sprite gunModel;
	public GameObject itemPrefab;
	public ItemType type;
	public int itemValue;
	public bool stackable;
	public int stackLimit;
	
	private const string group01_Name = "WeaponData";

	[Foldout(group01_Name), ShowIf(nameof(isWeapon))] public int ammo;
	[Foldout(group01_Name), ShowIf(nameof(isWeapon))] public int magazineSize;
	[Foldout(group01_Name), ShowIf(nameof(isWeapon))] public int dmgMin;
	[Foldout(group01_Name), ShowIf(nameof(isWeapon))] public int dmgMax;
	[Foldout(group01_Name), ShowIf(nameof(isWeapon))] public float fireRate;
	[Foldout(group01_Name), ShowIf(nameof(isWeapon))] public float speed;
	[Foldout(group01_Name), ShowIf(nameof(isWeapon))] public float distance;

	private bool isWeapon;

	private void OnValidate()
	{
		isWeapon = type == ItemType.Weapon;
	}
}

public enum ItemType
{
	Junk,
	Consumable,
	Equipment,
	Weapon
}