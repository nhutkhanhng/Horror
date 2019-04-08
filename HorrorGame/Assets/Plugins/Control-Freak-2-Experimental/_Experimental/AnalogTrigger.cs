#if false

using UnityEngine;
using UnityEngine.EventSystems;

using ControlFreak2.Internal;



namespace ControlFreak2
{
public class AnalogTrigger : TouchControl
	{
	public enum Axis
		{
		HORIZONTAL,
		VERTICAL
		}

	public Axis		axis;
	public float	midPoint;
	

	public string	positiveAxisName;
	public float	positiveStartValue;
	public float	positiveEndValue; 
	public float 	positiveStartPoint;
	public float	positiveEndPoint;

	public string	negativeAxisName;
	public float	negativeStartValue;
	public float	negativeEndValue; 
	public float 	negativeStartPoint;
	public float	negativeEndPoint;
		

	private bool	pointerOn;
	private int		pointerId;

	private TouchGestureBasicState touchState;

	private Vector2	vecRawCur;
	//private Vector2 pressWorldPos;
		
	private float
		valCur,
		valPrev;
		

	public float 
		positiveReturnTime = 1.0f,
		negativeReturnTime = 1.0f;

	public bool					pressBindingOn;
	public DigitalBinding		pressBinding;

	public bool					axisBindingOn;
	public AxisBinding	axisBinding;
	



	// ------------------
	public AnalogTrigger() : base()
		{
		this.touchState		= new TouchGestureBasicState();

		this.axisBinding	= new AxisBinding(false, "Vertical", "Accel", "Brake", false);
		this.pressBinding	= new DigitalBinding(); //KeyCode.None, false, "Fire1", false); //1, 0);
		}
		

	// ------------------
	public bool Pressed()		{ return this.touchState.Pressed(); }
	public float GetVal()		{ return this.valCur; }

	// ------------------
	override protected void OnInitControl()
		{
		}

	
	// ------------------
	override protected void OnUpdateControl()
		{
		if (this.pointerOn && (this.rig != null))
			this.rig.WakeTouchControlsUp();

		this.valPrev = this.valCur;
			

		this.touchState.Update();
		

		if (this.touchState.Pressed()) //.pointerOn)
			{
			float valRaw = this.WorldToNormalizedPos(this.touchState.GetCurPos() /*this.pressWorldPos*/)[((this.axis == Axis.HORIZONTAL) ? 0 : 1)];
			valRaw = ((valRaw + 1.0f) * 0.5f);

			valRaw = Mathf.Clamp(valRaw, -1, 1);
				

			// TODO : proccess more...

			this.valCur = valRaw;
			}
		else
			{
			if (this.valCur != 0)
				{
				float returnTime = (this.valCur < 0) ? this.negativeReturnTime : this.positiveReturnTime;

				if (returnTime <= 0.000001f)
					this.valCur = 0;
				else
					this.valCur = Mathf.MoveTowards(this.valCur, 0, (CFUtils.realDeltaTime / returnTime)); 
				}
			}
		
	
		this.SyncRigState();			
		}

		
	// ---------------------
	override protected void OnDestroyControl()
		{
		}


	// -------------------
	override public void ResetControl()
		{
		this.ReleaseAllTouches(true);
		}


	// -------------------
	private void SyncRigState()
		{
		if (this.pressBindingOn)
			this.pressBinding.Sync(this.Pressed());
			
		if (this.axisBindingOn)
			this.axisBinding.SyncFloat(this.GetVal(), InputRig.InputSource.ANALOG);
		}


	// ----------------------
	override public void ReleaseAllTouches(bool cancel)
		{
		this.pointerOn = false;
		this.touchState.OnTouchEnd(cancel);
		}

	// -------------------	
	override public bool CanBeHitTested()
		{
		return (!this.pointerOn && this.IsActive());
		}
/*
	// ------------------
	override public bool OnTouchStart(TouchControlPanel.SystemTouchEventData data, TouchControl sender)
		{ 
		if (this.pointerOn)
			return false;
			
		this.pointerOn = true;
		this.pointerId = data.id;
			
		Vector3 pos = this.ScreenToWorldPos(data.pos, data.cam);
		//this.pressWorldPos = pos;
			
		this.touchState.OnTouchStart(pos, pos, 0);

		return true;
		}

	// ------------------
	override public bool OnTouchEnd(TouchControlPanel.EventData data, TouchControl sender)
		{
		if (!this.pointerOn || (this.pointerId != data.id))
			return false;
			
		this.pointerOn = false;

		this.touchState.OnTouchEnd(false);

		return true;
		}

	// -------------------
	override public bool OnTouchMove(TouchControlPanel.EventData data, TouchControl sender)
		{
		if (!this.pointerOn || (this.pointerId != data.id))
			return false;
			
		Vector3 pos = this.ScreenToWorldPos(data.pos, data.cam);
		//this.pressWorldPos = pos;
			
		this.touchState.OnTouchMove(pos);

		return true;
		}

	*/	
		
	// TODO !!
	// ---------------------
	override public bool IsBoundToAxis(string axisName)
		{ return false; }

	// ---------------------
	override public bool IsBoundToJoystick(string joyName)			
		{ return false; }


	// ----------------------
	override public bool IsBoundToKeyCode(KeyCode key)
		{ return false; }
	}
}


		
#endif
