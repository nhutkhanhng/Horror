using UnityEngine;

using ControlFreak2;

namespace ControlFreak2Test
{

public class CF2MoveObject : MonoBehaviour 
	{
		
	public string
		horzAxis = "Horizontal",
		vertAxis = "Vertical";


	private Rigidbody
		rigidBody;


	public WorldPlane
		plane;
		

	private int
		horzAxisId,
		vertAxisId;	
	
	public float speed = 10;
		
		
public float debugAngle;
public Camera debugCam;


	// ------------------
	void OnEnable()
		{
		this.rigidBody = this.GetComponent<Rigidbody>();
		}

	// ---------------------
	void FixedUpdate()
		{
		float tilt;
		float angle = GetJoyAngleAndTilt(Camera.main.transform, CF2Input.GetAxis(this.horzAxis, ref this.horzAxisId), CF2Input.GetAxis(this.vertAxis, ref this.vertAxisId), this.plane, out tilt);
			
this.debugAngle = angle;
this.debugCam = Camera.main;

		Vector3 worldVec = AngleToWorldVec(angle, this.plane) * tilt; 		
			
		Vector3 targetPos = this.transform.position + (worldVec * (Time.deltaTime * this.speed)); 
			
		if (this.rigidBody != null)
			this.rigidBody.MovePosition(targetPos);
		else
			this.transform.position = targetPos;	


		}
		

//void OnGUI()
//	{
//	if (CF2Input.activeRig == null) 
//		GUILayout.Box("RIG NULL!");
//	else if (CF2Input.activeRig.tilt == null)
//		GUILayout.Box("TILT NULL!");
//	else if (CF2Input.activeRig.tilt.tiltState == null)
//		GUILayout.Box("TILT STATE  NULL!");
//	else
//		{
		
//		GUILayout.Box("Tilt Angles: " + CF2Input.activeRig.tilt.tiltState.GetAngles() + "\nTilt Analog : " + CF2Input.activeRig.tilt.tiltState.GetAnalog());
//		if (GUILayout.Button("Calibrate"))
//			CF2Input.CalibrateTilt();
//		}
//	}


		
	public enum WorldPlane
		{
		XZ,
		XY,
		ZY
		}
		

	// ----------------------
	// Transform 2d vector from camera space onto world plane...
	// -----------------------
	static public Vector3 GetWorldVecFromCamera(Transform camTr, float x, float y, WorldPlane plane)
		{
		Vector3 xDir;
		Vector3 yDir;

		switch (plane)
			{
			case WorldPlane.XZ :
				xDir = camTr.right;
				yDir = camTr.forward; 
				xDir.y = xDir.z;
				yDir.y = yDir.z;
				break;

			case WorldPlane.XY :
				xDir = camTr.right;
				yDir = camTr.up;
				xDir.z = 0;
				yDir.z = 0;
				break;

			case WorldPlane.ZY :
			default:
				xDir = camTr.forward;
				yDir = camTr.up;
				xDir.x = xDir.z;
				yDir.x = yDir.z;
				//xDir.x = 0;
				//yDir.x = 0;
				break;
			}

		xDir.z = 0;
		yDir.z = 0;

		xDir.Normalize();
		yDir.Normalize();


		return ((xDir * x) + (yDir * y));
	
		
			/*
		xDir.z = 0;
		xDir.Normalize();

		yDir.z = 0;
		yDir.Normalize();


		Vector2 inputv = new Vector2(x, y);
		return new Vector2(Vector2.Dot((Vector2)xDir, inputv), Vector2.Dot((Vector2)yDir, inputv));
*/
		}


	// -------------------------
	/// Calculate joystick's angle and tilt...
	// -------------------------
	static public float GetJoyAngleAndTilt(Transform camTr, float x, float y, WorldPlane plane, out float tilt)
		{
		Vector2 inputv = new Vector2(x, y);
		if (inputv.sqrMagnitude < 0.000001f)	
			{
			tilt = 0;
			return 0;
			}

		Vector2 v = GetWorldVecFromCamera(camTr, x, y, plane);
		float d = v.magnitude;
		if (d < 0.0001f)
			{
			tilt = 0;
			return 0;
			}
		
		v /= d;
		
		tilt = Mathf.Min(d, 1.0f);
		
		return (Mathf.Rad2Deg * Mathf.Atan2(v.x, v.y));
		}
	

	// -----------------
	static public Vector3 AngleToWorldVec(float angle, WorldPlane plane)
		{
		angle *= Mathf.Deg2Rad;
		float sinv = Mathf.Sin(angle);
		float cosv = Mathf.Cos(angle);

		switch (plane)
			{
			case WorldPlane.XY : 
				return new Vector3(sinv, cosv, 0); 

			case WorldPlane.XZ : 	
				return new Vector3(sinv, 0, cosv); 

			case WorldPlane.ZY :
			default:
				return new Vector3(0, cosv, sinv); 
			}
		}


	// -------------------
	static public Vector3 RotateVecOnPlane(Vector3 v, float angle, WorldPlane plane)
		{
		angle *= Mathf.Deg2Rad;
		float sinv = Mathf.Sin(angle);
		float cosv = Mathf.Cos(angle);

		switch (plane)
			{
			case WorldPlane.XY : 
				return new Vector3(
					(v.x * cosv) - (v.y * sinv), 
					(v.x * sinv) + (v.y * cosv), 
					v.z); 

			case WorldPlane.XZ : 	
				return new Vector3(
					(v.x * cosv) - (v.z * sinv),  
					v.y, 
					(v.x * sinv) + (v.z * cosv)); 

			case WorldPlane.ZY :
			default:
				return new Vector3(
					v.x, 
					(v.z * sinv) + (v.y * cosv), 
					(v.z * cosv) - (v.y * sinv));  
			}
		}

	}
}
