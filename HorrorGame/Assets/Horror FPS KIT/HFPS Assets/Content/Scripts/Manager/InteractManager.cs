using System;
using System.Runtime.InteropServices;
using EasyInputVR.Misc;
using UnityEngine;
using UnityEngine.UI;
using ThunderWire.Parser;

public class InteractManager : MonoBehaviour
{

	private ParsePrimitives parser = new ParsePrimitives();
	private InputManager inputManager;
	private HFPS_GameManager gameManager;
	private ItemSwitcher itemSelector;
	private Inventory inventory;

	[Header("Raycast")] public float RayLength = 3;
	public LayerMask cullLayers;
	public LayerMask moveLayer;
	public string InteractLayer;
	public string GhostLayer;
	public string DeadZombieLayer;
	public bool isMoveTrigger;

	[Header("Crosshair Textures")] public Sprite defaultCrosshair;
	public Sprite interactCrosshair;
	private Sprite default_interactCrosshair;

	[Header("Crosshair")] private Image CrosshairUI;
	public int crosshairSize = 5;
	public int interactSize = 10;

	[Header("OculusGo Pointer")]
	public GameObject laserPointer;

	[Header("OculusGo Crosshair")] public GameObject GoCrosshair;
	public bool isOculusGo;
	public bool enableGizmos;

	private int default_interactSize;
	private int default_crosshairSize;

	[HideInInspector] public bool isHeld = false;

	[HideInInspector] public bool inUse;

	[HideInInspector] public Ray playerAim;

	[HideInInspector] public GameObject RaycastObject;

	public KeyCode UseKey;
	public KeyCode PickupKey;

	private Camera playerCam;
	private DynamicObject dynamic;

	private GameObject LastRaycastObject;

	private string RaycastTag;

	private bool correctLayer;

	private bool isPressed;
	public bool useTexture;
	private bool isSet;
	private ZombieAI zombieAi;

	private static InteractManager instance;

	public static InteractManager Instance
	{
		get { return instance; }
	}


	public AudioClip ScareSound;
	public float SoundVolume;
	public bool isScareSound;
	
	private JumpscareEffects effects;

	void Awake()
	{
		inputManager = GetComponent<ScriptManager>().inputManager;
		gameManager = GetComponent<ScriptManager>().GameManager;
		itemSelector = GetComponent<ScriptManager>().itemSwitcher;
		CrosshairUI = gameManager.Crosshair;
		default_interactCrosshair = interactCrosshair;
		default_crosshairSize = crosshairSize;
		default_interactSize = interactSize;
		playerCam = Camera.main;
		RaycastObject = null;
		dynamic = null;
		isScareSound = false;

		if (instance == null)
			instance = this;
		else Destroy(gameObject);
	}

	void SetKeys()
	{
		isSet = true;
	}

	void Update()
	{
		inventory = GetComponent<ScriptManager>().inventory;

		if (inputManager.InputsCount() > 0 && !isSet)
		{
			SetKeys();
		}

		if (inputManager.GetRefreshStatus() && isSet)
		{
			isSet = false;
		}

		
#region Interact

		if (RaycastObject != null && RaycastObject.GetComponent<ExamineItem>() == null)
		{
			if (OculusGoInteractManager.Instance.TriggerClickInput() || Input.GetKeyDown(KeyCode.E))
			{
				if (RaycastObject && !isPressed)
				{
					Interact();
					isPressed = true;
				}
			}
		}
		

		if (isPressed)
		{
			isPressed = false;
		}
		
		Ray playerAim = new Ray(laserPointer.transform.position, laserPointer.transform.forward);
		RaycastHit hit;
		RaycastHit _hit;

		if (Physics.Raycast(playerAim, out _hit, RayLength, moveLayer))
		{
			if (_hit.collider.gameObject.layer == LayerMask.NameToLayer("MoveTrigger"))
			{
				isMoveTrigger = true;
			}
			else isMoveTrigger = false;
		}
		else isMoveTrigger = false;
		
		if (Physics.Raycast(playerAim, out hit, RayLength, cullLayers))
		{
			RaycastTag = hit.collider.gameObject.tag;
			if (hit.collider.gameObject.layer == LayerMask.NameToLayer(InteractLayer))
			{
				RaycastObject = hit.collider.gameObject;
				correctLayer = true;

				if (RaycastObject.GetComponent<DynamicObject>())
				{
					dynamic = RaycastObject.GetComponent<DynamicObject>();
				}
				else
				{
					dynamic = null;
				}

				useTexture = true;

				if (LastRaycastObject)
				{
					if (!(LastRaycastObject == RaycastObject))
					{
						ResetCrosshair();
					}
				}

				LastRaycastObject = RaycastObject;
				if (!inUse)
				{
					if (RaycastObject.GetComponent<ExamineItem>())
					{
						ExamineManager.Instance.examineObj = RaycastObject;
					}
					else
					{
						ExamineManager.Instance.examineObj = null;
					}
				}
			}
			else if (RaycastObject)
			{
				correctLayer = false;
			}
		}
		else if (RaycastObject)
		{
			correctLayer = false;
		}

		if (!correctLayer)
		{
			ResetCrosshair();
			useTexture = false;
			RaycastTag = null;
			RaycastObject = null;
			dynamic = null;
			if(!ExamineManager.Instance.isObjectHeld)
				ExamineManager.Instance.examineObj = null;
		}

		if (!RaycastObject)
		{
			gameManager.HideSprites(spriteType.Interact);
			useTexture = false;
			dynamic = null;
		}

		CrosshairUpdate();
#endregion
	}

