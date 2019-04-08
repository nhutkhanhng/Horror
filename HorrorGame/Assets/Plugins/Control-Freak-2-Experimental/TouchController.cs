#if false
using UnityEngine;
using System.Collections.Generic;

namespace ControlFreak2
{
	
[ExecuteInEditMode()]

public class TouchController : Internal.ComponentBase
	{

//	private List<TouchControl> controls;
//
//	
//
//	// --------------------
//	override protected void OnInitComponent()
//		{
//		if (this.controls == null)
//			this.controls = new List<TouchControl>(32);
//		}
//
//	// --------------------
//	override protected void OnDestroyComponent()
//		{
//		if (this.controls != null)
//			{
//			foreach (TouchControl c in this.controls)
//				{
//				if (c != null) 
//					c.SetController(null);
//				}
//			}
//		}
//
//		
//
//	// ---------------------
//	public void AddControl(TouchControl c)
//		{
//		if (!this.CanBeUsed())
//			return;
//
//#if UNITY_EDITOR
//		if (this.controls.Contains(c))
//			{
//			Debug.LogError("EventRecv(" + this.name + ") already contains " + ((c != null) ? c.name : "NULL"));
//			return;  
//			}
//#endif
//		this.controls.Add(c);
//		}
//
//	// --------------------
//	public void RemoveControl(TouchControl c)
//		{
//		if (!this.CanBeUsed())
//			return;
//
//		if (this.controls != null)
//			this.controls.Remove(c);
//		}	

	}
}

#endif
