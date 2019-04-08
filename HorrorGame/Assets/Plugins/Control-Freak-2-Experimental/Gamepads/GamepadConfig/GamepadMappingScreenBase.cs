using UnityEngine;
using System.Collections.Generic;
using ControlFreak2;
using System.Runtime.InteropServices;
using System;


namespace ControlFreak2.GamepadOptionsUI
{
abstract public class GamepadMappingScreenBase : GameState
	{
	public GamepadOptionsMain
		gamepadOptionsMain;

	// --------------------
	public enum GamepadElementId
		{
		LEFT_ANALOG_RIGHT,
		LEFT_ANALOG_LEFT,
		LEFT_ANALOG_UP,
		LEFT_ANALOG_DOWN,

		FACE_BOTTOM,
		FACE_RIGHT,
		FACE_LEFT,
		FACE_TOP,

		RIGHT_ANALOG_RIGHT,
		RIGHT_ANALOG_LEFT,
		RIGHT_ANALOG_UP,
		RIGHT_ANALOG_DOWN,
		
		DPAD_RIGHT,
		DPAD_LEFT,
		DPAD_UP,
		DPAD_DOWN,

		START_BUTTON,
		SELECT_BUTTON,
		
		L1,
		R1,
		L2,
		R2,
		L3,
		R3			
		}
		
	public const GamepadElementId
		GamepadElementIdFirst			= GamepadElementId.LEFT_ANALOG_RIGHT,
		GamepadElementIdLast			= GamepadElementId.R3,
		GamepadElementIdDpadLast		= GamepadElementId.DPAD_DOWN,
		GamepadElementIdRightAnalogLast	= GamepadElementId.RIGHT_ANALOG_DOWN;



	public const int 
		GamepadElementIdCount = (int)GamepadElementIdLast + 1;
		
	//public GamepadConfigQueryScreenBase
	//	queryScreen;



	protected GamepadManager.Gamepad 
		gamepad;

	protected List<GamepadElementConfig>
		elemConfigList;
		
	protected GamepadDigitalState
		combinedUsage,
		digitalStateCur,
		digitalStatePrev;
		

	protected GamepadElementId
		curElementId;

	//protected int 
	//	queryAttemptNum;
		

	//public delegate void GamepadElementIdCallback(GamepadElementId id);
		
	//public GamepadElementIdCallback
	//	onEnterElementQuery;
		

	// --------------------
	public GamepadMappingScreenBase() : base()
		{
		this.digitalStateCur	= new GamepadDigitalState();
		this.digitalStatePrev	= new GamepadDigitalState();
		this.combinedUsage		= new GamepadDigitalState();

		this.elemConfigList = new List<GamepadElementConfig>(GamepadElementIdCount);
		for (int i = 0; i < GamepadElementIdCount; ++i)
			this.elemConfigList.Add(new GamepadElementConfig(this));
		}
		
		

	// -------------------
	public GamepadManager.Gamepad GetGamepad()
		{ return this.gamepad; }

	public GamepadDigitalState GetCurGamepadState()		{ return this.digitalStateCur; }
	public GamepadDigitalState GetPrevGamepadState()	{ return this.digitalStatePrev; }
	public GamepadDigitalState GetUsedGamepadState()	{ return this.combinedUsage; }

	public bool IsCurGamepadStateUsable()
		{ return (!this.digitalStateCur.IsNeutral() && !this.digitalStateCur.IsFullyOverlappedBy(this.combinedUsage)); }

	// --------------------
	public GamepadDigitalState CollectCombinedUsage(GamepadElementConfig elemToExclude = null)
		{
		GamepadDigitalState s = new GamepadDigitalState();
			
		for (int i = 0; i < this.elemConfigList.Count; ++i)
			{
			if (this.elemConfigList[i] == elemToExclude)
				continue;

			s.Add(this.elemConfigList[i].GetUsedKeysAndAxes());
			}

		return s;
		}


	// -----------------
	static public bool IsElemEssential(GamepadElementId elemId)
		{
		return (
			(elemId == GamepadElementId.LEFT_ANALOG_DOWN) ||
			(elemId == GamepadElementId.LEFT_ANALOG_RIGHT) ||
			(elemId == GamepadElementId.LEFT_ANALOG_LEFT) ||
			(elemId == GamepadElementId.LEFT_ANALOG_UP) ||
			(elemId == GamepadElementId.FACE_BOTTOM) ||
			(elemId == GamepadElementId.FACE_RIGHT));
		}


