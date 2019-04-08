using UnityEngine;
using System.Collections;

public class TouchControlRenderer : MonoBehaviour {

	protected float alpha;
	protected Color color;
	
	virtual public float GetAlpha() { return this.alpha; }
	virtual public float GetFinalAlpha() { return this.alpha; }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
