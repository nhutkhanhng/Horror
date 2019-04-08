using UnityEngine;
using System.Collections;

namespace ControlFreak2
{
public class DansLogger : MonoBehaviour 
	{
	private TextLog log;
	private DebugConsole debugConsole;

	static private DansLogger mInst;
		
	public GUISkin customConsoleSkin;
	//public float consoleAlpha = 0.5f;		


	public bool runOnStandalone = false;
	public bool runInEditor		= false;
	public bool runOnAndroid	= true;

	public string logFilePrefix = "Log";
	public string logFileExt = ".txt";
	public bool addDateToLogFile = true;	
		
	public bool displayDebugConsole = false;
	public bool displayDebugConsoleOnError = true;
	public int debugConsoleMaxItems = 32;

	private bool debugConsoleVisible = false;
	private bool textLogActive = false;

	private bool errorEncountered = false;

	
	// --------------------
	void Awake()
		{
		if (((mInst != null) && mInst) 
			)
			{
			this.gameObject.SetActive(false);
			return;
			}

		mInst = this;
		

		Application.logMessageReceived += LogCallback;

		GameObject.DontDestroyOnLoad(this);
	
		
		if ((!this.runOnAndroid && (Application.platform == RuntimePlatform.Android)) ||
#if UNITY_EDITOR
			(!this.runInEditor) ||
#endif
			(!this.runOnStandalone && (SystemInfo.deviceType == DeviceType.Desktop)))
			{
			this.textLogActive = false;
			}
		else
			this.textLogActive = true;


		

		// Setup debug console..

		this.debugConsole.Init(this.debugConsoleMaxItems);
		
		this.debugConsoleVisible = 	this.displayDebugConsole;



		// Create log file...

		if (this.textLogActive)
			{
	
			string logFile = this.logFilePrefix;
			
			if (this.addDateToLogFile)
				logFile += "_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm");
			
			logFile += this.logFileExt;

			logFile = System.IO.Path.Combine(Application.persistentDataPath, logFile);
	
			this.debugConsole.AddItem("Logging to: " + logFile);

			if (!this.log.Init(logFile, false))
				{
				this.errorEncountered = true;
				this.displayDebugConsole = true;
				this.debugConsole.AddItem("CAN't START LOG!!!");
				}
			}


	
		}


	// ------------------------
	void OnDestroy()
		{
		if (mInst != this)
			return;

		mInst = null;

		Application.logMessageReceived -= LogCallback;
			
		if (this.textLogActive)
			this.log.Log("End of file!");
		}
	

	// ----------------
	void OnGUI()
		{
		if (!this.debugConsoleVisible)
			return;
		
		GUISkin initialSkin = GUI.skin;
		if (this.customConsoleSkin != null)
			GUI.skin = this.customConsoleSkin;


		//GUI.color = new Color(1,1,1, this.consoleAlpha);

		GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
		
			if (this.errorEncountered)
				{
				if (GUILayout.Button("Exit application", GUILayout.ExpandWidth(true), GUILayout.MinHeight(50)))
					Application.Quit();
				}

			this.debugConsole.DrawGUI();

		GUILayout.EndArea();

		GUI.skin = initialSkin;
		}

	
	// ---------------------------
	private void LogCallback(string logString, string stackTrace, LogType logType)
		{
		string s = logString;
			
		switch (logType)
			{
			case LogType.Assert 	: s = "[ASSRT] " + s; break;
			case LogType.Error		: s = "[ERROR] " + s; break;
			case LogType.Exception	: s = "[EXCPT] " + s; break;
			case LogType.Warning 	: s = "[W] " + s; break;
			}

		this.debugConsole.AddItem(s);


		if (this.textLogActive)
			{
	
			if ((logType == LogType.Error) || (logType == LogType.Assert) || (logType == LogType.Exception))
				{
				s += "\n\tStack: " + stackTrace + "\n";
				}
	
			this.log.Log(s);
			}

		if ((logType == LogType.Error) || (logType == LogType.Assert) || (logType == LogType.Exception))
			{
			this.errorEncountered = true;

			if (this.displayDebugConsoleOnError)
				this.debugConsoleVisible = true;
			}

		}

	}
}
