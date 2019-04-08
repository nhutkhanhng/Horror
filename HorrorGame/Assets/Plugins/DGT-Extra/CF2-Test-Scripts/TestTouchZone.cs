using UnityEngine;

using ControlFreak2;
using ControlFreak2.Internal;


namespace ControlFreak2Test
{
public class TestTouchZone : MonoBehaviour 
	{
	public SuperTouchZone zone;

	// -----------------
	void Start()
		{
		if (this.zone == null) 
			this.zone = this.GetComponent<ControlFreak2.SuperTouchZone>();
		}
		

	// -------------------
	void OnGUI()
		{
		if (this.zone == null)
			return;
			/*
		TouchGestureState state = this.zone.GetFingerState(0);

		GUILayout.Box("Zone [" + this.zone.name + "] " +
			"DIR: " + state.GetDir4() + " : " + state.GetDir8() + " Press:" + state.Pressed() + 
			" V:" + (state.GetCurPos()) + "Scroll: " + state.GetScroll());
		
		Vector2 joyCen = state.GetJoyCenter();
		float joyRad = this.zone.customThresh.dragJoyRadPx;	
			
		Rect r = new Rect(joyCen.x - joyRad, ((Screen.height - joyCen.y) - joyRad), joyRad*2, joyRad*2);	

		GUI.Box(r, "X");


		GUI.color = Color.red;	
		for (int i = -2; i <= 2; ++i)
			{
			Vector2 sp = state.GetStartPos();
			sp.x += (state.thresh.scrollThreshPx * (float)i);
			sp.y = Screen.height - sp.y;

			float rad =  state.thresh.scrollMagnetFactor * state.thresh.scrollThreshPx;
			
			GUI.Box(new Rect(sp.x - rad, sp.y, rad*2, 10), i.ToString());
			}
*/
		}
	
	}
}
