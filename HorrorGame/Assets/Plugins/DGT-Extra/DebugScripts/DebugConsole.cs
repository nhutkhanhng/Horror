using UnityEngine;

namespace ControlFreak2
{

public struct DebugConsole
	{
	private int 	maxItems;
	public int 		itemCount,
					startItem;
	public string[] items;
	public Vector2 	scrollPos;
	public bool 	showLineNumbers;


	private int 	itemCountAtUpdate;
	private int 	lastDrawFrame;

	
	/*
	private float 	fadeOutTime;
	public Color	fadeoutColor,
					normalColor;


	// -----------------------
	public DebugConsole()
		{
		this.normalColor	= new Color(1, 1, 1, 1);
		this.fadeOutColor	= new Color(1,1,1, 0.3f);
		this.fadeOutTime	= 2.0f;
		}
	*/	

	// -----------------------------
	public void Init(int maxItems, bool showLineNumbers = false)
		{
		this.maxItems 	= Mathf.Max(2, maxItems);
		this.items		= new string[this.maxItems];
		this.itemCount	= 0;
		this.startItem 	= 0;
		this.showLineNumbers = showLineNumbers;
		}

	// --------------------------
	public void AddItem(string v)
		{	
		if (this.items == null)
			return;

		if (this.itemCount < this.maxItems)
			++this.itemCount;

		--this.startItem;
		if (this.startItem < 0)
			this.startItem = this.items.Length - 1;

		this.items[this.startItem] = v;
		}




	// --------------------------
	public void DrawGUI() //Rect rect)
		{
		if (this.items == null)
			return;

		if (this.lastDrawFrame != Time.frameCount)
			{
			this.lastDrawFrame 		= Time.frameCount;	
			this.itemCountAtUpdate 	= this.itemCount;
			}
	

		this.scrollPos = GUILayout.BeginScrollView(this.scrollPos); //, 
	//		GUILayout.Height(300)); //, GUILayout.MaxWidth(Screen.width)); //.BeginArea(rect);
		
		int itemId = this.startItem;
		int numOfItemsToDraw = Mathf.Clamp(this.itemCountAtUpdate, 0, this.itemCount);

		for (int i = 0; i < numOfItemsToDraw; ++i)
			{
			GUILayout.Box((this.showLineNumbers ? ("" + i + " / " + this.itemCount + ": ") : "") + this.items[itemId]);			

			if (++itemId >= this.items.Length)
				itemId = 0;
			}
	
		GUILayout.EndScrollView(); //.EndArea();
		}

	}
}
