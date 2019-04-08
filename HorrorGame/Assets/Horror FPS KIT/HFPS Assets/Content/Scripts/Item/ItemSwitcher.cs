/* ItemSwitcher.cs by ThunderWire Games - Script only for Switching Items */
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using EasyInputVR.Misc;

public class ItemSwitcher : MonoBehaviour
{

    static ItemSwitcher instance;

    public static ItemSwitcher Instance
    {
        get { return instance; }
    }

    public List<GameObject> ItemList = new List<GameObject>();
    public int currentItem = 0;
    public bool selectCurrItem;

    [Header("Wall Detecting")]
    public bool detectWall;
    public LayerMask HitMask;
    public float wallHitRange;

    public Animation WallDetectAnim;
    public string HideAnim;
    public string ShowAnim;

    private bool hit;
    private bool ladder;
    private bool itemsDisabled;
    private bool switchItem;

	private int newItem = 0;

	public KeyCode PutBack;
	
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentItem = -1;
        if (selectCurrItem)
        {
            if(currentItem!= -1)
                selectItem(currentItem);
        }
    }

	public void selectItem(int id)
	{
		newItem = id;
		if (!CheckActiveItem()) {
			SelectItem ();
		} else {
			StartCoroutine (SwitchItem ());
		}
	}

	public void DeselectItems(bool isReset = false)
	{
	    if (currentItem != -1)
	    {
	        ItemList[currentItem].SendMessage("Deselect", SendMessageOptions.DontRequireReceiver);
	        if(!isReset)
	            currentItem = -1;
	    }
	}

	bool CheckActiveItem()
	{
		for (int i = 0; i < ItemList.Count; i++) {
			ActiveState ACState = ItemList [i].GetComponent<ActiveState> ();
			if (ACState.activeState())
				return true;
		}
		return false;
	}

	IEnumerator SwitchItem()
	{
        switchItem = true;

	    if (currentItem != -1)
	    {
	        DeselectItems(true);
	        yield return new WaitUntil(() => ItemList[currentItem].GetComponent<ActiveState>().activeState() == false);
	    }
	    ItemList [newItem].SendMessage ("Select", SendMessageOptions.DontRequireReceiver);
		currentItem = newItem;

	    switch (currentItem)
	    {
		    case 0:
			    OnOffUIPanel.Instance.ShowFlashBtn(true);
			    HFPS_GameManager.Instance.BatteryUI.SetActive(true);
			    HFPS_GameManager.Instance.GunUI.SetActive(false);
			    InteractManager.Instance.laserPointer.GetComponent<LineRenderer>().widthMultiplier = 0;
			    break;
		    case 1:
			    OnOffUIPanel.Instance.HideAllWeaponBtn(true);
			    HFPS_GameManager.Instance.BatteryUI.SetActive(false);
			    HFPS_GameManager.Instance.GunUI.SetActive(false);
			    InteractManager.Instance.laserPointer.GetComponent<LineRenderer>().widthMultiplier = 0;
			    break;
		    case 2:
			    OnOffUIPanel.Instance.ShowGlockBtn(true);
			    HFPS_GameManager.Instance.BatteryUI.SetActive(false);
			    HFPS_GameManager.Instance.GunUI.SetActive(true);
			    InteractManager.Instance.laserPointer.GetComponent<LineRenderer>().widthMultiplier = 0;
			    break;
	    }

	    switchItem = false;
    }

	void SelectItem()
	{
        switchItem = true;
        ItemList [newItem].SendMessage ("Select", SendMessageOptions.DontRequireReceiver);
		currentItem = newItem;
	    switch (currentItem)
	    {
	        case 0:
	            OnOffUIPanel.Instance.ShowFlashBtn(true);
		        HFPS_GameManager.Instance.BatteryUI.SetActive(true);
		        HFPS_GameManager.Instance.GunUI.SetActive(false);
		        InteractManager.Instance.laserPointer.GetComponent<LineRenderer>().widthMultiplier = 0;

	            break;
	        case 1:
		        OnOffUIPanel.Instance.HideAllWeaponBtn(true);
		        HFPS_GameManager.Instance.BatteryUI.SetActive(false);
		        HFPS_GameManager.Instance.GunUI.SetActive(false);
		        InteractManager.Instance.laserPointer.GetComponent<LineRenderer>().widthMultiplier = 0;

	            break;
	        case 2:
		        OnOffUIPanel.Instance.ShowGlockBtn(true);
		        HFPS_GameManager.Instance.BatteryUI.SetActive(false);
		        HFPS_GameManager.Instance.GunUI.SetActive(true);
		        InteractManager.Instance.laserPointer.GetComponent<LineRenderer>().widthMultiplier = 0;

	            break;
	    }
        switchItem = false;
    }

    void Update()
    {
        if (WallDetectAnim && detectWall && !ladder && currentItem != -1)
        {
            if (WallHit())
            {
                if (!hit)
                {
                    WallDetectAnim.Play(HideAnim);
                    ItemList[currentItem].SendMessage("WallHit", true, SendMessageOptions.DontRequireReceiver);
                    hit = true;
                }
            }
            else
            {
                if (hit)
                {
                    WallDetectAnim.Play(ShowAnim);
                    ItemList[currentItem].SendMessage("WallHit", false, SendMessageOptions.DontRequireReceiver);
                    hit = false;
                }
            }
        }

        if (OculusGoInteractManager.Instance.BackLongClickInput() && currentItem != -1)
        {
	        OnOffUIPanel.Instance.HideAllWeaponBtn(true);
	        HFPS_GameManager.Instance.BatteryUI.SetActive(false);
	        HFPS_GameManager.Instance.GunUI.SetActive(false);
	        InteractManager.Instance.laserPointer.GetComponent<LineRenderer>().widthMultiplier = 1;
            DeselectItems();
        }
        
    }

    void LateUpdate()
    {
        /*if (!CheckActiveItem() && !switchItem)
        {
            Debug.LogError("!");
            currentItem = -1;
        }*/
        
        if(currentItem == -1)
            OnOffUIPanel.Instance.ShowPutBack(false);
        else OnOffUIPanel.Instance.ShowPutBack(true);
    }

    bool GetItemsActive()
    {
        bool response = true;
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].GetComponent<ActiveState>().activeState())
            {
                response = false;
                break;
            }
        }
        return response;
    }

    public void SetItemSwitcher(int switchID)
    {
        ItemList[switchID].SendMessage("LoaderSetItemEnabled", SendMessageOptions.DontRequireReceiver);
        currentItem = switchID;
    }

    public void Ladder(bool onLadder)
    {
        if (currentItem != -1)
        {
            if (onLadder && !ladder)
            {
                Debug.Log("On Ladder");
                WallDetectAnim.Play(HideAnim);
                ItemList[currentItem].SendMessage("WallHit", true, SendMessageOptions.DontRequireReceiver);
                ladder = true;
            }
            if (!onLadder && ladder)
            {
                Debug.Log("On Ladder False");
                WallDetectAnim.Play(ShowAnim);
                ItemList[currentItem].SendMessage("WallHit", false, SendMessageOptions.DontRequireReceiver);
                ladder = false;
            }
        }
    }

    bool WallHit()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, wallHitRange, HitMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (detectWall)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward * wallHitRange));
        }
    }
}