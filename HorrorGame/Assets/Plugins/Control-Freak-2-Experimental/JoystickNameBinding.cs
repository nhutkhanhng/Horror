#if false

using UnityEngine;

using ControlFreak2;

namespace ControlFreak2.Internal
{
// ------------------------
/// Named Joystick binding.
// -------------------------

[System.Serializable]
public class JoystickNameBinding : InputBindingBase
	{
	//public bool		enabled;
	public string 	joystickName;
	private int 	joyId;

	// ----------------------
	public JoystickNameBinding(InputBindingBase parent = null) : base(parent)
		{
		this.enabled		= false;
		this.joystickName = InputRig.DEFAULT_LEFT_STICK_NAME;
		this.joyId = 0;
		}
		

	// ----------------------
	public JoystickNameBinding(string joyName, bool enabled, InputBindingBase parent = null) : base(parent)
		{
		this.enabled		= enabled;
		this.joystickName = joyName;
		this.joyId = 0;
		}



	// -------------------
	public void CopyFrom(JoystickNameBinding b)
		{
		if (this.enabled = b.enabled)
			{
			this.Enable();
			this.joystickName = b.joystickName;
			}
		}

	// ----------------------
	public void Sync(Vector2 joyVec, bool circleClamped, InputRig rig)
		{
		if (!this.enabled || (rig == null))
			return;
	
		rig.SetJoystickState(this.joystickName, ref this.joyId, joyVec, circleClamped);
		}

	
	// -------------------
	public JoystickState GetState(InputRig rig)
		{
		if (!this.enabled || (rig == null)) return null;
		return rig.GetJoystickState(this.joystickName, ref this.joyId);
		}

	public JoystickState GetState()	{ return GetState(CF2Input.activeRig); }



	// --------------------
	override protected bool OnIsBoundToAxis(string axisName, InputRig rig)
		{
		InputRig.VirtualJoystickConfig joyConfig;
		if ((rig == null) || ((joyConfig = rig.joysticks.Get(this.joystickName, ref this.joyId)) == null))
			return false;

		return joyConfig.IsBoundToAxis(axisName, rig);
		}

	// -----------------
	override protected bool OnIsBoundToKey(KeyCode keycode, InputRig rig)
		{
		InputRig.VirtualJoystickConfig joyConfig;
		if ((rig == null) || ((joyConfig = rig.joysticks.Get(this.joystickName, ref this.joyId)) == null))
			return false;

		return joyConfig.IsBoundToKey(keycode, rig);
		}

	// ---------------
	override protected bool OnIsBoundToJoystick(string joyName, InputRig rig)
		{
		return (this.enabled && (this.joystickName == joyName));
		}

	}



}

#endif