	// ---------------------
	public void StartMappingScreen(GamepadManager.Gamepad gamepad, GameState parentState)
		{
		this.gamepad = gamepad;
		parentState.StartSubState(this);
		}
		
/*

	// ----------------------
	public void DrawGUI()
		{
		if (this.gamepad == null)	
			return;

		GUILayout.Box("Config for [" + this.gamepad.GetInternalJoyName() + "]\n" +
			"Getting input for :" + this.curElementId + "\n");

		switch (this.queryPhase)
			{
			case QueryPhase.WAITING_FOR_NEUTRAL :
				GUILayout.Box("Put controller in neutral state! " + this.queryElapsed + "/" + this.waitForNeutralTime);
				break;
	
			case QueryPhase.WAITING_FOR_INPUT :	
				GUILayout.Box("Press specified button or tilt the joystick in specified direction! " + this.queryElapsed + "/" + this.waitForInputTime);
				break;

			case QueryPhase.WAITING_FOR_CONFIRMATION :
				GUILayout.Box("Hold that config for a bit to confirm! " + this.queryElapsed + "/" + this.waitForConfirmationTime);

				GUI.contentColor = Color.white;
				break;

			case QueryPhase.DONE :
				GUILayout.Box("Good job!");
				break;
			}

		if ((this.queryPhase == QueryPhase.WAITING_FOR_NEUTRAL) || (this.queryPhase == QueryPhase.WAITING_FOR_CONFIRMATION))
			{
			for (int i = 0; i < GamepadManager.MAX_INTERNAL_AXES; ++i)
				{
				if (this.digitalStateCur.ContainsAxis(i, true))
					{
					GUI.contentColor = this.combinedUsage.ContainsAxis(i, true) ? Color.red : Color.white;
					GUILayout.Box("Axis " + i + " PLUS");
					}
				if (this.digitalStateCur.ContainsAxis(i, false))
					{
					GUI.contentColor = this.combinedUsage.ContainsAxis(i, false) ? Color.red : Color.white;
					GUILayout.Box("Axis " + i + " MINUS");
					}
				}
			for (int i = 0; i < GamepadManager.MAX_INTERNAL_KEYS; ++i)
				{
				GUI.contentColor = this.combinedUsage.ContainsKey(i) ? Color.red : Color.white;
				if (this.digitalStateCur.ContainsKey(i))
					GUILayout.Box("Key " + i);
				}

			}
		}

*/

	// --------------------
	protected override void OnStartState (GameState parentState)
		{
		base.OnStartState(parentState);
			
		this.gamepad.Block(true);
			
		//this.gameObject.SetActive(true);
		
		//this.queryActive = false;
		
		//this.OnStartConfiguration();

		this.StartMappingQueryFromTheFirstElem();
		}



	// --------------------
	protected void StartMappingQueryFromTheFirstElem()
		{
		this.ClearConfiguration();
		this.EnterElementQuery(GamepadElementIdFirst);
		}


	// --------------------
	protected void ClearConfiguration()
		{
		for (int i = 0; i < this.elemConfigList.Count; ++i)
			{
			this.elemConfigList[i].Clear();
			}

		this.combinedUsage.Clear();
		}


	// ---------------------
	protected override void OnExitState ()
		{
		base.OnExitState ();

		this.gamepad.Block(false);	

		//this.gameObject.SetActive(false);


		}


	// ------------------
	protected override void OnUpdateState ()
		{
		this.digitalStatePrev.CopyFrom(this.digitalStateCur);	
		this.digitalStateCur.GetFromGamepad(this.gamepad);


		//this.UpdateQuery();

		base.OnUpdateState ();
		}

	
	// -----------------
	virtual protected void OnEnterElementQuery(GamepadElementId elemId) 
		{
		}




	// --------------------
	public bool ApplyGamepadStateToElement(GamepadElementId elemId, GamepadDigitalState state)
		{
		this.elemConfigList[(int)elemId].FromDigitalState(state);
		this.combinedUsage = this.CollectCombinedUsage();

		return !this.elemConfigList[(int)elemId].IsEmpty();	
		}

