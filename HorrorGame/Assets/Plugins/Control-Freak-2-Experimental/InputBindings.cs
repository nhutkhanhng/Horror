using UnityEngine;

using ControlFreak2;

namespace ControlFreak2.Internal
{

	




	/*
// ------------------------
/// Delta vector binding.
// -------------------------

[System.Serializable]
public class DeltaVectorBinding
	{
	public string	deltaVectorName;
	private int 	deltaVectorCachedId;



	// ---------------------
	public DeltaVectorBinding()
		{
		this.deltaVectorName = "Mouse Delta";		
		}

	public DeltaVectorBinding(string targetName)
		{
		this.deltaVectorName = targetName;
		}


	// ----------------------
	public void SyncTouchDelta(Vector2 touchDelta)
		{
		InputRig rig = CF2Input.activeRig;
		if (rig != null)
			rig.SetDeltaVectorTouchState(this.deltaVectorName, ref this.deltaVectorCachedId, touchDelta);
		}

	// --------------------
	public void SyncAnalogVec(Vector2 analogVec)
		{
		InputRig rig = CF2Input.activeRig;
		if (rig != null)
			rig.SetDeltaVectorAnalogState(this.deltaVectorName, ref this.deltaVectorCachedId, analogVec);
		}



	// --------------------
	public bool IsBoundToAxis(string axisName)
		{
		return false;
		}

	// -----------------
	public bool IsBoundToKeyCode(KeyCode keycode)
		{
		return false;
		}

	// ---------------
	public bool IsBoundToJoystick(string joyName)
		{
		return false;
		}

	// ---------------
	public bool IsBoundToDeltaVector(string deltaVecName)
		{
		return (this.deltaVectorName == deltaVecName);
		}



//	public bool		resolutionIndepented;
//	public float	refMonitorHorzRes;
//	
//	public string 	horzAxis;
//	public string	vertAxis;
//	public bool		flipHorz;
//	public bool		flipVert;
//	public float	horzScale;
//	public float	vertScale;
//
//	private int		horzAxisId;
//	private int		vertAxisId;
//
//
//	// ----------------------
//	public DeltaVectorBinding()
//		{
//		this.resolutionIndepented	= true;
//		this.refMonitorHorzRes		= 1024.0f;
//
//		this.horzScale				= 1.0f;
//		this.vertScale				= 1.0f;
//	
//		this.horzAxis				= "Mouse X";
//		this.vertAxis				= "Mouse Y";
//		this.flipHorz				= false;
//		this.flipVert				= false;
//		
//		this.horzAxisId		= 0;
//		this.vertAxisId		= 0;
//		}
//		
//		
//	// ------------------
//	public void SyncAnalog(Vector2 analogVec)
//		{
//		// TODO : 
//		}
//
//	// ----------------------
//	public void SyncTouch(Vector2 deltaVec)
//		{
//		InputRig rig = CF2Input.activeRig;
//		if (rig == null)
//			return;
//	
//		if (this.resolutionIndepented)
//			{
//			// TODO : 
//			}		
//		
//		deltaVec.x *= this.horzScale;
//		deltaVec.y *= this.vertScale;
//
//		if (this.flipHorz)	
//			deltaVec.x = -deltaVec.x;
//		if (this.flipVert)	
//			deltaVec.y = -deltaVec.y;
//
//		rig.SetAxisAnalog(this.horzAxis, ref this.horzAxisId, deltaVec.x);
//		rig.SetAxisAnalog(this.vertAxis, ref this.vertAxisId, deltaVec.y);
//		}

	}
	 */
}
