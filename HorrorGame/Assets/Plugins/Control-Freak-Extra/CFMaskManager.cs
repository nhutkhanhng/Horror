// ----------------------
// (c) Dan's Game Tools 2015
// ----------------------

// ------------------------------------
// NOTE: 
// If for some reason you want to modify mask parameters at run-time,
// please call this script's UpdateMasks() to, well, update them.
// ------------------------------------



using UnityEngine;

namespace ControlFreakExtra
{
[ExecuteInEditMode()]
public class CFMaskManager : MonoBehaviour 
	{
	[Tooltip("Target controller. (Can be left empty if this script is attached to controller's gameObject.)")] 
	public TouchController ctrl;
	
	public bool hideWhenPlaying;

	
	[Tooltip("List of mask areas.")]
	public MaskAreaParams[] maskAreas;


	// -------------------------
	public CFMaskManager() : base()
		{
		this.hideWhenPlaying = true;
		
		this.maskAreas = new MaskAreaParams[] {
			new MaskAreaParams(0.0f, 0.2f, 0.0f, 1.0f), // left margin
			new MaskAreaParams(0.8f, 1.0f, 0.0f, 1.0f), // right margin
			new MaskAreaParams(0.0f, 1.0f, 0.0f, 0.2f), // top margin
			new MaskAreaParams(0.0f, 1.0f, 0.8f, 1.0f)  // bottom margin
			};
		}

	

	// -------------------
	void OnEnable()
		{
		if (this.ctrl == null)
			this.ctrl = this.GetComponent<TouchController>();
	
		if (this.ctrl != null)
			{
			this.UpdateMasks();
			}
		}

	// --------------------
	void OnDisable()
		{
		if (this.ctrl != null)
			this.ctrl.ResetMaskAreas();
		}

	// --------------------
	void Start()
		{
		this.UpdateMasks();
		}

		
	// --------------------
	void Update()
		{
		if (this.ctrl == null)
			return;

		if (this.ctrl.LayoutChanged())
			{
			for (int i = 0; i < this.maskAreas.Length; ++i)
				{
				this.maskAreas[i].AddToController(this.ctrl);
				}

			this.ctrl.LayoutChangeHandled();
			}
		}

		
#if UNITY_EDITOR
	// ------------------	
	void OnGUI()
		{
		if (this.hideWhenPlaying && UnityEditor.EditorApplication.isPlaying)
			return;

		if (this.maskAreas != null)
			{
			for (int i = 0; i < this.maskAreas.Length; ++i)
				{
				MaskAreaParams mask = this.maskAreas[i];
				if (!mask.IsActive())
					continue;

				GUI.color = mask.maskColor;
				GUI.DrawTexture(mask.GetRect(), Texture2D.whiteTexture);
				}
			}
		}

#endif



	// ------------------
	public void UpdateMasks()
		{
		if (this.ctrl == null)
			return;

		this.ctrl.ResetMaskAreas();

		if (this.maskAreas == null)
			return;

		for (int i = 0; i < this.maskAreas.Length; ++i)
			{
			if (this.maskAreas[i] != null)
				this.maskAreas[i].AddToController(this.ctrl);
			}
		}

		
	// --------------------
		



	// ----------------------
	[System.Serializable]
	public class MaskAreaParams 
		{

		public bool enabled;

		public Color maskColor;

		[Range(0, 1), Tooltip("Minimal X coord (0 = left, 1 = right)")]
		public float minX;
		[Range(0, 1), Tooltip("Maximal X coord (0 = left, 1 = right)")]
		public float maxX;
		[Range(0, 1), Tooltip("Minimal Y coord (0 = top, 1 = bottom)")]
		public float minY;
		[Range(0, 1), Tooltip("Maximal Y coord (0 = top, 1 = bottom)")]
		public float maxY;


		// --------------------
		public MaskAreaParams(float minX, float maxX, float minY, float maxY)
			{
			this.enabled = true;
			this.minX = minX;
			this.maxX = maxX;
			this.minY = minY;
			this.maxY = maxY;
			this.maskColor = new Color(1, 0, 0, 0.5f);
			}

			
		// --------------------
		public bool IsActive()
			{
			return (this.enabled && (this.maxX > this.minX) &&  (this.maxY > this.minY));
			}

		// -------------------
		public Rect GetRect()
			{
			if (!this.IsActive())
				return new Rect(0, 0, 0, 0);

			return (new Rect((Screen.width * this.minX), (Screen.height * this.minY), 
				(Screen.width * (this.maxX - this.minX)), (Screen.height * (this.maxY - this.minY)))); 
			
			}
					

		// --------------------
		public void AddToController(TouchController ctrl)
			{
			if ((ctrl == null) || !this.IsActive())
				return;

			ctrl.AddMaskArea(this.GetRect());
			}
		}
	
	}
}