	// -------------------
	protected void EnterElementQuery(GamepadElementId elemId)
		{
		this.curElementId = elemId;
		//this.delayElapsed = 0;
		//this.queryElapsed = 0;

		//this.queryActive = true;
		
		this.digitalStateCur.GetFromGamepad(this.gamepad);
		this.digitalStatePrev.CopyFrom(this.digitalStateCur);
	
		this.combinedUsage = this.CollectCombinedUsage(null);
	
		this.OnEnterElementQuery(elemId);


		//this.ChangeQueryPhase(this.digitalStateCur.IsNeutral() ? QueryPhase.WAITING_FOR_NEUTRAL : QueryPhase.WAITING_FOR_INPUT);	

		//if (this.onEnterElementQuery != null)
		//	this.onEnterElementQuery(elemId);
		}


	// -----------------------
	protected void EnterNextElementQuery(GamepadElementId prevElemId)
		{
		if (prevElemId == GamepadElementIdLast)
			this.FinishConfiguration();
		else
			this.EnterElementQuery(prevElemId + 1);
		}
		
		



	// --------------------
	public GamepadElementId GetQueryTargetElementId()
		{ return this.curElementId; }




	
	//virtual void OnQueryCancel()			

	// ----------------
	virtual public void OnQuerySuccess()
		{
		this.EnterNextElementQuery(this.curElementId);
		}

	// ------------------
	virtual public void OnQuerySkip()				
		{


		switch (this.curElementId)
			{
			// Essential elements...

			case GamepadElementId.LEFT_ANALOG_DOWN :
			case GamepadElementId.LEFT_ANALOG_LEFT :
			case GamepadElementId.LEFT_ANALOG_RIGHT :
			case GamepadElementId.LEFT_ANALOG_UP :
			
			case GamepadElementId.FACE_BOTTOM :
			case GamepadElementId.FACE_RIGHT :

				this.CancelConfiguration();
				break;

			case GamepadElementId.DPAD_DOWN :
			case GamepadElementId.DPAD_LEFT :	
			case GamepadElementId.DPAD_RIGHT :
			case GamepadElementId.DPAD_UP :	
				this.elemConfigList[(int)GamepadElementId.DPAD_DOWN].Clear();
				this.elemConfigList[(int)GamepadElementId.DPAD_LEFT].Clear();
				this.elemConfigList[(int)GamepadElementId.DPAD_RIGHT].Clear();
				this.elemConfigList[(int)GamepadElementId.DPAD_UP].Clear();
				this.EnterNextElementQuery(GamepadElementIdDpadLast);
				break;

			case GamepadElementId.RIGHT_ANALOG_DOWN :
			case GamepadElementId.RIGHT_ANALOG_LEFT :	
			case GamepadElementId.RIGHT_ANALOG_RIGHT :
			case GamepadElementId.RIGHT_ANALOG_UP :	
				this.elemConfigList[(int)GamepadElementId.RIGHT_ANALOG_DOWN].Clear();
				this.elemConfigList[(int)GamepadElementId.RIGHT_ANALOG_LEFT].Clear();
				this.elemConfigList[(int)GamepadElementId.RIGHT_ANALOG_RIGHT].Clear();
				this.elemConfigList[(int)GamepadElementId.RIGHT_ANALOG_UP].Clear();
				this.EnterNextElementQuery(GamepadElementIdRightAnalogLast);
				break;
				
			default :
				this.EnterNextElementQuery(this.curElementId);
				break;
			
			}
		}


	// --------------------
	public void CancelConfiguration()
		{
		this.OnConfigurationFailure();
		}


	// ---------------------
	virtual protected void OnConfigurationFailure()
		{
		this.gamepadOptionsMain.EnterGamepadListScreen();
		}



	// ----------------------
	protected void FinishConfiguration()
		{
		this.PostprocessInputSources();

		if (!this.CanBeUsedAsProfile())
			this.OnConfigurationFailure();
		else
			this.OnConfigurationSuccess();

		}


