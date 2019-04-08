using UnityEngine;

namespace ControlFreak2.Internal
{

public class HierarchyDummy : MonoBehaviour 
	{
	// -----------------------
	void OnTransformParentChanged()
		{
		this.SendMessageUpwards("OnControlParentChanged()", SendMessageOptions.DontRequireReceiver);
		}
	

	}
}
