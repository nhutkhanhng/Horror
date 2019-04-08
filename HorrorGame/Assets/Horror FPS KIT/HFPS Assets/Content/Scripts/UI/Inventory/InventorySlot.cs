﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
	public int id;

	private Inventory inventory;
	private Sprite defaultSprite;

	[HideInInspector]
	public bool isCombining;

	[HideInInspector]
	public bool isCombinable;

	void Start()
	{
		defaultSprite = GetComponent<Image> ().sprite;
		inventory = Inventory.Instance;
	}

	void Update()
	{
		if (transform.childCount > 0) {
			GetComponent<Image> ().sprite = inventory.inventorySlotFilled;

			if (transform.GetChild (0).GetComponent<InventoryItemData> ().selected) {
				GetComponent<Image> ().color = inventory.selectedColor;
			} else if(!isCombining) {
				GetComponent<Image> ().color = inventory.normalColor;
			}

			if (isCombining) {
				transform.GetChild (0).GetComponent<InventoryItemData> ().isDisabled = true;
			} else {
				transform.GetChild (0).GetComponent<InventoryItemData> ().isDisabled = false;;
			}
		} else {
			GetComponent<Image> ().sprite = defaultSprite;
			GetComponent<Image> ().color = inventory.normalColor;
		}

        if (isCombining)
        {
            inventory.CombineButton.GetComponent<Button>().interactable = false;
            inventory.CombineButton.GetComponent<EventTrigger>().enabled = false;
        }
        else
        {
            inventory.CombineButton.GetComponent<Button>().interactable = true;
            inventory.CombineButton.GetComponent<EventTrigger>().enabled = true;
            inventory.CombineButton.transform.GetChild(0).GetComponent<MenuEvents>().ChangeTextColor("Black");
        }
	}

	public void OnClick()
	{
		if (transform.childCount > 0) {
			InventoryItemData itemData = transform.GetChild (0).GetComponent<InventoryItemData> ();

			inventory.CombineButton.SetActive (false);
			inventory.UseButton.SetActive (false);

			if (!isCombining) {
				for (int i = 0; i < inventory.slots.Count; i++) {
					inventory.slots [i].GetComponent<Image> ().color = inventory.normalColor;
					if (inventory.slots [i].transform.childCount > 0) {
						inventory.slots [i].transform.GetChild (0).GetComponent<InventoryItemData> ().selected = false;
					}
				}

				GetComponent<Image> ().color = inventory.selectedColor;
				inventory.ItemLabel.text = itemData.item.Title;
				inventory.ItemDescription.text = itemData.item.Description;
				inventory.selectedID = itemData.slotID;
				itemData.selected = true;

				if(inventory.CheckItemIDInventory(itemData.item.CombineWithID)){
					if (itemData.item.Combinable) {
						inventory.CombineButton.SetActive (true);
					} else {
						inventory.CombineButton.SetActive (false);
					}
				}
				if (itemData.item.IsUsable) {
                    if (itemData.item.UseItemSwitcher)
                    {
                        if (inventory.selectedSwitcherID != itemData.item.ItemSwitcherID)
                        {
                            inventory.UseButton.SetActive(true);
                        }
                        else
                        {
                            inventory.UseButton.SetActive(false);
                        }
                    }
                    else
                    {
                        inventory.UseButton.SetActive(true);
                    }
				} else {
					inventory.UseButton.SetActive (false);
				}
			} else if(isCombinable) {
				inventory.CombineWith (itemData.item, id);
			}
		}
	}
}
