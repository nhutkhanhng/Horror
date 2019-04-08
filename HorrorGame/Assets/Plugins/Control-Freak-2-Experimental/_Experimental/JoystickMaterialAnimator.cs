#if false
using UnityEngine;

namespace ControlFreak2
{
public class JoystickMaterialAnimator : ControlMaterialAnimator
	{
	public bool autoConnectToJoystick = true;
	public Joystick joystick;

	public Color releasedColor = Color.white;
	public Color pressedNeutralColor = Color.white;
	public Color pressedNonNeutralColor = Color.white;
		

	// ---------------------
	override protected void OnInitComponent() 
		{
		base.OnInitComponent();	
			
		if (this.autoConnectToJoystick || (this.joystick == null))
			this.joystick = this.GetComponentInParent<Joystick>();

		if (this.joystick == null)
			{
#if UNITY_EDITOR
			Debug.LogError("JoystickMaterialAnimator (" + this.name + ") must be a child of CF2 Joystick!");
#endif
			}
		}


	// ----------------------
	void Update()
		{
		if (!this.IsInitialized)
			return;

#if UNITY_EDITOR
		if (!UnityEditor.EditorApplication.isPlaying)
			return;
#endif		

		if (this.joystick == null)
			return;
			
		Color color = (!this.joystick.Pressed() ? this.releasedColor : (this.joystick.GetDir() != Dir.N) ? this.pressedNonNeutralColor : this.pressedNeutralColor); 

		this.SetColor(CFUtils.ScaleColorAlpha(color, this.joystick.GetAlpha()));
		}

	}
}

#endif
