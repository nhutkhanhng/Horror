using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ControlFreak1Extra
{
[AddComponentMenu("ControlFreak/Unity5 UI Blocker")]
public class UIBlocker : Graphic, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
	void IPointerUpHandler.OnPointerUp(PointerEventData data)		{}
	void IPointerDownHandler.OnPointerDown(PointerEventData data)	{} 
	void IDragHandler.OnDrag(PointerEventData data)					{}
		
	// ----------------
	override public bool Raycast(Vector2 sp, Camera cam)
		{		
		return true;
		}
		
	// -------------
	//override protected void OnFillVBO (List<UIVertex> vbo)
	//	{
	//	vbo.Clear();
	//	}
	
	override protected void OnEnable()
		{
		this.color = new Color(1,1,1, 0);
		}
	
	// ----------------
	void OnDrawGizmos()
		{
		RectTransform rectTr = this.transform as RectTransform;
		if (rectTr != null)
			{
			Gizmos.matrix = this.transform.localToWorldMatrix;
			Rect r = rectTr.rect;

			Gizmos.DrawWireCube(r.center, r.size);
			}
		}


#if UNITY_EDITOR
	[UnityEditor.MenuItem("GameObject/UI/UI Blocker", false, 1000)]
	static void CreateUIBlocker()
		{
		Transform parent = UnityEditor.Selection.activeTransform;
		if (parent != null)
			{
			Canvas canvas = parent.GetComponent<Canvas>();
			if (canvas == null)
				canvas = parent.GetComponentInParent<Canvas>();

			if (canvas != null)
				parent = canvas.transform;
			else
				parent = null;
			}

		GameObject newObj = new GameObject("CF-UI-Blocker", typeof(UIBlocker));
		if (newObj != null)
			{
			if (parent != null)
				newObj.transform.SetParent(parent, false);
			}

		UnityEditor.Selection.activeGameObject = newObj;
		}
#endif

	}
}
