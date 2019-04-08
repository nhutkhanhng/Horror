using UnityEngine;

namespace ControlFreak2
{
public class SimpleFollowCam : MonoBehaviour 
	{
	public Transform target;
		
	public bool 
		followX = true,
		followY = true,
		followZ = true;

	public float smoothingTime = 0.1f;		
		
	private Vector3 targetPos;
	private Vector3 followPosVel;

	private Vector3 offset;

	
	// -------------------
	private void StoreOffset()
		{
		if (this.target != null)
			this.offset = this.transform.position - this.target.position;
		}

	// -----------------------
	void Start()
		{
		this.StoreOffset();
		}


	// -------------------
	void Update()
		{
		if (this.target == null)
			return;
			
		Vector3 curPos = this.transform.position;

		this.targetPos = this.target.position + this.offset;
			
		if (!this.followX) this.targetPos.x = curPos.x;
		if (!this.followY) this.targetPos.y = curPos.y;
		if (!this.followZ) this.targetPos.z = curPos.z;
	
		this.transform.position = Vector3.SmoothDamp(curPos, this.targetPos, ref this.followPosVel, this.smoothingTime, 1000000, Time.unscaledDeltaTime);
		}

	}
	
}
