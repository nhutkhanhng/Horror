using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraRotation : MonoBehaviour
{	
	public float rotationX = 0F;
	public float rotationY = 0F;

	Quaternion originalRotation;
	private Vector3 playerOriginalRotation;
	
	public Vector3 myRot;
	public Vector3 changeRot;
	
	public GameObject player;

	public Quaternion yQuaternion;
	// Use this for initialization
	void Start ()
	{
		myRot = transform.localRotation.eulerAngles;
		
		originalRotation = transform.localRotation;
		
		rotationX = myRot.x;
		rotationY = myRot.y;
		
		playerOriginalRotation = player.transform.localEulerAngles;
	
	}
	
	// Update is called once per frame
	void Update () {
	
		myRot = transform.localRotation.eulerAngles;
		/*changeRot = myRot - originalRotation.eulerAngles;

		rotationY = changeRot.y;

		//Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
		 yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.up);

		player.transform.localRotation = playerOriginalRotation * yQuaternion;*/
		player.transform.localEulerAngles = new Vector3(playerOriginalRotation.x,myRot.y -90,playerOriginalRotation.z);
	}
	
	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}
