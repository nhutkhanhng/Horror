using UnityEngine;

using ControlFreak2.Internal;

namespace ControlFreak2
{

public class Joystick3dAnimator : MonoBehaviour 
	{
	public TouchJoystick joy;
	//public Transform stickObj;
		
	public float pressDepth = -5;

	[Tooltip("Max. Horizontal tilt angle.")] //, Range(-90,90)]
	public float horzTiltAngle = 15.0f;

	[Tooltip("Max. Vertical tilt angle.")] //, Range(-90,90)]
	public float vertTiltAngle = 15.0f;

	//[Tooltip("Horizontal tilt rotation axis."), Range(0,2)]
	//public int horzRotAxis = 1;
	//[Tooltip("Vertical tilt rotation axis."), Range(0,2)]
	//public int vertRotAxis = 0;

	[Tooltip("Horizontal transl.")] //, Range(-2,2)]
	public float horzTransl = 10;

	[Tooltip("Vertical translation.")] //, Range(0,2)]
	public float vertTransl = 10;
	

	public float smoothingInTime = 0.1f;
	public float smoothingOutTime = 0.2f;


	private Vector3 initialStickPos;
	private Quaternion initialStickRot;		
		
	//private float 
	//	smoothDepth,	
	//	smoothDepthVel;
	private Quaternion
		smoothRot;
	private Vector3 
		smoothTransl,
		smoothTranslVel;


	// -----------------
	void Start()
		{
		if (this.joy == null)
			{		
			this.joy = this.GetComponentInParent<TouchJoystick>();
			}

		this.smoothRot = Quaternion.identity;
		this.smoothTransl = this.smoothTranslVel = Vector3.zero;


		//this.smoothDepth = 0;
		//this.smoothDepthVel = 0;

		this.initialStickPos = this.transform.localPosition;
		this.initialStickRot = this.transform.localRotation;
		}
		

	// ------------------
	void Update()
		{
#if UNITY_EDITOR
		if (!UnityEditor.EditorApplication.isPlaying)
			return;
#endif
		//Transform tr = this.transform;
			
		if (this.joy != null)
			{
			Vector2 vec = this.joy.GetVector();
			bool pressed = this.joy.Pressed();
				
			Vector3 eulerRot = Vector3.zero;
			//eulerRot[this.horzRotAxis] = vec.x * this.horzTiltAngle;
			//eulerRot[this.vertRotAxis] = vec.y * this.vertTiltAngle;
			eulerRot.y = vec.x * -this.horzTiltAngle;
			eulerRot.x = vec.y * this.vertTiltAngle;
			Quaternion targetRot = Quaternion.Euler(eulerRot);
				
			Vector3 targetTransl = new Vector3(this.horzTransl * vec.x, this.vertTransl * vec.y, (pressed ? this.pressDepth : 0));
			

			float t = (pressed ? this.smoothingInTime : this.smoothingOutTime);
				
			if (t <= 0)
				{
				//this.smoothDepth = (pressed ? this.pressDepth : 0);	
				//this.smoothDepthVel = 0;

				this.smoothRot = targetRot;

				this.smoothTransl = targetTransl;
				this.smoothTranslVel = Vector3.zero;
				}
			else
				{
				//this.smoothDepth = Mathf.SmoothDamp(this.smoothDepth, (pressed ? this.pressDepth : 0), ref this.smoothDepthVel, t);
				this.smoothRot = Quaternion.Slerp(this.smoothRot, targetRot, Mathf.Clamp01(CFUtils.realDeltaTime / t)); 
				this.smoothTransl = Vector3.SmoothDamp(this.smoothTransl, targetTransl, ref this.smoothTranslVel, t, 1000000, Time.unscaledDeltaTime);
				}
			

			this.transform.localRotation = this.smoothRot * this.initialStickRot;

			this.transform.localPosition = this.initialStickPos + this.smoothTransl; //new Vector3(0, 0, this.smoothDepth);
			}
		}
	}
}