		// --------------------
	virtual protected void OnConfigurationSuccess()	
		{
		
for (int i = 0; i < this.elemConfigList.Count; ++i)
	{
	GamepadElementConfig elemConfig = this.elemConfigList[i];
	InputSource s = elemConfig.bestSource;

	Debug.Log("Element[" + (GamepadElementId)i + "] best source: "  + (s.IsEmpty() ? "EMPTY" : s.IsAxis() ? ("Axis:"  + s.axisId + " " + (s.axisPositive ? "+" : "-")) :
		("Key: " + s.keyId)));
	}
		// Create the profile, save it and apply to source gamepad!
		
		// TODO : apply to profile!

		this.gamepadOptionsMain.EnterGamepadListScreen();
		}
		

	
	
		
	// ---------------------
	protected bool CanBeUsedAsProfile()
		{
		if (this.elemConfigList[(int)GamepadElementId.LEFT_ANALOG_DOWN].bestSource.IsEmpty() ||
			this.elemConfigList[(int)GamepadElementId.LEFT_ANALOG_LEFT].bestSource.IsEmpty() ||
			this.elemConfigList[(int)GamepadElementId.LEFT_ANALOG_RIGHT].bestSource.IsEmpty() ||
			this.elemConfigList[(int)GamepadElementId.LEFT_ANALOG_UP].bestSource.IsEmpty() ||

			this.elemConfigList[(int)GamepadElementId.FACE_BOTTOM].bestSource.IsEmpty() ||
			this.elemConfigList[(int)GamepadElementId.FACE_RIGHT].bestSource.IsEmpty())
			return false;

		return true;
		}


	// ---------------------
	public GamepadProfile CreateProfile()
		{
		GamepadProfile gp = new GamepadProfile();

		this.ApplyToGamepadProfile(gp);
		gp.name = this.gamepad.GetInternalJoyName();
			
		return gp;
		}

	// -------------------
	public void ApplyToGamepadProfile(GamepadProfile profile)
		{	
		//this.PostprocessInputSources();
		//.SortInputSources();

		profile.unityVerTo 		= profile.unityVerFrom = Application.unityVersion;
		//profile.platformFlags	= GamepadProfile.RuntimePlatformToFlag(Application.platform);
		profile.joystickIdentifier = this.gamepad.GetInternalJoyName();
		//profile.deviceType 		= "";
		
		GamepadElementConfig.ToKeySource(profile.keyFaceD,	this.elemConfigList[(int)GamepadElementId.FACE_BOTTOM]);
		GamepadElementConfig.ToKeySource(profile.keyFaceU,	this.elemConfigList[(int)GamepadElementId.FACE_TOP]);
		GamepadElementConfig.ToKeySource(profile.keyFaceL,	this.elemConfigList[(int)GamepadElementId.FACE_LEFT]);
		GamepadElementConfig.ToKeySource(profile.keyFaceR,	this.elemConfigList[(int)GamepadElementId.FACE_RIGHT]);

		GamepadElementConfig.ToKeySource(profile.keyStart,	this.elemConfigList[(int)GamepadElementId.START_BUTTON]);
		GamepadElementConfig.ToKeySource(profile.keySelect,	this.elemConfigList[(int)GamepadElementId.SELECT_BUTTON]);

		GamepadElementConfig.ToKeySource(profile.keyL1,		this.elemConfigList[(int)GamepadElementId.L1]);
		GamepadElementConfig.ToKeySource(profile.keyR1,		this.elemConfigList[(int)GamepadElementId.R1]);
		GamepadElementConfig.ToKeySource(profile.keyL2,		this.elemConfigList[(int)GamepadElementId.L2]);
		GamepadElementConfig.ToKeySource(profile.keyR2,		this.elemConfigList[(int)GamepadElementId.R2]);
		GamepadElementConfig.ToKeySource(profile.keyL3,		this.elemConfigList[(int)GamepadElementId.L3]);
		GamepadElementConfig.ToKeySource(profile.keyR3,		this.elemConfigList[(int)GamepadElementId.R3]);

		GamepadElementConfig.ToJoySource(profile.leftStick,
			this.elemConfigList[(int)GamepadElementId.LEFT_ANALOG_UP],
			this.elemConfigList[(int)GamepadElementId.LEFT_ANALOG_DOWN],
			this.elemConfigList[(int)GamepadElementId.LEFT_ANALOG_LEFT],
			this.elemConfigList[(int)GamepadElementId.LEFT_ANALOG_RIGHT]);

		GamepadElementConfig.ToJoySource(profile.rightStick,
			this.elemConfigList[(int)GamepadElementId.RIGHT_ANALOG_UP],
			this.elemConfigList[(int)GamepadElementId.RIGHT_ANALOG_DOWN],
			this.elemConfigList[(int)GamepadElementId.RIGHT_ANALOG_LEFT],
			this.elemConfigList[(int)GamepadElementId.RIGHT_ANALOG_RIGHT]);

		GamepadElementConfig.ToJoySource(profile.dpad,
			this.elemConfigList[(int)GamepadElementId.DPAD_UP],
			this.elemConfigList[(int)GamepadElementId.DPAD_DOWN],
			this.elemConfigList[(int)GamepadElementId.DPAD_LEFT],
			this.elemConfigList[(int)GamepadElementId.DPAD_RIGHT]);
		}
		

	
	// ---------------------
	protected void PostprocessInputSources()
		{
		for (int i = 0; i < this.elemConfigList.Count; ++i)
			this.elemConfigList[i].SelectBestSource();

		for (int i = 0; i < this.elemConfigList.Count; ++i)
			this.elemConfigList[i].RemoveSharedSources();
		}

		