	void CrosshairUpdate()
	{
		if (useTexture)
		{
			if (isOculusGo)
			{
				GoCrosshair.GetComponent<MeshRenderer>().materials[0].color = Color.red;
			}
			else
			{
				CrosshairUI.rectTransform.sizeDelta = new Vector2(interactSize, interactSize);
				CrosshairUI.sprite = interactCrosshair;
			}
		}
		else
		{
			if (isOculusGo)
			{
				GoCrosshair.GetComponent<MeshRenderer>().materials[0].color  = Color.white;
			}
			else
			{
				CrosshairUI.rectTransform.sizeDelta = new Vector2(crosshairSize, crosshairSize);
				CrosshairUI.sprite = defaultCrosshair;
			}
		}
	}

	private void ResetCrosshair()
	{
		if (isOculusGo)
		{
			GoCrosshair.GetComponent<MeshRenderer>().materials[0].color  = Color.white;
		}
		else
		{
			crosshairSize = default_crosshairSize;
			interactSize = default_interactSize;
			interactCrosshair = default_interactCrosshair;
		}
	}

	public void CrosshairVisible(bool state)
	{
		switch (state)
		{
			case true:
				CrosshairUI.enabled = true;
				break;
			case false:
				CrosshairUI.enabled = false;
				break;
		}
	}

	public bool GetInteractBool()
	{
		if (RaycastObject)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void Interact(GameObject InteracObj = null)
	{
		
		
		if (InteracObj != null)
		{
			RaycastObject = InteracObj;
		}
		
		if (RaycastObject.GetComponent<ItemID>())
		{
			ItemID iID = RaycastObject.GetComponent<ItemID>();
			Item item = inventory.GetItem(iID.InventoryID);

			if (iID.ItemType == ItemID.Type.BackpackExpand)
			{
				inventory.ExpandSlots(iID.BackpackExpand);
				Pickup();
			}

			if (iID.ItemType == ItemID.Type.QuestItem)
			{
				QuestManager.Instance.RecordQuestItem(iID);
				if (iID.messageType == ItemID.MessageType.Quest)
				{
					gameManager.AddMessage(iID.message,QuestManager.Instance.IdQuestFinish);
				}
				Pickup();
			}
			else if (inventory.CheckInventorySpace())
			{
				if (inventory.GetItemAmount(item.ID) < item.MaxItemCount || item.MaxItemCount == 0)
				{
					if (iID.ItemType == ItemID.Type.NoInventoryItem)
					{
						itemSelector.selectItem(iID.WeaponID);
					}
					else if (iID.ItemType == ItemID.Type.InventoryItem)
					{
						if(iID.QuestID != 0)
							QuestManager.Instance.RecordQuestItem(iID);
						inventory.AddItemToSlot(iID.InventoryID, iID.Amount);
					}
					else if (iID.ItemType == ItemID.Type.WeaponItem)
					{
						if (iID.weaponType == ItemID.WeaponType.Weapon)
						{
							inventory.AddItemToSlot(iID.InventoryID, iID.Amount);
							inventory.SetWeaponID(iID.InventoryID, iID.WeaponID);
							itemSelector.selectItem(iID.WeaponID);
						}
						else if (iID.weaponType == ItemID.WeaponType.Ammo)
						{
							inventory.AddItemToSlot(iID.InventoryID, iID.Amount);
						}
					}

					if (iID.messageType == ItemID.MessageType.Hint)
					{
						gameManager.ShowHint(iID.message);
					}

					if (iID.messageType == ItemID.MessageType.Message)
					{
						gameManager.AddMessage(iID.message,QuestManager.Instance.IdQuestFinish);
					}

					if (iID.messageType == ItemID.MessageType.ItemName)
					{
						gameManager.AddPickupMessage(iID.message);
					}

					Pickup();
				}
				else if (inventory.GetItemAmount(item.ID) >= item.MaxItemCount)
				{
					gameManager.ShowHint("You cannot carry more " + item.Title);
				}
			}
			else
			{
				gameManager.ShowHint("No Inventory Space!");
			}
			
			//MobileInteractManager.Instance.RemoveInteracObject(InteracObj);

		}
		else
		{
			Pickup();
		}
	}

	void Pickup()
	{
		gameManager.HideSprites(spriteType.Interact);
		RaycastObject.SendMessage("UseObject", SendMessageOptions.DontRequireReceiver);
	}
	
	void OnDrawGizmosSelected()
	{
		if (!enableGizmos) return;
		Gizmos.color = Color.green;
        
		
        
		Gizmos.DrawRay(laserPointer.transform.position, laserPointer.transform.forward * 10);
	}


	public bool RayToLayerInteract()
	{
		if (!isMoveTrigger && !useTexture)
			return false;
		return true;
	}
}
