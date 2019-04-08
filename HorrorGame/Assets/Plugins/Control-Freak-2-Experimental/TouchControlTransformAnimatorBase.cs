#if false


using UnityEngine;

namespace ControlFreak2.Internal
{
[ExecuteInEditMode()]
public abstract class TouchControlTransformAnimatorBase : TouchControlAnimatorBase 
	{
	[Range(0, 1)]
	public float 
		pressAnimDuration,
		releaseAnimDuration;


	protected Vector3
		initialTransl,
		initialScale;
	protected Quaternion 
		initialRotation;




	// --------------------
	public TouchControlTransformAnimatorBase(System.Type sourceType) : base(sourceType)
		{
		this.pressAnimDuration = 0.25f;
		this.releaseAnimDuration = 0.5f;
		}

	
	// ------------------
	override protected void OnInitComponent()
		{
		base.OnInitComponent();
			

		// Get Initial Transforms...

		Transform tr = this.transform;
	
		this.initialTransl		= tr.localPosition;
		this.initialScale		= tr.localScale;
		this.initialRotation	= tr.localRotation;
		}

	}
}

#endif
