using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Runtime.InteropServices;
using UnityEngine;

public class InteractItem
{
	public GameObject item;
	public bool allow;
	public float distance;
	public Transform position;

	public InteractItem(GameObject _item, float _distance, Transform _position,bool _allow = false )
	{
		this.item = _item;
		this.allow = _allow;
		this.distance = _distance;
		this.position = _position;
	}
}

public class MobileInteractManager : MonoBehaviour {

	public List<InteractItem> LstInteractItems = new List<InteractItem>();
	public float minDistance = 1;
	public int curItem = -1;

	public int preItem = -1;
	public int idxMin = -1;
	
	public KeyCode UseKey;
	public KeyCode PickupKey;

	private bool isPress;

	private DynamicObject dynamic;
	
	private static MobileInteractManager instance;

	public static MobileInteractManager Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(gameObject);
	}
	
	public void Update()
	{
		if (ControlFreak2.CF2Input.GetKeyDown(UseKey) && !isPress)
		{
			isPress = true;
			InteractManager.Instance.Interact(LstInteractItems[curItem].item);
		}

		if (ControlFreak2.CF2Input.GetKeyUp(UseKey) && isPress)
		{
			isPress = false;
		}
		
		CalculateDistance();
	}

	public void CalculateDistance()
	{
		switch (LstInteractItems.Count)
		{
			case 0:
				curItem = -1;
				preItem = -1;			
				OnOffUIPanel.Instance.ShowUse(false);
				OnOffUIPanel.Instance.ShowExam(false);
				break;
			case 1:
				curItem = 0;
				preItem = -1;
				break;
		}

		if (LstInteractItems.Count > 1)
		{
			minDistance = 1;
			for (int i = 0; i < LstInteractItems.Count; i++)
			{

				if (LstInteractItems[i].distance < minDistance)
				{
					idxMin = i;
					minDistance = LstInteractItems[i].distance;
				}
			}

			if (idxMin != curItem)
			{
				preItem = curItem;
				curItem = idxMin;
			}
		}

		

		if (curItem != -1)
		{
			if (!LstInteractItems[curItem].allow)
			{
				LstInteractItems[curItem].item.GetComponent<MobileInteract>().AllowInteract();
				LstInteractItems[curItem].allow = true;
				
				if (preItem != -1)
				{
					LstInteractItems[preItem].item.GetComponent<MobileInteract>().UnAllowInteract();
					LstInteractItems[preItem].allow = false;
				}
				
				//OnOffUIPanel.Instance.ShowUse(true, "abc", LstInteractItems[curItem].position);

				
				if ( LstInteractItems[curItem].item.GetComponent<DynamicObject>())
				{
					dynamic = LstInteractItems[curItem].item.GetComponent<DynamicObject>();
				}
				else
				{
					dynamic = null;
				}
				var ObjectTag = LstInteractItems[curItem].item.tag;
				
				
				if (!InteractManager.Instance.inUse)
				{
					if (dynamic)
					{
						if (dynamic.useType == DynamicObject.type.Locked)
						{
							if (dynamic.CheckHasKey())
							{
								HFPS_GameManager.Instance.ShowInteractSprite(1, "Unlock", UseKey.ToString(),LstInteractItems[curItem].position);
							}
							else
							{
								HFPS_GameManager.Instance.ShowInteractSprite(1, "Use", UseKey.ToString(),LstInteractItems[curItem].position);
							}
						}
						else
						{
							HFPS_GameManager.Instance.ShowInteractSprite(1, "Use", UseKey.ToString(),LstInteractItems[curItem].position);
						}
					}
					else
					{
						if (!(ObjectTag == "OnlyGrab"))
						{
							HFPS_GameManager.Instance.ShowInteractSprite(1, "Use", UseKey.ToString(),LstInteractItems[curItem].position);
						}
					}

					if (ObjectTag == "OnlyGrab")
					{
						HFPS_GameManager.Instance.ShowInteractSprite(1, "Grab", PickupKey.ToString(),LstInteractItems[curItem].position);
					}
					else if (ObjectTag == "Grab")
					{
						HFPS_GameManager.Instance.ShowInteractSprite(1, "Use", UseKey.ToString(),LstInteractItems[curItem].position);
						HFPS_GameManager.Instance.ShowInteractSprite(2, "Grab", PickupKey.ToString(),LstInteractItems[curItem].position);
					}
					else if (ObjectTag == "Paper")
					{
						HFPS_GameManager.Instance.ShowInteractSprite(1, "Examine", PickupKey.ToString(),LstInteractItems[curItem].position);
					}

					if (LstInteractItems[curItem].item.GetComponent<ExamineItem>())
					{
						ExamineManager.Instance.examineObj = LstInteractItems[curItem].item;
						if (LstInteractItems[curItem].item.GetComponent<ExamineItem>().isUsable)
						{
							HFPS_GameManager.Instance.ShowInteractSprite(1, "Use", UseKey.ToString(),LstInteractItems[curItem].position);
							HFPS_GameManager.Instance.ShowInteractSprite(2, "Examine", PickupKey.ToString(),LstInteractItems[curItem].position);
						}
						else
						{
							HFPS_GameManager.Instance.ShowInteractSprite(1, "Examine", PickupKey.ToString(),LstInteractItems[curItem].position);
						}
					}
					else
					{
						ExamineManager.Instance.examineObj = null;
					 	OnOffUIPanel.Instance.ShowExam(false);
					}

						
				}
			}
			else
			{
				OnOffUIPanel.Instance.UpdateInteractPosition(LstInteractItems[curItem].position);
			}
		}
	}
	
	public void AddInteractObject(GameObject item, float distance, Transform position)
	{
		var interactItem = new InteractItem(item,distance,position);
		LstInteractItems.Add(interactItem);
	}

	public void RemoveInteracObject(GameObject item)
	{
		var itemRemove = LstInteractItems.Find(o => o.item == item);
		if (itemRemove != null)
		{
			item.GetComponent<MobileInteract>().UnAllowInteract();
			LstInteractItems.Remove(itemRemove);
		}
		
		//Debug.LogError(LstInteractItems.Count);
	}	

	public void UpdateDistance(GameObject item, float distance)
	{
		var itemUpdate = LstInteractItems.Find(o => o.item == item);
		if (itemUpdate != null)
		{
			itemUpdate.distance = distance;
		}
	}
}