	// ------------------
	// Gamepad Digital State
	// ------------------

	public struct GamepadDigitalState
		{
		public int 
			axesPositiveBitset,
			axesNegativeBitset,
			keysBitset;
	
		// --------------------
		public void Clear()
			{
			this.axesPositiveBitset = 0;
			this.axesNegativeBitset = 0;
			this.keysBitset = 0;
			}


		// --------------------
		public void CopyFrom(GamepadDigitalState otherState)
			{
			this.axesNegativeBitset = otherState.axesNegativeBitset;
			this.axesPositiveBitset	= otherState.axesPositiveBitset;
			this.keysBitset			= otherState.keysBitset;
			}
			

		// -------------------
		public void AddKey(int keyId)
			{
			if ((keyId >= 0) && (keyId < GamepadManager.MAX_INTERNAL_KEYS))
				this.keysBitset |= (1 << keyId);
			}
			

		// ------------------
		public void AddAxis(int axisId, bool positiveSign)
			{
			if ((axisId < 0) || (axisId >= GamepadManager.MAX_INTERNAL_AXES))
				return;

			if (positiveSign)
				this.axesPositiveBitset |= (1 << axisId);
			else
				this.axesNegativeBitset |= (1 << axisId);
			}
	
		// ------------------
		public bool ContainsKey(int keyId)
			{
			if ((keyId < 0) || (keyId >= GamepadManager.MAX_INTERNAL_KEYS))
				return false;
			return ((this.keysBitset & (1 << keyId)) != 0);
			}

		// ------------------
		public bool ContainsAxis(int axisId, bool positiveSign)
			{
			if ((axisId < 0) || (axisId >= GamepadManager.MAX_INTERNAL_AXES))
				return false;
			return (((positiveSign ? this.axesPositiveBitset : this.axesNegativeBitset) & (1 << axisId)) != 0);
			}

		// -----------------
		public int GetAxisState(int axisId)
			{
			if ((axisId < 0) || (axisId >= GamepadManager.MAX_INTERNAL_AXES))
				return 0;
			return ((((this.axesPositiveBitset & (1 << axisId)) != 0) ? 1 : 0) + (((this.axesNegativeBitset & (1 << axisId)) != 0) ? -1 : 0));
			}

		// ----------------
		public bool GetKeyState(int keyId)
			{
			if ((keyId < 0) || (keyId >= GamepadManager.MAX_INTERNAL_KEYS))
				return false;
			return ((this.keysBitset & (1 << keyId)) != 0);
			}


		// -------------------
		public void GetFromGamepad(GamepadManager.Gamepad gamepad)
			{
			this.Clear();

			for (int axisi = 0; axisi < GamepadManager.MAX_INTERNAL_AXES; ++axisi)
				{
				int axisDigi = gamepad.GetInternalAxisDigital(axisi);
				if (axisDigi == 0)
					continue;
					
				if (axisDigi < 0)
					this.axesNegativeBitset |= (1 << axisi);
				else
					this.axesPositiveBitset |= (1 << axisi);
				}

			for (int keyi = 0; keyi < GamepadManager.MAX_INTERNAL_KEYS; ++keyi)
				{
				if (gamepad.GetInternalKeyState(keyi))
					this.keysBitset |= (1 << keyi);
				}
			}
		

