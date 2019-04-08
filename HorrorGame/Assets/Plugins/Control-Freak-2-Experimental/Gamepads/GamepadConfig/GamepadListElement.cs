using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ControlFreak2;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace ControlFreak2.GamepadOptionsUI
{

public class GamepadListElement : UnityEngine.UI.Selectable, ISubmitHandler
	{	
	public GamepadOptionsMain
		gamepadOptionsMain;
 
	private GamepadManager.Gamepad
		gamepad;	
	private List<GamepadProfile> 
		profileList;
	private int 
		curProfileId = 0;

	public UnityEngine.UI.Button
		buttonLeft,
		buttonRight,
		buttonEdit;

	public GameObject
		iconDisconnected,
		iconConnected,
		iconActivated;

	public UnityEngine.UI.Text
		titleText;
		
	public delegate void OnProfileSwitchCallback(int dir);

	public OnProfileSwitchCallback
		onProfileSwitch;		



	// ----------------
	public GamepadListElement() : base()
		{
		this.profileList = new List<GamepadProfile>(4);
		}


	// ----------------
	public GamepadManager.Gamepad GetGamepad()
		{ return this.gamepad; }


	// ---------------------
	public void RefreshGamepadState()
		{ this.SetGamepad(this.gamepad); }

	// -----------------
	public void SetGamepad(GamepadManager.Gamepad gamepad)
		{
		this.gamepad = gamepad;
		
		if (gamepad == null)
			{
			this.gameObject.SetActive(false);
			return;
			}
		
		this.gameObject.SetActive(true);

		this.profileList.Clear();
		
		if (GamepadManager.activeManager != null)	
			GamepadManager.activeManager.GetCustomProfileBank().GetCompatibleProfiles(this.gamepad.GetInternalJoyName(), this.profileList);
	
		this.curProfileId = this.profileList.IndexOf(this.gamepad.GetProfile());
		if (this.curProfileId < 0)
			this.curProfileId = 0;


		this.buttonLeft	.gameObject.SetActive(this.profileList.Count > 0);
		this.buttonRight.gameObject.SetActive(this.profileList.Count > 0);
		
		if (this.iconActivated != null) 
			this.iconActivated.SetActive(this.gamepad.IsActivated());
		if (this.iconConnected != null) 
			this.iconConnected.SetActive(!this.gamepad.IsActivated() && this.gamepad.IsConnected());
		if (this.iconDisconnected != null) 
			this.iconDisconnected.SetActive(!this.gamepad.IsConnected());
			

		if (this.titleText != null)
			{
			if (this.gamepad.GetProfile() != null)
				this.titleText.text = this.gamepad.GetProfileName();
			else
				this.titleText.text = this.gamepad.GetInternalJoyName();
			}

		}




	// ----------------
	static public int Compare(GamepadListElement a, GamepadListElement b)
		{
		if ((a == null) || (b == null))
			return 0;
		
		if ((a.gamepad == null) || (b.gamepad == null))
			return ((a.gamepad == null) ? ((b.gamepad == null) ? 0 : 1) : (b.gamepad == null) ? -1  : 0); 

		return 0;
		}


	// --------------------
	static public void SortElementList(List<GamepadListElement> list)
		{
		list.Sort(GamepadListElement.Compare);

		for (int i = 0; i < list.Count; ++i)
			{
			if (list[i] != null)
				list[i].transform.SetSiblingIndex(i);
			}
		} 


		
	// -----------------
	override public void OnSelect(BaseEventData data)
		{
Debug.Log("FR[" + Time.frameCount + "} Select[" + this.name + "]");
		base.OnSelect(data);
		}


	// -----------------
	override public void OnDeselect(BaseEventData data)
		{
Debug.Log("FR[" + Time.frameCount + "} DeSelect[" + this.name + "] Sel:" + ((data.selectedObject != null) ? data.selectedObject.name : "NULL") );
		base.OnDeselect(data);
		}

	// ----------------
	override public void OnMove(AxisEventData data)
		{
Debug.Log("FR[" + Time.frameCount + "} Move[" + this.name + "] dir:" + data.moveDir);
		if ((data.moveDir == MoveDirection.Left) || (data.moveDir == MoveDirection.Right))
			{
			this.SwitchProfile((data.moveDir == MoveDirection.Left) ? -1 : 1);
			data.Use();
			} 
		else
			{
			base.OnMove(data);
			}
		}

	// --------------
	void ISubmitHandler.OnSubmit(BaseEventData data)
		{
Debug.Log("OnSubmit : " + this.name);
		if (this.gamepadOptionsMain != null)
			{
			data.Use();
			this.OnEditPressed();
			}
		}

	

	// ------------------
	private void OnLeftPressed()	{ this.SwitchProfile(-1); }
	private void OnRightPressed()	{ this.SwitchProfile(1); }
	private void OnEditPressed()	{ this.gamepadOptionsMain.EnterMappingScreen(this.gamepad); }

		
	// ------------------
	public void SwitchProfile(int dir)
		{


		if (this.onProfileSwitch != null)
			this.onProfileSwitch(dir);
		}



	// --------------------
	override protected void OnEnable()
		{	
		base.OnEnable();

		if (CFUtils.editorStopped)
			return;

		this.buttonLeft.onClick.AddListener(this.OnLeftPressed);
		this.buttonRight.onClick.AddListener(this.OnRightPressed);
		this.buttonEdit.onClick.AddListener(this.OnEditPressed);

		
		}
	
	// --------------------
	override protected void OnDisable()
		{
		if (CFUtils.editorStopped)
			return;

		this.buttonLeft.onClick.RemoveAllListeners();
		this.buttonRight.onClick.RemoveAllListeners();
		this.buttonEdit.onClick.RemoveAllListeners();

		base.OnDisable();
		}

				
	
	}
}
