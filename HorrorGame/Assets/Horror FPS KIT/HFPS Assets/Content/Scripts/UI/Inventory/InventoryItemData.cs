using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItemData : MonoBehaviour{
	public Item item;
	public string itemTitle;
	public int amount;
    public int slotID;

	[HideInInspector]
	public bool selected;

	[HideInInspector]
	public bool isDisabled;

	private Text textAmount;
	private Inventory inventory;
    private Vector2 offset;

	void Start()
	{
		inventory = Inventory.Instance;
		this.transform.position = transform.parent.transform.position;
	}

	void Update()
	{
		textAmount = transform.GetChild (0).gameObject.GetComponent<Text> ();
		if (item.ItemIdentifier == itemType.Bullets || item.ItemIdentifier == itemType.Weapon) {
			textAmount.text = amount.ToString ();
		} else {
			if (amount > 1) {
				textAmount.text = amount.ToString ();
			} else if (amount == 1) {
				textAmount.text = "";
			}
		}

		itemTitle = item.Title;
	}

	void OnDisable()
	{
		if(selected)
		inventory.Deselect (slotID);
	}
}