		// ------------------
		public bool IsNeutral()
			{ return ((this.axesNegativeBitset | this.axesPositiveBitset | this.keysBitset) == 0); }

		// ------------------
		public bool Equals(GamepadDigitalState otherState)
			{
			return (
				(this.axesNegativeBitset	== otherState.axesNegativeBitset) && 
				(this.axesPositiveBitset	== otherState.axesPositiveBitset) &&
				(this.keysBitset			== otherState.keysBitset)); 
			}
	
		// -------------------
		public bool Intersets(GamepadDigitalState otherState)
			{
			return (((this.keysBitset & otherState.keysBitset) != 0) ||
				((this.axesPositiveBitset & otherState.axesPositiveBitset) != 0) ||
				((this.axesNegativeBitset & otherState.axesNegativeBitset) != 0) );
			}

		public bool IsFullyOverlappedBy(GamepadDigitalState otherState)
			{
			if (otherState.IsNeutral())
				return false;

			return (
				((this.keysBitset			& ~otherState.keysBitset) == 0) &&
				((this.axesPositiveBitset	& ~otherState.axesPositiveBitset) == 0) &&
				((this.axesNegativeBitset	& ~otherState.axesNegativeBitset) == 0) );
			}

		// -------------------
		public void Add(GamepadDigitalState otherState)
			{
			this.axesNegativeBitset |= otherState.axesNegativeBitset;
			this.axesPositiveBitset |= otherState.axesPositiveBitset;
			this.keysBitset			|= otherState.keysBitset;
			}

		// -------------------
		public void Subtract(GamepadDigitalState otherState)
			{
			this.axesNegativeBitset &= ~otherState.axesNegativeBitset;
			this.axesPositiveBitset &= ~otherState.axesPositiveBitset;
			this.keysBitset			&= ~otherState.keysBitset;
			}
		}

	// ---------------
	// Gamepad Element Config.
	// ---------------	
	public class GamepadElementConfig
		{
		//public const int 
		//	MAX_SOURCES = 4;

		public GamepadMappingScreenBase
			configScreen;
		
		//public List<InputSource>
		//	sourceList;

		private GamepadDigitalState
			usedKeysAndAxes;
			
		public InputSource
			bestSource;

			//usedKeysBitset,
			//usedPositiveAxesBitset,
			//usedNegativeAxesBitset;

		// ------------------	
		public GamepadElementConfig(GamepadMappingScreenBase screen)
			{
			this.configScreen = screen;
				
			this.bestSource = new InputSource();

			//this.sourceList = new List<InputSource>(MAX_SOURCES);
			//for (int i = 0; i < MAX_SOURCES; ++i)
			//	this.sourceList.Add(new InputSource());
			}
		


		// -----------------------
		public GamepadDigitalState GetUsedKeysAndAxes()	{ return this.usedKeysAndAxes; }		
	




		// -------------------
		public bool IsEmpty()
			{	
			return this.usedKeysAndAxes.IsNeutral();
			}


		// -------------------
		public void Clear()
			{
			//for (int i = 0; i < this.sourceList.Count; ++i)
			//	this.sourceList[i].Clear();

			this.usedKeysAndAxes.Clear();
			}
			


		// --------------------
		public void FromDigitalState(GamepadDigitalState ds)
			{
			this.Clear();
			this.usedKeysAndAxes.CopyFrom(ds);
			this.RemoveSharedSources();
			}


		

		// ----------------------
		public void RemoveSharedSources()
			{
			if (this.IsEmpty())
				return;
	
			this.usedKeysAndAxes.Subtract(this.configScreen.GetUsedGamepadState());
			}


			

