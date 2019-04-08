using UnityEngine;

public class ControlFreakKeyboardToggle : MonoBehaviour 
	{
	public bool		turnOffControlFreak;
	public KeyCode	toggleHotkey 			= KeyCode.F12;

	
#if UNITY_EDITOR

	private TouchController activeCtrl; 
	
	// -----------------
	void OnEnable()
		{
		GameObject.DontDestroyOnLoad(this);
		}


	// -----------------
	void Update()
		{
		if ((this.toggleHotkey != KeyCode.None) && Input.GetKeyDown(this.toggleHotkey))
			this.turnOffControlFreak = !this.turnOffControlFreak;

		if (this.turnOffControlFreak)
			{
			if (CFInput.ctrl != null)
				{
				this.activeCtrl = CFInput.ctrl;
				CFInput.ctrl = null;
				}
			}
		else
			{
			if (CFInput.ctrl == null)
				{
				if ((this.activeCtrl != null) && this.activeCtrl)
					CFInput.ctrl = this.activeCtrl;				 
				}
			else
				this.activeCtrl = CFInput.ctrl;
			}
		}

#endif
	}
