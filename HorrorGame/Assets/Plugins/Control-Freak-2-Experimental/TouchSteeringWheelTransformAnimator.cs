#if false

using UnityEngine;
using ControlFreak2.Internal;


namespace ControlFreak2
{
public class SteeringWheelTransformAnimator : ComponentBase	
	{
	public enum AxisId { X, Y, Z };

	public bool				autoConnectToWheel;
	public SteeringWheel 	wheel;
	//public Transform		wheelObj;
		
	public bool		rotate			= true;
	public AxisId	rotationAxis	= AxisId.Z;
	public float	rotationRange	= 45;
		
	public bool		scale = false;
	public float	pressedScale = 1.2f;
	public bool		scaleDownByBaseAlpha	= false;
	

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
	


	// --------------------
	override protected void OnInitComponent()
		{
		//base.OnInitComponent();

		if (!this.autoConnectToWheel || (this.wheel == null))
			this.wheel = this.GetComponentInParent<SteeringWheel>();
			
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


	// -----------------
	void Update()
		{
#if UNITY_EDITOR
		if (!UnityEditor.EditorApplication.isPlaying)
			return;
#endif

		if (!this.IsInitialized || (this.wheel == null))
			return;


		
		bool pressed = this.wheel.Pressed();			
		float t = (pressed ? this.pressAnimDuration : this.releaseAnimDuration) * TouchControl.MAX_ANIM_DURATION;


		if (this.scale || this.scaleDownByBaseAlpha)
			{		
			if (this.scale)
				{
				float targetScale = (pressed ? this.pressedScale : 1.0f);	
				this.smoothScale = ((t <= 0.00001f) ? targetScale : Mathf.SmoothDamp(this.smoothScale, targetScale, ref this.smoothScaleVel, t)); //, 1000000, Time.unscaledDeltaTime));
				}
			else
				this.smoothScale = 1.0f;


			this.transform.localScale = this.initialScale * (this.smoothScale * (this.scaleDownByBaseAlpha ? this.wheel.GetAlpha() : 1.0f));
			}

		if (this.rotate)
			{		
			Vector3 targetEuler = Vector3.zero;
			targetEuler[(int)this.rotationAxis] = -this.wheel.GetValue() * this.rotationRange;

			//this.smoothRotation = ((t <= 0.00001f) ? Quaternion.Euler(targetEuler) : Quaternion.Slerp(this.smoothRotation, Quaternion.Euler(targetEuler), Mathf.Clamp01(CFUtils.realDeltaTime / t))); 
			this.smoothRotation = Quaternion.Euler(targetEuler);
			this.transform.localRotation =  this.smoothRotation * this.initialRotation;
			}
		}
	}

}

#endif
