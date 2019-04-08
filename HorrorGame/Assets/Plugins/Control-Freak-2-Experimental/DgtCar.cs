using UnityEngine;
using System.Collections;
using ControlFreak2;

namespace ControlFreak2.Demos.Racing
{
public class DgtCar : MonoBehaviour 
	{

	public WheelCollider[] 
		wheels;

	public float 
		forwardTorque = 2500,
		backwardTorgue = 500;
		
	public float 
		turnAngle = 15;
		
	[Range(0, 100000)]
	public float
		velToDownForce = 1,
		minDownForce		= 10,
		maxDownForce		= 100;		

	// ------------------
	void FixedUpdate()
		{
		float h = CF2Input.GetAxis("Horizontal");
		float v = CF2Input.GetAxis("Vertical");

		this.Move(v, v, h);
		}

	// ------------------------	
	private void Move(float accel, float brake, float steer)	
		{
		accel = Mathf.Clamp(accel, 0, 1);
		brake = Mathf.Clamp(brake, -1, 0);

		float torque = (this.forwardTorque * accel) + (this.backwardTorgue * brake);
	
		for (int i = 2; i < this.wheels.Length; ++i)
			{
			this.wheels[i].motorTorque = torque;
			}
			
		float steerAngle = steer * this.turnAngle;
		for (int i = 0; i < 2; ++i)
			{
			this.wheels[i].transform.parent.localRotation = Quaternion.Euler(0,steerAngle,  0);
			this.wheels[i].steerAngle = steerAngle;
			}

		Rigidbody rb = this.wheels[0].attachedRigidbody;
			
		float df = Mathf.Clamp(rb.velocity.magnitude * this.velToDownForce, this.minDownForce, this.maxDownForce);

		rb.AddForce(-transform.up * df);
			
		float forwardVel = Vector3.Dot(rb.transform.forward, rb.velocity); 
		
		Vector3 
			frontAxlePos = 	((this.wheels[0].transform.position + this.wheels[1].transform.position) * 0.5f),
			rearAxlePos = 	((this.wheels[2].transform.position + this.wheels[3].transform.position) * 0.5f);

		frontAxlePos = rb.transform.worldToLocalMatrix.MultiplyPoint3x4(frontAxlePos);			
		rearAxlePos = rb.transform.worldToLocalMatrix.MultiplyPoint3x4(rearAxlePos);			


		Vector3 steerVec = (Quaternion.Euler(0, steerAngle, 0) * (new Vector3(0, 0, 1))) * forwardVel * Time.deltaTime;
			
		

		//Vector3 steerDir = (Quaternion.AngleAxis(steerAngle, rb.transform.up) * rb.transform.forward);
		//frontAxlePos += (steerDir * forwardVel);
		
		Vector3 newFrontAxlePos = frontAxlePos + steerVec;
		Vector3 newRearAxlePos = rearAxlePos + ((new Vector3(0, 0, 1)) * forwardVel * Time.deltaTime);
	
		
		//float angleDiff = Vector3.Angle((frontAxlePos - rearAxlePos), (newFrontAxlePos - newRearAxlePos));
		
		Quaternion rotDiff = Quaternion.FromToRotation((frontAxlePos - rearAxlePos), (newFrontAxlePos - newRearAxlePos));

		//float angleDiff = Mathf.Rad2Deg * Mathf.Atan2(
		Vector3 translOfs = ((newFrontAxlePos + newRearAxlePos) * 0.5f) - ((frontAxlePos + rearAxlePos) * 0.5f);
			
		translOfs = rb.transform.localToWorldMatrix.MultiplyVector(translOfs);



		WheelHit hit;
		if (this.wheels[0].GetGroundHit(out hit) || this.wheels[1].GetGroundHit(out hit))
			{
			rb.MoveRotation(rb.rotation * rotDiff); //Quaternion.Euler(0, angleDiff, 0));
			//rb.MovePosition(rb.position + translOfs);
			}

		}

	
	}
}
