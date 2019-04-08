using UnityEngine;
namespace ControlFreak2
{
public class SloMoButton : MonoBehaviour 
	{
	public float slomoScale = 0.2f;
	private bool isOn;

	public Rect screenRect = new Rect(0.9f, 0, 0.1f, 0.1f);


	// ------------------
	void OnDisable() 
		{
		Time.timeScale = 1;
		}

	// -------------------
	void OnGUI()
		{
		Rect r = new Rect(this.screenRect.x * Screen.width, this.screenRect.y * Screen.height, this.screenRect.width * Screen.width, this.screenRect.height * Screen.height);
		if (GUI.Button(r, this.isOn ? "SLOMO" : "100%"))
			{
			this.isOn = !this.isOn;
			Time.timeScale = (this.isOn ? this.slomoScale : 1);
			}
		}

	}
}
