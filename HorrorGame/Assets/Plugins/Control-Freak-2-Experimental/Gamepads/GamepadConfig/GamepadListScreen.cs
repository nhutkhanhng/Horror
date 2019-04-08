using UnityEngine;
using System.Collections.Generic;
using ControlFreak2;

namespace ControlFreak2.GamepadOptionsUI
{
public class GamepadListScreen : GameState
	{
	public GamepadListElement
		gamepadElemTemplate;
	public RectTransform 
		gamepadElemListContent;

	private List<GamepadListElement> 
		gamepadElemList;	


	// --------------------
	public GamepadListScreen() : base()
		{
		this.gamepadElemList = new List<GamepadListElement>(16);
		}


	// ---------------------
	protected override void OnStartState (GameState parentState)
		{
		base.OnStartState (parentState);

		this.gameObject.SetActive(true);

		GamepadManager.onChange += this.RebuildGamepadList;
		}


	// --------------------
	protected override void OnExitState ()
		{
		base.OnExitState ();

		this.gameObject.SetActive(false);

		GamepadManager.onChange -= this.RebuildGamepadList;
		}


	// --------------------
	private void RebuildGamepadList()
		{
		this.gamepadElemList.RemoveAll(x => (x == null));

		for (int i = 0; i < this.gamepadElemList.Count; ++i)
			{
			this.gamepadElemList[i].RefreshGamepadState();
			}

		// Add new gamepads...

		GamepadManager gm = GamepadManager.activeManager;
		if (gm != null)
			{
			for (int i = 0; i < gm.GetConnectedGamepadCount(); ++i)
				{
				GamepadManager.Gamepad g = gm.GetConnectedGamepad(i);
				if ((g != null) && (this.FindElementByGamepad(g) == null))
					this.UpdateOrCreateGamepadElement(g);
				}
			}

		// Sort the list...
		
		GamepadListElement.SortElementList(this.gamepadElemList);

		}

	// ------------------
	private GamepadListElement FindElementByGamepad(GamepadManager.Gamepad gamepad)
		{
		return (this.gamepadElemList.Find(x => ((x != null) && (x.GetGamepad() == gamepad)))); 
		}

	// ----------------
	private GamepadListElement UpdateOrCreateGamepadElement(GamepadManager.Gamepad gamepad)
		{
		if (gamepad == null)
			return null;

		GamepadListElement elem = this.FindElementByGamepad(gamepad);	
		if ((elem == null) && (this.gamepadElemTemplate != null))
			{
			elem = (GamepadListElement)GamepadListElement.Instantiate(this.gamepadElemTemplate);
			if (elem != null)
				this.gamepadElemList.Add(elem);
			}

		if (elem != null)
			{
			elem.transform.SetParent(this.gamepadElemListContent, false);
			elem.SetGamepad(gamepad);
			}

		return elem;
		}
	}
}
