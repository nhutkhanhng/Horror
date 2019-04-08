using UnityEngine;
using System.Collections;

namespace ControlFreak2.Extra
{
public class Oscilator : MonoBehaviour
	{
	public Vector3 rotationRange = new Vector3(15, 15, 0);
	public float cycleDuration = 10.0f;

	private Quaternion initialRotation;
	private float elapsed;


	// ----------------------	
	void Start()
		{	
		this.elapsed = 0;
		this.initialRotation = this.transform.localRotation;
		}
	


	// ----------------------
	void Update()
		{		
		this.elapsed += Time.unscaledDeltaTime;
	
		float t = Mathf.Repeat(this.elapsed, this.cycleDuration) / this.cycleDuration;
		this.transform.localRotation = this.initialRotation * Quaternion.Euler(this.rotationRange * Mathf.Cos(t * 2.0f * Mathf.PI));
		}

	
	}
}

