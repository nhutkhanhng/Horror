using UnityEngine;
using System.IO;
using Touch = ControlFreak2.InputRig.Touch;
using Input = ControlFreak2.CF2Input;

namespace ControlFreak2
{
public class TouchDebug : MonoBehaviour
	{
	//public Texture2D	touchMarker;
	
	public DebugConsole 	touchConsole;
	public TextLog			log;
	
		
	const string LOG_FILENAME = "KageLog.txt";
	
	public int MARKER_SIZE = 32;
	
	private Touch[] touches;
	private int 	touchCount;
	
	public bool displayConsole = true,
					debugLogTouches = false;
		
	public GUIStyle	markerStyle;
	
	private int 	fpsAccum,
					fps;
	private float 	fpsElapsed;
	
	// ---------------------
	void Start()
		{
		this.fpsAccum 		= 0;
		this.fpsElapsed 	= 0;
		this.fps		 	= 0;
	
		this.touches = new Touch[20];
		this.touchCount = 0;
			
	
		this.touchConsole.Init(100);
	
		this.touchConsole.AddItem("Screen : " + Screen.width + " x " + Screen.height + " dpi:" + Screen.dpi);
	
		this.touchConsole.AddItem("PDP: " + Application.persistentDataPath);
		this.touchConsole.AddItem("TDP: " + Application.temporaryCachePath);
		this.touchConsole.AddItem(" DP: " + Application.dataPath);
	
		if (!this.log.Init(Path.Combine(Application.persistentDataPath, LOG_FILENAME)))
			this.touchConsole.AddItem("CANT INIT LOG!!!!!!!");
		else
			this.touchConsole.AddItem("LOG INITIALIZED!");
	
		
	
		}
	
	
	// -----------------------
	public void Update()	
		{
		// Calc fps
	
		this.fpsAccum++;
		if ((this.fpsElapsed += Time.deltaTime)	> 1.0f)
			{
			this.fpsElapsed 	= 0;
			this.fps	 		= this.fpsAccum;
			this.fpsAccum 		= 0;
			}
	
		// Debug touches...
		if (this.debugLogTouches)
			{
		#if UNITY_EDITOR
			if (Input.GetMouseButtonDown(1))
				this.touchConsole.AddItem("MOUSECLICK! " + Time.time);
		#endif
		
				
if ((Input.touchCount > 0) || UnityEngine.Input.GetMouseButton(0)) Debug.Log("[" + Time.frameCount + "] Update count:" + Input.touchCount);

			
			// Process touches...
			
			this.touchCount = 0;
			for (int i = 0; i < Input.touchCount; ++i)
				{
				Touch curTouch = Input.GetTouch(i);
		 
				if (this.touchCount < this.touches.Length)
					this.touches[this.touchCount++] = curTouch;
		
Debug.Log("[" + Time.frameCount + "] Touch[" + i + ":" + curTouch.fingerId + "] Phase:" + curTouch.phase + " pos:" + curTouch.position);

		
				switch (curTouch.phase)
					{
					case TouchPhase.Began : 
						this.touchConsole.AddItem("" + Time.time.ToString("g") + ": [" + curTouch.fingerId + "] BEGAN!");
						break;
					case TouchPhase.Ended : 
						this.touchConsole.AddItem("" + Time.time.ToString("g") + ": [" + curTouch.fingerId + "] Ended!");
						break;
					case TouchPhase.Canceled : 
						this.touchConsole.AddItem("" + Time.time.ToString("g") + ": [" + curTouch.fingerId + "] CANCELD!!!");
						break;
					case TouchPhase.Moved : 
						break;
					case TouchPhase.Stationary : 
						break;
					}			
				}	
			}
		}
	
	
	// ------------------------
	public void OnGUI()
		{
		GUI.color = Color.white;

		for (int i = 0; i < this.touchCount; ++i)
			{
			GUI.DrawTexture(new Rect(this.touches[i].position.x, Screen.height - this.touches[i].position.y, this.MARKER_SIZE, this.MARKER_SIZE), 
				Texture2D.whiteTexture);
			//GUI.Box(new Rect(this.touches[i].position.x, this.touches[i].position.y, this.MARKER_SIZE, this.MARKER_SIZE), 
			//	this.touches[i].fingerId.ToString(), this.markerStyle);
			}

	GUI.color = new Color(1, 0,0, 0.3f);	
	GUI.DrawTexture(new Rect(UnityEngine.Input.mousePosition.x, Screen.height -  UnityEngine.Input.mousePosition.y, this.MARKER_SIZE/2.0f, this.MARKER_SIZE/2.0f), 
			Texture2D.whiteTexture);
		GUI.color = Color.white;

		// Draw console
			
		GUI.Box(new Rect(Screen.width-50,0, 50,20), this.fps.ToString());

		GUILayout.Box("EmuTouch: " + ControlFreak2.CF2Input.activeRig.GetEmuTouchCount() + " In.touchCnt : " + Input.touchCount);
	
		if (this.displayConsole = GUILayout.Toggle(this.displayConsole, "Display Console"))
			{
			GUILayout.BeginHorizontal();
				this.debugLogTouches = GUILayout.Toggle(this.debugLogTouches, "LogTouches");
			GUILayout.EndHorizontal();
	
			this.touchConsole.DrawGUI(); //new Rect
			}
		}
	
	
	
	// ---------------------
	public void OnApplicationPause(bool pause)	
		{ 
		string s = "[SYSTEM] OnAppPause(" + pause + ")";
		
		this.touchConsole.AddItem(s);
		this.log.Log(s); 
		}
	
	public void OnApplicationFocus(bool focus)	
		{ 
		string s = ("[SYSTEM] OnAppFocus(" + focus + ")"); 
	
		this.touchConsole.AddItem(s);
		this.log.Log(s); 
		}
	
	public void OnApplicationQuit()
		{
		string s = ("[SYSTEM] OnAppQuit()");
	
		this.touchConsole.AddItem(s);
		this.log.Log(s); 
		}
	}
	
}

