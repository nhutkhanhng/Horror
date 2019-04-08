using UnityEngine;
using System.Collections;

public class CheckUIPosAndDelegate : MonoBehaviour
	{

	private RectTransform rectTf;
	public Vector3 worldPos;

	public Vector2 scrollVal, scrollAccum;


	void Start ()
		{
		this.rectTf = this.transform as RectTransform;
		//RectTransform.reapplyDrivenProperties += OnLayout;
		}

	void OnDisable()
		{
		//RectTransform.reapplyDrivenProperties -= OnLayout;
		}
	

	void Update()
		{
		this.worldPos = this.rectTf.position;

		this.scrollVal = Input.mouseScrollDelta; 
		this.scrollAccum += this.scrollVal;

		}

/*
	void OnLayout(RectTransform tf)
		{
		Debug.Log("FR[" + Time.frameCount + "] OnLayout(  " + this.name + "  ) TF: " + tf.name);
		}
*/
}
