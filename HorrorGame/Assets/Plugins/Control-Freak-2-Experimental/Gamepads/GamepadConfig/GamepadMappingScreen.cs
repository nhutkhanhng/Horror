using UnityEngine;
using System.Collections.Generic;
using ControlFreak2;

namespace ControlFreak2.GamepadOptionsUI
{
public class GamepadMappingScreen : GamepadMappingScreenBase
	{

	//public GamepadConfigFrontendIntroScreen
	//	introScreen;
	public GamepadMappingSuccessScreen
		successScreen;
	public GamepadMappingFailureScreen
		failureScreen;
	public GamepadMappingQueryScreen	
		queryScreen;


	public GameObject
//		//introScreen,
//		leftStickInfoScreen,
//		rightStickInfoScreen,
//		triggersInfoScreen,
//		startAndSelectInfoScreen,
		
		
		gamepadStateGUI;

	public List<UnityEngine.UI.Toggle>
		positiveAxisToggleList,
		negativeAxisToggleList,
		keyToggleList;
		


	// ----------------------
	public GamepadMappingScreen() : base()
		{
		this.keyToggleList 			= new List<UnityEngine.UI.Toggle>(20);
		this.positiveAxisToggleList = new List<UnityEngine.UI.Toggle>(10);
		this.negativeAxisToggleList = new List<UnityEngine.UI.Toggle>(10);
		}


	
	// ----------------------
	public void ExitMappingScreen()
		{
		this.gamepadOptionsMain.EnterGamepadListScreen();
		}


	// -------------------
	protected override void OnStartState (GameState parentState)
		{
		base.OnStartState (parentState);
		this.gameObject.SetActive(true);

		this.successScreen.gameObject.SetActive(false);	
		this.failureScreen.gameObject.SetActive(false);	
		this.queryScreen.gameObject.SetActive(false);	

		}
			
	// -------------------
	protected override void OnExitState ()
		{
		base.OnExitState ();
		this.gameObject.SetActive(false);
		}

	// ---------------------
	protected override void OnUpdateState ()
		{
		if (!this.IsRunning())
			return;

		this.SyncGamepadState();
		
		base.OnUpdateState ();
		}


//	// -------------------
//	protected override void OnStartConfiguration ()
//		{
//		this.ShowGamepadState(true);
//
//		base.OnStartConfiguration ();
//		}


	// -------------------	
	protected override void OnConfigurationSuccess ()
		{	
		this.ShowGamepadState(false);
		this.StartSubState(this.successScreen);
		//base.OnConfigurationSuccess ();
		}


	// -----------------
	protected override void OnConfigurationFailure ()
		{
		this.ShowGamepadState(false);
		this.StartSubState(this.failureScreen);
		//base.OnConfigurationFailure ();
		}


	// ----------------
	override protected void OnEnterElementQuery (GamepadElementId elemId)
		{
		this.ShowGamepadState(true);


		base.OnEnterElementQuery (elemId);
		this.queryScreen.EnterQuery(this, elemId);
		}


	// -----------------
	override public void OnQuerySkip ()
		{
		base.OnQuerySkip ();
		}
			
	
	// -----------------
	override public void OnQuerySuccess ()
		{
		base.OnQuerySuccess ();		
		}

//	// -----------------
//	override public void OnChangeQueryPhase(QueryPhase phaseId)
//		{
//		base.OnChangeQueryPhase (phaseId);
//
////		switch (this.GetQueryPhase())
////			{
////			case QueryPhase.WAITING_FOR_CONFIRMATION :
////			}
//		}

	// ----------------
	protected void ShowGamepadState(bool show)
		{
		this.gamepadStateGUI.SetActive(show);
		}


	// ------------------
	protected void SyncGamepadState()
		{
		if (!this.gamepadStateGUI.activeSelf)
			return;

		GamepadMappingScreenBase.GamepadDigitalState 
			digiState = base.GetCurGamepadState();

		for (int i = 0; i < GamepadManager.MAX_INTERNAL_AXES; ++i)
			{
			int axisState = digiState.GetAxisState(i);

			if ((this.positiveAxisToggleList.Count > i) && (this.positiveAxisToggleList[i] != null))
				this.positiveAxisToggleList[i].isOn = (axisState > 0);

			if ((this.negativeAxisToggleList.Count > i) && (this.negativeAxisToggleList[i] != null))
				this.negativeAxisToggleList[i].isOn = (axisState < 0);
			}


		for (int i = 0; i < GamepadManager.MAX_INTERNAL_KEYS; ++i)
			{
			if ((this.keyToggleList.Count > i) && (this.keyToggleList[i] != null))
				this.keyToggleList[i].isOn = digiState.GetKeyState(i);
			}
	
		}
		
	}
}
