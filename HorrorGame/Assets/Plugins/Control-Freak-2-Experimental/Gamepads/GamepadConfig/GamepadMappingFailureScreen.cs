using UnityEngine;
using ControlFreak2;

namespace ControlFreak2.GamepadOptionsUI
{
public class GamepadMappingFailureScreen : GameState
	{
	public GamepadMappingScreen
		frontend;

	public float 
		duration = 3;
	
	private float	
		elapsed = 0;	

	// ----------------------
	protected override void OnStartState (GameState parentState)
		{
		base.OnStartState(parentState);
		this.gameObject.SetActive(true);

		this.elapsed = 0;
		} 

	// ------------------
	protected override void OnExitState ()
		{
		base.OnExitState ();
		this.gameObject.SetActive(false);
		}

	// -------------------
	protected override void OnUpdateState ()
		{
		this.elapsed += CFUtils.realDeltaTimeClamped;
		if (elapsed > this.duration)
			{
			this.EndConfigurationAndGoBack();
			return;
			}
			


		base.OnUpdateState();
		}


	// --------------
	public void EndConfigurationAndGoBack()
		{
		this.frontend.ExitMappingScreen();
		}

	}
}
