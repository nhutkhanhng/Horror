#if false
using UnityEngine;
using UnityEngine.EventSystems;

namespace ControlFreak2
{
public class JoystickHat : MonoBehaviour // Touchable
	{
	private Joystick joy;		// reference to parent joystick
	
	// -----------------
	void Start()
		{
		this.Init();
		}


	// ----------------
	private void Init()
		{
		this.joy = null;

		Transform parent = this.transform.parent;
		if (parent != null)
			{
			this.joy = parent.GetComponent<Joystick>(); 
			}

		if (this.joy == null)
			{
#if UNITY_EDITOR	
			Debug.LogError("JoystickHat [" + this.name + "] must be a child of Joystick!!");
#endif		
			//this.gameObject.SetActive(false);
			}
		else
			{
			//this.joy.SetHat(this);
			}
		
		}


/*
	// -----------------
	override public bool CanRecievePointerEvent(Touchable o)
		{
		if (o == this.joy)
			return false;

		return base.CanRecievePointerEvent(o);
		}


	// --------------
	override public bool HandlePointerDown(PointerEventData data)
		{
		if (this.joy != null)
			{
			return this.joy.HandlePointerDown(data);
			}
			
			return false;
		}
			
		// --------------
		override public bool HandlePointerUp(PointerEventData data)
		{
			if (this.joy != null)
			{
				return this.joy.HandlePointerUp(data);
			}
			
			return false;
		}
		
		// --------------
		override public bool HandlePointerMove(PointerEventData data)
		{
			if (this.joy != null)
			{
				return this.joy.HandlePointerMove(data);
			}
			
			return false;
		}
*/
	}
}

#endif
