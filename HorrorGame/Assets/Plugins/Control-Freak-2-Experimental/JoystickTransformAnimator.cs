#if false

using UnityEngine;

using ControlFreak2.Internal;


namespace ControlFreak2
{
public class JoystickTransformAnimator : ControlFreak2.Internal.ComponentBase
	{
	public enum AxisId { X, Y, Z }


	public bool autoConnectToJoystick = true;
	public Joystick joystick;

	public bool useVirtualJoystickState;
		
	public bool 	rotate = false;

	public AxisId	horzAxis = AxisId.X;
	public bool		horzAxisFlip = false;

	public AxisId 	vertAxis = AxisId.Y;
	public bool		vertAxisFlip = false;
	public float	rotationAngle			= 0.0f;
		
	public bool 	scale = false;

	public bool		scaleDownByBaseAlpha	= false;
	public float	pressedScale			= 1.0f;

	public bool 	move = false;
	public float 	pressDepth 				= -5;
	public float	moveScale 				= 0.5f;
	public bool		moveRelativeToJoySize	= true;

	
	[Range(0, 1)]
	public float pressAnimDuration		= 0.25f;
	[Range(0, 1)]
	public float releaseAnimDuration	= 0.5f;


	private Vector3 
		initialTransl,
		initialScale,
		smoothTransl,
		smoothTranslVel;

	private float		
		smoothScale,	
		smoothScaleVel;

	private Quaternion 
		initialRotation,
		smoothRotation;


	// ----------------------
	override protected void OnInitComponent()
		{
		
		if (this.autoConnectToJoystick || (this.joystick == null))
			this.joystick = this.GetComponentInParent<Joystick>();


		//this.CheckHierarchy();

			
		this.initialTransl		= this.transform.localPosition;
		this.initialScale		= this.transform.localScale;
		this.initialRotation	= this.transform.localRotation;
		this.smoothScale		= 1.0f;
		this.smoothScaleVel		= 0;
		this.smoothTransl		= Vector3.zero;	
		this.smoothTranslVel	= Vector3.zero;
		this.smoothRotation		= Quaternion.identity;
		}


	// ------------------
	override protected void OnDestroyComponent()	{}	
	override protected void OnEnableComponent()		{} 
	override protected void OnDisableComponent()	{}	

/*
	// ---------------------
	private void CheckHierarchy()
		{		
		if (this.joy == null)
			this.joy = this.GetComponentInParent<Joystick>();
		}
*/

		
	// ----------------------
	void Update()
		{
#if UNITY_EDITOR
		if (!UnityEditor.EditorApplication.isPlaying)
			{
			//this.CheckHierarchy();
			return;
			}
#endif
			
		if (this.joystick == null)
			return;
			
		JoystickState joyState = this.joystick.GetState(this.useVirtualJoystickState);
			
		Vector2 vec = joyState.GetVector();
			

		float t = (this.joystick.Pressed() ? this.pressAnimDuration : this.releaseAnimDuration) * TouchControl.MAX_ANIM_DURATION;

	
		// Move...

		if (this.move)
			{
			Vector2 maxTransl = (this.moveRelativeToJoySize ? (this.joystick.GetLocalRect().size * 0.5f) : (Vector2.one)) * this.moveScale;
			Vector3 targetTransl = new Vector3(maxTransl.x * vec.x, maxTransl.y * vec.y, (this.joystick.Pressed() ? this.pressDepth : 0));
		
			if (t <= 0)
				{
				this.smoothTransl = targetTransl;
				this.smoothTranslVel = Vector3.zero;
				}
			else
				{
				this.smoothTransl = Vector3.SmoothDamp(this.smoothTransl, targetTransl, ref this.smoothTranslVel, t, 1000000, Time.unscaledDeltaTime);
				}
		
			this.transform.localPosition = this.initialTransl + this.smoothTransl; 
			}
	

		// Rotate...

		if (this.rotate)
			{
			Vector3 eulerRot = Vector3.zero;
			eulerRot[(int)this.horzAxis] = vec.x * this.rotationAngle * (this.horzAxisFlip ? -1 : 1);
			eulerRot[(int)this.vertAxis] = vec.y * this.rotationAngle * (this.vertAxisFlip ? -1 : 1);
				
			eulerRot.x = vec.y * this.rotationAngle;
			Quaternion targetRot = Quaternion.Euler(eulerRot);
			
			if (t <= 0)
				{
				this.smoothRotation = targetRot;
				}
			else
				{
				this.smoothRotation = Quaternion.Slerp(this.smoothRotation, targetRot, Mathf.Clamp01(CFUtils.realDeltaTime / t)); 
				}

			this.transform.localRotation = this.smoothRotation * this.initialRotation;
			}
			

		// ... and scale...

		if (this.scale || scaleDownByBaseAlpha)
			{				
			if (this.scale)
				{	
				float targetScale = (this.joystick.Pressed() ? this.pressedScale : 1);
			
				if (t <= 0)
					{
					this.smoothScale = targetScale;
					this.smoothScaleVel = 0;
					}
				else
					{
					this.smoothScale = Mathf.SmoothDamp(this.smoothScale, targetScale, ref this.smoothScaleVel, t, 1000000, Time.unscaledDeltaTime);
					}
				}

			this.transform.localScale = this.initialScale * ((this.scale ? this.smoothScale : 1) * 
				(this.scaleDownByBaseAlpha ? this.joystick.GetAlpha() : 1.0f));
			}

		}
	}
}
#endif