		// ------------------------
		public void SelectBestSource()
			{
			this.bestSource.Clear();

			GamepadDigitalState 
				combinedUsage = this.configScreen.CollectCombinedUsage(this);

			this.usedKeysAndAxes.Subtract(combinedUsage);
			
				
			int usedAxes	= this.usedKeysAndAxes.axesNegativeBitset | this.usedKeysAndAxes.axesPositiveBitset;
			int sharedAxes	= combinedUsage.axesNegativeBitset | combinedUsage.axesPositiveBitset;
				
			// Look for axis source, preferably an exclusive axis (to avoid common problem of Analog Triggers sharing the same axis)

			int sharedAxisId = -1;
				
			if (usedAxes != 0)
				{						
				for (int i = 0; i < GamepadManager.MAX_INTERNAL_AXES; ++i)
					{
					if ((usedAxes & (1 << i)) == 0) 
						continue;
						
					if ((sharedAxes & (1 << i)) == 0)	
						{
						this.bestSource.SetAxis(i, ((this.usedKeysAndAxes.axesPositiveBitset & (1 << i)) != 0));
						return;
						}
					else if (sharedAxisId < 0)
						sharedAxisId = i;					 
					}
					
				}

			// Look for digital source...

			if (this.usedKeysAndAxes.keysBitset != 0)
				{
				for (int i = 0; i < GamepadManager.MAX_INTERNAL_KEYS; ++i)
					{ 
					if ((this.usedKeysAndAxes.keysBitset & (1 << i)) != 0)
						{
						this.bestSource.SetKey(i);
						return;
						}
					}
		
				}
			
			// Use shared axie if no exclusive axis or exclusive digital source is available... 

			if (sharedAxisId >= 0)
				{
				this.bestSource.SetAxis(sharedAxisId, ((this.usedKeysAndAxes.axesPositiveBitset & (1 << sharedAxisId)) != 0));
				return;
				}			
			}


		// ------------------------
		public static void ToKeySource(GamepadProfile.KeySource keyDst, GamepadElementConfig keySrc)
			{	
			keyDst.Clear();
				
			InputSource s = keySrc.bestSource; //((keySrc.sourceList.Count == 0) ? null : keySrc.sourceList[0]);
			if (s.IsEmpty())
				return;				

			if (s.IsKey())
				keyDst.SetKey(s.keyId);
			else 
				keyDst.SetAxis(s.axisId, s.axisPositive);		
			}
			
		// ---------------------
		public static void ToJoySource(GamepadProfile.JoystickSource joyDst,
			GamepadElementConfig 	upSrc,
			GamepadElementConfig 	downSrc,
			GamepadElementConfig 	leftSrc,
			GamepadElementConfig 	rightSrc)	
			{
			ToKeySource(joyDst.keyU, upSrc);
			ToKeySource(joyDst.keyD, downSrc);
			ToKeySource(joyDst.keyL, leftSrc);
			ToKeySource(joyDst.keyR, rightSrc);			
			}
		
		}



	// -----------------
	// Input Source.
	// -----------------
	public class InputSource
		{
		public int 
			keyId;
		public int
			axisId;
		public bool
			axisPositive;
		//private bool
		//	sharedAxis;


		// -----------------
		public InputSource()
			{
			this.keyId = -1;
			this.axisId = -1;
			this.axisPositive = true;
			//this.sharedAxis = false;
			}
		
		// ---------------------
		public void CopyFrom(InputSource s)
			{
			this.axisId			= s.axisId;
			this.keyId		= s.keyId;
			this.axisPositive	= s.axisPositive;
			//this.sharedAxis		= s.sharedAxis;
			}
			
		// ----------------------
		public void Clear()
			{
			this.axisId			= -1;
			this.keyId			= -1;
			this.axisPositive	= true;
			//this.sharedAxis		= false;
			}


		// ----------------------
		public bool IsEmpty()		{ return ((this.axisId < 0) && (this.keyId < 0)); }
		public bool IsAxis()		{ return (this.axisId >= 0); }
		public bool IsKey()			{ return ((this.keyId >= 0) && (this.axisId < 0)); }

		// -----------------
		public void SetKey(int keyId)
			{
			this.Clear();
			this.keyId = keyId;
			}

		// ------------------
		public void SetAxis(int axisId, bool positiveSide)
			{
			this.Clear();
			this.axisId 		= axisId;
			this.axisPositive 	= positiveSide;
			}
		
		}


	}
}
