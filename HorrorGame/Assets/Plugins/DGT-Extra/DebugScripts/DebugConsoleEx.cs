using UnityEngine;
using System.Collections.Generic;


namespace ControlFreak2
{

public class DebugConsoleEx
	{
	private int 		maxItems;	
	private List<Entry> items;

	public Vector2 	scrollPos;
	public bool 	showLineNumbers;

		
	private float 	curTime;
		
	private bool	enableFading;
	private float 	fadeTime;
	private Color	normalColor,
					freshColor;

	private int 	itemCountAtUpdate;
	private int 	lastDrawFrame;

	public GUIStyle	itemStyle;


	// ----------------

	private struct Entry
		{
		public string msg;
		public Color color;
		public float time;
		}
		


	// --------------------
	public DebugConsoleEx(int maxItems, GUIStyle itemStyle,  bool enableFading = true, float fadeTime = 2.0f)
		{
		this.maxItems		= Mathf.Max(0, maxItems);
		this.items			= new List<Entry>((maxItems > 0) ? (maxItems + 1) : 128);
		this.enableFading	= enableFading;			
		this.fadeTime 		= fadeTime;
		this.normalColor	= Color.black;
		this.freshColor		= Color.red;
		this.itemStyle 		= itemStyle;
		this.curTime		= 0;

		this.Reset();
		}


	// ----------------------
	public void Reset()
		{
		this.items.Clear();
		}

	public void Clear()
		{
		this.items.Clear();
		}
		
		
	// ---------------------
	public void Update()
		{
		this.curTime += Time.unscaledDeltaTime;
		}

	// --------------------
	public void Log(string s)				{ this.Log(Color.white, s); }
	public void Log(Color color, string s)
		{
		Entry entry	= new Entry();
		entry.msg	= s;
		entry.color	= color;
		entry.time	= this.curTime;
			
		this.items.Insert(0, entry);

		if ((this.maxItems > 0) && (this.items.Count > this.maxItems))
			this.items.RemoveAt(this.items.Count - 1);
			
		}
		
		

		

	// ----------------------
	private Color GetItemColor(Entry entry)
		{
		if (!this.enableFading || (entry.time > this.curTime))
			return this.normalColor;

		float t = (this.curTime - entry.time);
		if (t > this.fadeTime)
			return this.normalColor;

		return Color.Lerp(this.freshColor, this.normalColor, (t / this.fadeTime));	
		}
		


	// -----------------------
	public void DrawGUI()
		{
		if (this.items == null)
			return;

		if (this.lastDrawFrame != Time.frameCount)
			{
			this.lastDrawFrame 		= Time.frameCount;	
			this.itemCountAtUpdate 	= this.items.Count;
			}
	

		this.scrollPos = GUILayout.BeginScrollView(this.scrollPos); //, 
	//		GUILayout.Height(300)); //, GUILayout.MaxWidth(Screen.width)); //.BeginArea(rect);
		
		int numOfItemsToDraw = Mathf.Clamp(this.itemCountAtUpdate, 0, this.items.Count);

		for (int i = 0; i < numOfItemsToDraw; ++i)
			{
			Entry entry = ((i < this.items.Count) ? this.items[i] : (new Entry()));

			GUI.contentColor = this.GetItemColor(entry);
			GUI.backgroundColor = entry.color;

			if (this.itemStyle != null)
				GUILayout.Box(entry.msg, this.itemStyle);//(this.showLineNumbers ? ("" + i + " / " + this.itemCount + ": ") : "") + this.items[itemId]);			
			else
				GUILayout.Box(entry.msg);

			}
	
		GUILayout.EndScrollView(); //.EndArea();
		}

	}
}
