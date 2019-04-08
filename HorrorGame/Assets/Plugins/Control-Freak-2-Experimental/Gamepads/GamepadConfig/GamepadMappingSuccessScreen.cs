using UnityEngine;
using ControlFreak2;
using UnityEngine.UI;

namespace ControlFreak2.GamepadOptionsUI
{
public class GamepadMappingSuccessScreen : GameState
	{
	public GamepadMappingScreen
		frontend;

	public float 
		duration = 3;
	
	private float	
		elapsed = 0;

	public GameObject
		duplicateProfileGUI,
		newProfileGUI;

	public UnityEngine.UI.InputField	
		profileNameEditText;
	
	

	private GamepadProfile
		gamepadProfile;

	private bool
		profileIsDuplicate;


	// ----------------------
	protected override void OnStartState (GameState parentState)
		{
		base.OnStartState (parentState);

		this.elapsed = 0;

		this.gameObject.SetActive(true);


		// Create the profile and check if it's a duplicate...

		this.gamepadProfile = this.frontend.CreateProfile();
	

		CustomGamepadProfileBank profileBank = GamepadManager.activeManager.GetCustomProfileBank();
		if (profileBank != null)
			{
			this.profileIsDuplicate = (profileBank.FindDuplicate(this.gamepadProfile) >= 0);
			}


		// Setup GUI...

		this.newProfileGUI		.SetActive(this.profileIsDuplicate == false);
		this.duplicateProfileGUI.SetActive(this.profileIsDuplicate == true);

		if (this.profileNameEditText != null)		
			this.profileNameEditText.text = this.gamepadProfile.name;

		}

	// --------------------
	protected override void OnExitState ()
		{
		base.OnExitState ();
		this.gameObject.SetActive(false);
		}


	// -----------------
	protected override void OnUpdateState ()
		{
		this.elapsed += CFUtils.realDeltaTimeClamped;

		if (this.profileIsDuplicate)
			{
			if (elapsed > this.duration)
				{
				this.ApplyProfileAndExit();
				return;
				}
			}


		base.OnUpdateState ();
		}


	// ------------------
	public void ApplyProfileAndExit()
		{
		this.ApplyProfileToGamepad();
		this.frontend.ExitMappingScreen();
		}


	// -----------------
	public void ApplyProfileToGamepad()
		{
		CustomGamepadProfileBank profileBank = (GamepadManager.activeManager.GetCustomProfileBank());
		if (profileBank == null) 
			return;

		string newProfileName = (this.profileNameEditText != null) ? string.Empty : this.profileNameEditText.text;
	
		if (!this.profileIsDuplicate && !string.IsNullOrEmpty(newProfileName))
			this.gamepadProfile.name = newProfileName;

		GamepadProfile profileToApply = profileBank.AddProfile(this.gamepadProfile);
		if (profileToApply != null)
			this.frontend.GetGamepad().SetProfile(profileToApply);

		profileBank.Save();	
		}

	}
}
