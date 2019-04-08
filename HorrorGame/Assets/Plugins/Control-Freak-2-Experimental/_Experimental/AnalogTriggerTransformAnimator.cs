#if false

using UnityEngine;
using ControlFreak2.Internal;


namespace ControlFreak2
{
public class AnalogTriggerTransformAnimator : ComponentBase	
	{
	public bool autoConnectToTrigger = true;
	public AnalogTrigger 	trigger;
	//public Transform		wheelObj;


	public bool		scale					= false;
	public bool		scaleDownByBaseAlpha	= false;
	public float	pressedScale			= 1.0f;
	public float	positiveScale			= 1.0f;
	public float	negativeScale			= 1.0f;

	public bool		rotate					= false;
	public Vector3	positiveRotation 		= Vector3.zero;
	public Vector3	negativeRotation		= Vector3.zero;
		
	public bool		move					= false;
	public Vector3	positiveTransl			= Vector3.zero;
	public Vector3	negativeTransl			= Vector3.zero;

	
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
		base.OnInitComponent();

		if (this.autoConnectToTrigger || (this.trigger == null))
			this.trigger = this.GetComponentInParent<AnalogTrigger>();
			
		this.initialTransl		= this.transform.localPosition;
		this.initialScale		= this.transform.localScale;
		this.initialRotation	= this.transform.localRotation;
		this.smoothScale		= 1.0f;
		this.smoothScaleVel		= 0;
		this.smoothTransl		= Vector3.zero;	
		this.smoothTranslVel	= Vector3.zero;
		this.smoothRotation		= Quaternion.identity;
		}
	


	// -----------------
	void Update()
		{
#if UNITY_EDITOR
		if (!UnityEditor.EditorApplication.isPlaying)
			return;
#endif

		if (!this.IsInitialized || (this.trigger == null))
			return;

		float v = this.trigger.GetVal();

		float t = TouchControl.MAX_ANIM_DURATION * (this.trigger.Pressed() ? 0 : this.releaseAnimDuration);
			
		Transform tr = this.transform;
		
//		tr.localScale = this.initialLocalScale * Mathf.Lerp(1, ((v >= 0) ? this.positiveScale : this.negativeScale), ((v >= 0) ? v : -v));
//		tr.localRotation = this.initialLocalRot * Quaternion.Euler((v >= 0) ? (this.positiveRotation * v) : (this.negativeRotation * -v));
//		tr.localPosition = this.initialLocalPos + ((v >= 0) ? (this.positiveTransl * v) : (this.negativeTransl * -v));
			
		bool pressed = this.trigger.Pressed();
	
		if (this.move)
			{		
			Vector3 targetTransl = (!pressed ? Vector3.zero : ((v >= 0) ? (this.positiveTransl * v) : (this.negativeTransl * -v)));	
			this.smoothTransl = ((t <= 0.00001f) ? targetTransl : Vector3.SmoothDamp(this.smoothTransl, targetTransl, ref this.smoothTranslVel, t, 1000000, Time.unscaledDeltaTime));
			this.transform.localPosition = this.initialTransl + this.smoothTransl;
			}

		if (this.scale || this.scaleDownByBaseAlpha)
			{		
			if (this.scale)
				{
				float targetScale = (!pressed ? 1.0f : Mathf.Lerp(1, ((v >= 0) ? this.positiveScale : this.negativeScale), ((v >= 0) ? v : -v)));
				this.smoothScale = ((t <= 0.00001f) ? targetScale : Mathf.SmoothDamp(this.smoothScale, targetScale, ref this.smoothScaleVel, t, 1000000, Time.unscaledDeltaTime));
				}
			else
				this.smoothScale = 1.0f;


			this.transform.localScale = this.initialScale * (this.smoothScale * (this.scaleDownByBaseAlpha ? this.trigger.GetAlpha() : 1.0f));
			}

		if (this.rotate)
			{		
			Quaternion targetRot = (!pressed ? Quaternion.identity : Quaternion.Euler((v >= 0) ? (this.positiveRotation * v) : (this.negativeRotation * -v)));
			this.smoothRotation = ((t <= 0.00001f) ? targetRot : Quaternion.Slerp(this.smoothRotation, targetRot, Mathf.Clamp01(CFUtils.realDeltaTime / t))); 
			this.transform.localRotation =  this.smoothRotation * this.initialRotation;
			}


		}
	}

}

#endif
