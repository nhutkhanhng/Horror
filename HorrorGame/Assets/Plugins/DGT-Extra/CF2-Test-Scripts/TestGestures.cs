using UnityEngine;

using ControlFreak2;
//using ControlFreak2.DebugUtils;


/*
 
TODO : odseparować SWIPE THRESH od activate StrictMultiOnSwipeThresh
RESETOWAC JUST PINCHED co klatke!  
 
TODO : wykryc czy CancelTap nie jest robione bez przyczyny
TODO : 


*/


namespace ControlFreak2Test
{
public class TestGestures : MonoBehaviour 
	{
	public SuperTouchZone zone;
		
	private DebugConsoleEx dbgConsole;
	private bool displayConsole;
		
	public GUIStyle consoleItemStyle;

	static private TestGestures mInst;

	public bool 
		printPress 		= false,
		printRelease 	= false,
		printPressEx 	= true,
		printReleaseEx 	= true,
		printHoldPress 	= true,
		printHoldRelease= true,
		printSwipeStart = true,
		printTaps		= true,
		printLongTap	= true;


	// --------------------
	public TestGestures() : base()
		{
		this.dbgConsole = new DebugConsoleEx(128, this.consoleItemStyle, true, 2.0f);
		this.displayConsole = true;
		}

	// -----------------
	void Start()
		{
		mInst = this;

		if (this.zone == null) 
			this.zone = this.GetComponent<ControlFreak2.SuperTouchZone>();
	
		this.dbgConsole.itemStyle = this.consoleItemStyle;
		}
		
		
	// -----------------
	private string dbgMsg;

	void Update()
		{
		this.dbgConsole.Update();

		if (this.zone == null)
			return;
			
		this.dbgMsg = "";
			
		//this.dbgMsg += "" + this.zone.Get
		

		for (int i = 1; i <= 3; i++)
			{
			this.dbgMsg += "" + i + " fingers : Prs:" + this.zone.PressedRaw(i) + "(" + this.zone.PressedNormal(i) + ")" + 
				"\tDrag:" + this.zone.GetUnconstrainedSwipeVector(i) + 
				"\tCDrag:" + this.zone.GetSwipeVector(i) + 
				"\tDir:" + this.zone.GetSwipeDir(i) + 
				"\tScrl:" + this.zone.GetScroll(i) + "\n";
				

					
			if (this.printPress && this.zone.JustPressedRaw(i)) this.LogMsg("F" + i + " pressed!");
			if (this.printPressEx && this.zone.JustPressedNormal(i)) this.LogMsg("F" + i + " pressed Normal!!");

			if (this.printHoldPress && this.zone.JustPressedLong(i)) this.LogMsg("F" + i + " HOLD start!");
			if (this.printHoldRelease && this.zone.JustReleasedLong(i)) this.LogMsg("F" + i + " HOLD RELEAED!!");


			if (this.printRelease && this.zone.JustReleasedRaw(i))
				{
				float sm = this.zone.GetSwipeVector(i).magnitude * CFScreen.invDpcm;
				float t = this.zone.GetReleasedDuration(i);

				this.LogMsg("F" + i + " released! Moved: " + this.zone.SwipeActive(i)  + " " + sm.ToString("F3") + "cm" +
					 ((sm > this.zone.customThresh.tapMoveThreshCm) ? "(!T)" : "") + " " +
					"dur: " + t.ToString("F3") + ((t > this.zone.customThresh.tapMaxDur) ? "(!T)" : "")); 
				this.LogMsg("");
				}

			if (this.printReleaseEx && this.zone.JustReleasedNormal(i)) this.LogMsg("F" + i + " released Normal!!");
			if (this.printReleaseEx && this.zone.JustReleasedLong(i)) this.LogMsg("F" + i + " released Long!!");
			if (this.printLongTap && this.zone.JustLongTapped(i)) this.LogMsg("F" + i + " LONG TAPPED!!");


			if (this.printSwipeStart && this.zone.SwipeJustActivated(i)) this.LogMsg("F" + i + " swiped!");

			for (int ti = 1; ti <= 4; ++ti)
				{
				if (this.printTaps && this.zone.JustTapped(i, ti))
					this.LogMsg("F" + i + " TAPPED " + ti + " times!");
				}
			}
	
		if (this.zone.PinchJustActivated()) this.LogMsg("F" + 2 + " PINCHED!");
		if (this.zone.TwistJustActivated()) this.LogMsg("F" + 2 + " TWISTED!");

		this.dbgMsg += "Pinch: " + this.zone.GetPinchScale() + "\n";
		this.dbgMsg += "Twist: " + this.zone.GetTwist() + "\n";


		

		}


	// -----------------
	private void LogMsg(string msg)	
		{
		this.dbgConsole.Log("[" + Time.frameCount + "] " + msg);
		}

		
	// ---------------
	public static void Log(string msg)
		{
		if (mInst != null)
			mInst.LogMsg("\t" + msg);
		}

	// -------------------
	void OnGUI()
		{

		GUILayout.Box(this.dbgMsg);
			
		GUILayout.BeginHorizontal ();

		this.displayConsole = GUILayout.Toggle(this.displayConsole, "DISPLAY CONSOLE");

		if (GUILayout.Button("CLEAR")) 
			this.dbgConsole.Clear();

		GUILayout.EndHorizontal();
			
		if (this.displayConsole)
			this.dbgConsole.DrawGUI();
	
		}

	}
}
