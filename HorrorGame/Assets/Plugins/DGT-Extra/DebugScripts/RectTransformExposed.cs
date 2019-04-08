using UnityEngine;

namespace ControlFreak2
{
[ExecuteInEditMode()]
public class RectTransformExposed : MonoBehaviour 
	{
		
	private RectTransform rectTr;

	public Vector2
		offsetMin,
		offsetMax;
		
	public Vector2 	
		anchoredPosition3D;
	
	public Vector2
		anchorMin,
		anchorMax;


	// -----------------
	void Awake()
		{
		this.rectTr = this.GetComponent<RectTransform>();
		}


	// ----------------
	void Update()
		{
		if (this.rectTr == null)
			return;

		this.offsetMin = this.rectTr.offsetMin;
		this.offsetMax = this.rectTr.offsetMax;

		this.anchorMin = this.rectTr.anchorMin;
		this.anchorMax = this.rectTr.anchorMax;
			
		this.anchoredPosition3D = this.rectTr.anchoredPosition3D;
		}

	
	}
}
