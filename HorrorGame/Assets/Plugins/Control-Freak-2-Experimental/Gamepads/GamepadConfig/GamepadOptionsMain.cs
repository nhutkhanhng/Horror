using UnityEngine;
using System.Collections;
using ControlFreak2;


namespace ControlFreak2.GamepadOptionsUI
{
public class GamepadOptionsMain : GameState 
	{

	public GamepadListScreen
		gamepadListScreen;
	public GamepadMappingScreen
		gamepadMappingScreen;


void Start() { this.OnStartState(null); } void Update() { this.OnUpdateState(); }


	// ------------------
	protected override void OnStartState (GameState parentState)
		{
		base.OnStartState (parentState);

		this.gamepadMappingScreen.gameObject.SetActive(false);
		this.gamepadListScreen.gameObject.SetActive(false);
	
		this.EnterGamepadListScreen();
		}

	
	// ------------------
	protected override void OnExitState ()
		{
		base.OnExitState ();
		}



	// -------------------
	public void EnterMappingScreen(GamepadManager.Gamepad gamepad)
		{
		if (gamepad == null)
			return;

		this.gamepadMappingScreen.StartMappingScreen(gamepad, this);
		}


	// -------------------------
	public void EnterGamepadListScreen()
		{
		this.StartSubState(this.gamepadListScreen);
		}

	
	}
}
