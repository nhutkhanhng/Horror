
#if false

using UnityEngine;
using UnityEngine.UI;

using ControlFreak2.Internal;

namespace ControlFreak2
{
	
[RequireComponent(typeof(RectTransform)), ExecuteInEditMode()]
public class AnalogTriggerSpriteAnimator : ComponentBase 
	{
	public bool autoConnectToTrigger;
	public AnalogTrigger trigger;

	public Sprite
		neutralSprite,
		pressedSprite,
		positiveSprite,
		negativeSprite;

	public Color
		neutralColor,
		pressedColor,
		positiveColor,
		negativeColor;

	[Range(0, 1)]
	public float pressAnimDuration		= 0.25f;
	[Range(0, 1)]
	public float releaseAnimDuration	= 0.5f;

		

	/*
	public float
		neutralScale,	
		pressedScale,
		toggledScale;
	*/

	private Image image;
	private RectTransform rectTr;
		


	// ----------------------
	public AnalogTriggerSpriteAnimator()
		{
		this.neutralColor			= new Color(1,1,1, 0.5f);
		this.pressedColor			= new Color(1,1,1, 1.0f);
		this.positiveColor			= new Color(1,1,1, 1.0f);
		this.negativeColor			= new Color(1,1,1, 1);
		//this.neutralScale = 1.0f;
		//this.pressedScale = 1.25f;
		//this.toggledScale = 1.0f;
		}
	
	// ----------------------
	override protected void OnInitComponent()
		{
		this.rectTr = this.GetComponent<RectTransform>();
			
		if (this.autoConnectToTrigger || (this.trigger == null))
			this.trigger = this.GetComponentInParent<AnalogTrigger>(); 

		if (this.trigger == null)
			{
#if UNITY_EDITOR
			Debug.LogError("Could not find an AnalogTrigger parent for [" + this.name + "] AnalogTriggerSpriteAnimator!"); 
#endif
			//this.enabled = false;
			return;
			}

		this.image = this.GetComponent<Image>();
		if (this.image == null)
			{
			this.image = this.gameObject.AddComponent<Image>();
			}

		if ((this.neutralSprite == null) && (this.image.sprite != null))
			{
			this.neutralSprite = this.image.sprite;
			this.pressedSprite = this.image.sprite;
			this.positiveSprite = this.image.sprite;
			this.negativeSprite = this.image.sprite;
			}
		}
	

	// ----------------------
	void Update()		
		{
		if ((this.trigger == null) || (this.image == null))
			return;

		Sprite sprite;
		Color color;
		//float scale;

		bool pressed = this.trigger.Pressed();	

		if (pressed)
			{
			sprite = this.pressedSprite;
			color = this.pressedColor;
			//scale = this.pressedScale;
			}
		else
			{
			sprite = this.neutralSprite;
			color = this.neutralColor;
			//scale = this.neutralScale;
			}



		float t = TouchControl.MAX_ANIM_DURATION * (pressed ? this.pressAnimDuration : this.releaseAnimDuration);

		if (!CFUtils.editorStopped)
			color = ((t <= 0.00001f) ? color : Color.Lerp(this.image.color, color, Mathf.Clamp01(CFUtils.realDeltaTime / t))); 


		this.image.sprite = sprite;
		this.image.color = CFUtils.ScaleColorAlpha(color, this.trigger.GetAlpha());
		}
		
	
	}
}

#endif
