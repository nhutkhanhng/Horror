using UnityEngine;

	
[System.Serializable]
public class CFChargingButton
	{	
	[Tooltip("Name of the button controlling this charging button.")]
	public string	buttonName;

	[Tooltip("Time in seconds needed to charge this button to the max.")]
	public float	maxChargeTime;

	[Tooltip("Auto release when the power reach maximum.")]
	public bool		autoReleaseWhenMaxed;
	

	private float	powerCur,
					powerAtRelease;
	private float	timeSincePress;
	private bool	pressedCur,
					pressedPrev;
	private bool	chargingCur,
					chargingPrev;
	
	
	
	// -------------------
	public CFChargingButton()
		{	
		this.buttonName				= "Fire1";
		this.maxChargeTime			= 1;
		this.autoReleaseWhenMaxed	= true;
		this.Reset();
		}

	// -------------------
	public CFChargingButton(string buttonName, float maxTime, bool autoRelease)
		{	
		this.buttonName				= buttonName;
		this.maxChargeTime			= maxTime;
		this.autoReleaseWhenMaxed	= autoRelease;
		this.Reset();
		}

	// ---------------------
	public float	GetCurPower()		{ return this.powerCur; }
	public float	GetPowerAtRelease()	{ return this.powerAtRelease; }
	public bool		IsCharging()		{ return this.chargingCur; }
	public bool		JustReleased()		{ return (!this.chargingCur && this.chargingPrev); }
	public bool		JustPressed()		{ return (this.chargingCur && !this.chargingPrev); }

	private bool GetButton()		{ return this.pressedCur; }
	private bool GetButtonDown()	{ return (this.pressedCur && !this.pressedPrev); }
	private bool GetButtonUp()	{ return (!this.pressedCur && this.pressedPrev); }
	
	// -------------------
	public void Reset()
		{
		this.timeSincePress	= 0;
		this.powerCur		= 0;
		this.powerAtRelease = 0;
		this.pressedCur		= false;
		this.pressedPrev	= false;
		this.chargingPrev 	= false;
		this.chargingCur	= false;
		}


	// ------------------
	public void Update()
		{
		this.chargingPrev = this.chargingCur;
		this.pressedPrev = this.pressedCur;

		this.pressedCur = CFInput.GetButton(this.buttonName);
			
		// Start charging...

		if (this.GetButtonDown())
			{
			this.chargingCur = true;
			this.timeSincePress = 0;
			this.powerCur = 0;
			}
		
		if (this.chargingCur)
			{
			this.timeSincePress += Time.deltaTime;
			if (this.timeSincePress >= this.maxChargeTime)
				{
				this.powerCur = 1.0f;
				
				if (this.autoReleaseWhenMaxed)
					this.chargingCur = false;				
				}
			else
				{
				this.powerCur = (this.timeSincePress / this.maxChargeTime);
				}

			if (this.GetButtonUp())
				this.chargingCur = false;
		

			if (!this.chargingCur)
				{
				this.powerAtRelease = this.powerCur;
				}
			}
		else
			{
			this.powerCur = 0;
			}
		
		}
		


	}
