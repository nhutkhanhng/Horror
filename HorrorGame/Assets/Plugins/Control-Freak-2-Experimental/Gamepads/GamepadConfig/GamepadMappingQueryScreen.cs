using UnityEngine;
using ControlFreak2;

namespace ControlFreak2.GamepadOptionsUI
{
public class GamepadMappingQueryScreen : GamepadMappingQueryScreenBase
	{
	public GamepadMappingScreen
		frontend;

	public float
		outcomeScreenDuration = 1.0f;
	private float
		outcomeScreenElapsed = 0;


	public GameObject
		activeQueryGUI,
		waitingForInputGUI,
		waitingForNeutralGUI,	
		waitingForConfirmationGUI;

	public GameObject
		successMsgGUI,
		skippedMsgGUI,

		essentialElemInfo;

	public UnityEngine.UI.Slider
		waitingForInputSlider,
		waitingForNeutralSlider,
		waitingForConfirmationSlider;

	public GameObject
		infoForLeftStick,
		infoForLeftStickR,
		infoForLeftStickL,
		infoForLeftStickU,
		infoForLeftStickD,
	
		infoForRightStick,
		infoForRightStickR,
		infoForRightStickL,
		infoForRightStickU,
		infoForRightStickD,

		infoForDpad,
		infoForDpadStickR,
		infoForDpadStickL,
		infoForDpadStickU,
		infoForDpadStickD,

		infoForFaceButtons,
		infoForFaceBottom,
		infoForFaceRight,	
		infoForFaceLeft,
		infoForFaceTop,

		infoForStartAndSelect,
		infoForSelect,
		infoForStart,
		
		infoForLR1,
		infoForL1,
		infoForR1,

		infoForLR2,
		infoForL2,
		infoForR2,

		infoForLR3,
		infoForL3,
		infoForR3;




