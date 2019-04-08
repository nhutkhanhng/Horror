#if false

using UnityEngine;
using ControlFreak2.Internal;

namespace ControlFreak2
{
public class ButtonTransformAnimator : TouchControlTransformAnimatorBase
	{	
	//public bool useVirtualButtonState;
	

	//public bool autoConnectToButton = true;
	//public Button button;		

/*
	[Range(0, 1)]
	public float pressAnimDuration;
	[Range(0, 1)]
	public float releaseAnimDuration;

	public bool scaleDownByBaseAlpha = false;
*/

	public bool move;
	public Vector3 pressedTranslation;
	public Vector3 toggledTranslation;
	public Vector3 pressedAndToggledTranslation;

	public bool scale;	
	public float pressedScale;
	public float toggledScale;
	public float pressedAndToggledScale;
	
	public bool rotate;
	public Vector3 pressedRotation;
	public Vector3 toggledRotation;
	public Vector3 pressedAndToggledRotation;
		

/*
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
*/



	// -----------------
	public ButtonTransformAnimator() : base(typeof(Button))	
		{
		this.pressAnimDuration		= 0.25f;
		this.releaseAnimDuration	= 0.5f;


		this.move = false;
		this.pressedTranslation = new Vector3(0, 0, -1);
		this.toggledTranslation = new Vector3(0, 0, -1);
		this.pressedAndToggledTranslation = new Vector3(0, 0, -1);

		this.scale					= true;
		this.pressedScale			= 1.2f;
		this.toggledScale			= 1.1f;
		this.pressedAndToggledScale	= 1.3f;

		this.rotate					= false;
		this.pressedRotation		= Vector3.zero;
		this.toggledRotation		= Vector3.zero;
		this.pressedAndToggledRotation = Vector3.zero;
		}


	// --------------------
	override protected void OnInitComponent()
		{
		base.OnInitComponent();
/*

		if (this.autoConnectToButton || (button == null))
			button = this.GetComponentInParent<Button>();

		if (button == null)
			{
#if UNITY_EDITOR
			Debug.LogError("ButtonTransformAnimator (" + this.name + ") must be a child of CF2 Button!"); 
#endif
			}
*/
		this.initialTransl		= this.transform.localPosition;
		this.initialScale		= this.transform.localScale;
		this.initialRotation	= this.transform.localRotation;
/*
		this.smoothScale		= 1.0f;
		this.smoothScaleVel		= 0;
		this.smoothTransl		= Vector3.zero;	
		this.smoothTranslVel	= Vector3.zero;
		this.smoothRotation		= Quaternion.identity;
*/		}
		

	// ------------------
//	override protected void OnDestroyComponent()	{}	
//	override protected void OnEnableComponent()		{} 
//	override protected void OnDisableComponent()	{}	


	// ------------------
	override protected void OnUpdateAnimator()
	//void Update()
		{
#if UNITY_EDITOR
		if (!UnityEditor.EditorApplication.isPlaying)	
			return;
#endif
			
		Button button = (Button)this.sourceControl;

		if (button == null)
			return;
			
		bool pressed = button.Pressed();
		bool toggled = button.Toggled();

		float t = TouchControl.MAX_ANIM_DURATION * ((pressed || toggled) ? this.pressAnimDuration : this.releaseAnimDuration);

		if (this.move)
			{		
			Vector3 targetTransl = ((pressed && toggled) ? this.pressedAndToggledTranslation : toggled ? this.toggledTranslation : 
				pressed ? this.pressedTranslation : Vector3.zero);	
			this.smoothTransl = ((t <= 0.00001f) ? targetTransl : Vector3.SmoothDamp(this.smoothTransl, targetTransl, ref this.smoothTranslVel, t, 1000000, Time.unscaledDeltaTime));
			this.transform.localPosition = this.initialTransl + this.smoothTransl;
			}

		if (this.scale || this.scaleDownByBaseAlpha)
			{		
			if (this.scale)
				{
				float targetScale = ((pressed && toggled) ? this.pressedAndToggledScale : toggled ? this.toggledScale : pressed ? this.pressedScale : 1.0f);	
				this.smoothScale = ((t <= 0.00001f) ? targetScale : Mathf.SmoothDamp(this.smoothScale, targetScale, ref this.smoothScaleVel, t, 1000000, Time.unscaledDeltaTime));
				}
			else
				this.smoothScale = 1.0f;


			this.transform.localScale = this.initialScale * (this.smoothScale * (this.scaleDownByBaseAlpha ? button.GetAlpha() : 1.0f));
			}

		if (this.rotate)
			{		
			Quaternion targetRot = ((pressed && toggled) ? Quaternion.Euler(this.pressedAndToggledRotation) : 
				toggled ? Quaternion.Euler(this.toggledRotation) : pressed ? Quaternion.Euler(this.pressedRotation) : Quaternion.identity);
			this.smoothRotation = ((t <= 0.00001f) ? targetRot : Quaternion.Slerp(this.smoothRotation, targetRot, Mathf.Clamp01(CFUtils.realDeltaTime / t))); 
			this.transform.localRotation =  this.smoothRotation * this.initialRotation;
			}

		}
	}
}

#endif