	// ----------------
	protected override void OnStartState (GameState parentState)
		{
		base.OnStartState (parentState);

		this.frontend = (GamepadMappingScreen)this.config;


		this.gameObject.SetActive(true);


		this.successMsgGUI.SetActive(false);
		this.skippedMsgGUI.SetActive(false);
		
		this.activeQueryGUI.SetActive(true);

		if (this.essentialElemInfo != null)
			this.essentialElemInfo.SetActive(GamepadMappingScreenBase.IsElemEssential(this.elemId));

		// Activate info...

		EnableGameObject(this.infoForLeftStick, 	
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.LEFT_ANALOG_DOWN) ||
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.LEFT_ANALOG_UP) ||
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.LEFT_ANALOG_LEFT) ||
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.LEFT_ANALOG_RIGHT));

		EnableGameObject(this.infoForLeftStickR, 	(this.elemId == GamepadMappingScreenBase.GamepadElementId.LEFT_ANALOG_RIGHT));
		EnableGameObject(this.infoForLeftStickL, 	(this.elemId == GamepadMappingScreenBase.GamepadElementId.LEFT_ANALOG_LEFT));
		EnableGameObject(this.infoForLeftStickU,	(this.elemId == GamepadMappingScreenBase.GamepadElementId.LEFT_ANALOG_UP));
		EnableGameObject(this.infoForLeftStickD,	(this.elemId == GamepadMappingScreenBase.GamepadElementId.LEFT_ANALOG_DOWN));
		
		
	EnableGameObject(	this.infoForRightStick,		
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.RIGHT_ANALOG_DOWN) ||
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.RIGHT_ANALOG_UP) ||
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.RIGHT_ANALOG_LEFT) ||
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.RIGHT_ANALOG_RIGHT));

		EnableGameObject(this.infoForRightStickR,	(this.elemId == GamepadMappingScreenBase.GamepadElementId.RIGHT_ANALOG_RIGHT));
		EnableGameObject(this.infoForRightStickL,	(this.elemId == GamepadMappingScreenBase.GamepadElementId.RIGHT_ANALOG_LEFT));
		EnableGameObject(this.infoForRightStickU,	(this.elemId == GamepadMappingScreenBase.GamepadElementId.RIGHT_ANALOG_UP));
		EnableGameObject(this.infoForRightStickD,	(this.elemId == GamepadMappingScreenBase.GamepadElementId.RIGHT_ANALOG_DOWN));
	


		EnableGameObject(this.infoForDpad,		
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.DPAD_DOWN) ||
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.DPAD_UP) ||
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.DPAD_LEFT) ||
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.DPAD_RIGHT));
		EnableGameObject(this.infoForDpadStickR,	(this.elemId == GamepadMappingScreenBase.GamepadElementId.DPAD_RIGHT));
		EnableGameObject(this.infoForDpadStickL,	(this.elemId == GamepadMappingScreenBase.GamepadElementId.DPAD_LEFT));
		EnableGameObject(this.infoForDpadStickU,	(this.elemId == GamepadMappingScreenBase.GamepadElementId.DPAD_UP));
		EnableGameObject(this.infoForDpadStickD,	(this.elemId == GamepadMappingScreenBase.GamepadElementId.DPAD_DOWN));
		
		EnableGameObject(this.infoForFaceButtons,		
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.FACE_BOTTOM) ||
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.FACE_TOP) ||
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.FACE_LEFT) ||
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.FACE_RIGHT));
		EnableGameObject(this.infoForFaceBottom,	(this.elemId == GamepadMappingScreenBase.GamepadElementId.FACE_BOTTOM));
		EnableGameObject(this.infoForFaceRight,		(this.elemId == GamepadMappingScreenBase.GamepadElementId.FACE_RIGHT));
		EnableGameObject(this.infoForFaceLeft, 		(this.elemId == GamepadMappingScreenBase.GamepadElementId.FACE_LEFT));
		EnableGameObject(this.infoForFaceTop,		(this.elemId == GamepadMappingScreenBase.GamepadElementId.FACE_TOP));

		

		EnableGameObject(this.infoForStartAndSelect,		
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.SELECT_BUTTON) ||
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.START_BUTTON));
		EnableGameObject(this.infoForSelect, 	(this.elemId == GamepadMappingScreenBase.GamepadElementId.SELECT_BUTTON));
		EnableGameObject(this.infoForStart, 	(this.elemId == GamepadMappingScreenBase.GamepadElementId.START_BUTTON));
		
		
		EnableGameObject(this.infoForLR1,		
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.L1) ||
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.R1));
		EnableGameObject(this.infoForL1, 	(this.elemId == GamepadMappingScreenBase.GamepadElementId.L1));
		EnableGameObject(this.infoForR1, 	(this.elemId == GamepadMappingScreenBase.GamepadElementId.R1));

		EnableGameObject(this.infoForLR2,		
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.L2) ||
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.R2));
		EnableGameObject(this.infoForL2, 	(this.elemId == GamepadMappingScreenBase.GamepadElementId.L2));
		EnableGameObject(this.infoForR2, 	(this.elemId == GamepadMappingScreenBase.GamepadElementId.R2));

		EnableGameObject(this.infoForLR3,		
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.L3) ||
				(this.elemId == GamepadMappingScreenBase.GamepadElementId.R3));
		EnableGameObject(this.infoForL3, 	(this.elemId == GamepadMappingScreenBase.GamepadElementId.L3));
		EnableGameObject(this.infoForR3, 	(this.elemId == GamepadMappingScreenBase.GamepadElementId.R3));
	

		this.StartActiveQuery();
		}

	// -------------------
	private void EnableGameObject(GameObject go, bool enable)
		{
		if (go != null) go.SetActive(enable);
		}

	// ----------------------
	protected override void OnExitState ()
		{
		base.OnExitState ();
		this.gameObject.SetActive(false);

		}


	// ----------------
	protected override void OnUpdateState ()
		{
		this.UpdateQuery();

		if (this.IsQueryActive())
			{

			// Update Query progress..

			UnityEngine.UI.Slider 
				progressSlider = null;

			switch (this.GetQueryPhase())
				{
				case QueryPhase.WAITING_FOR_INPUT :			progressSlider = this.waitingForInputSlider; break;
				case QueryPhase.WAITING_FOR_NEUTRAL :		progressSlider = this.waitingForNeutralSlider; break;
				case QueryPhase.WAITING_FOR_CONFIRMATION :	progressSlider = this.waitingForConfirmationSlider; break;
				}

			if (progressSlider != null)
				progressSlider.value = this.GetQueryPhaseProgress();
			}
		else
			{
			if ((this.outcomeScreenElapsed += CFUtils.realDeltaTimeClamped) > this.outcomeScreenDuration)
				{
				this.ReturnToConfigScreen();
				return;
				}
			}


		// Update sub states...

		base.OnUpdateState ();
		}


	// -------------------	
	protected override void OnChangeQueryPhase (QueryPhase phaseId)
		{
		base.OnChangeQueryPhase (phaseId);

		this.waitingForNeutralGUI		.SetActive(phaseId == QueryPhase.WAITING_FOR_NEUTRAL);
		this.waitingForInputGUI			.SetActive(phaseId == QueryPhase.WAITING_FOR_INPUT);
		this.waitingForConfirmationGUI	.SetActive(phaseId == QueryPhase.WAITING_FOR_CONFIRMATION);
		}


	// -----------------
	protected override void OnEndActiveQuery ()
		{
		this.activeQueryGUI.SetActive(false);
Debug.Log("End Query : " + this.GetOutcome());

		this.successMsgGUI.SetActive(this.GetOutcome() == QueryOutcome.SUCCESS);
		this.skippedMsgGUI.SetActive(this.GetOutcome() == QueryOutcome.SKIP);	

		this.outcomeScreenElapsed = 0;


		if ((this.GetOutcome() != QueryOutcome.SUCCESS) && (this.GetOutcome() != QueryOutcome.SKIP))
			this.ReturnToConfigScreen();
		}

	
	}
}
