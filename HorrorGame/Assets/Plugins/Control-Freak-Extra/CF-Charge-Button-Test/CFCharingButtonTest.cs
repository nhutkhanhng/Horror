using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class CFCharingButtonTest : MonoBehaviour
	{

	public CFChargingButton shootButton		= new CFChargingButton("Fire1", 1.0f, true);
	public CFChargingButton jumpButton 		= new CFChargingButton("Jump", 1.0f, false);
	
	
	public UnityEngine.UI.Text			shootLabel;
	public UnityEngine.UI.Slider 		shootPowerSlider;

	public UnityEngine.UI.Text			jumpLabel;
	public UnityEngine.UI.Slider 		jumpPowerSlider;

	public AudioClip shootSound;
	public AudioClip jumpSound;
	
	public AudioSource audioSource;


	// ---------------------
	void Start ()
		{
		this.audioSource = this.GetComponent<AudioSource>();
		}

	//void OnGUI() { GUILayout.Label("Fire Axis: " + CFInput.GetAxis("Fire1")); }

	// ----------------------
	void Update ()
		{
		// Update buttons...

		this.shootButton.Update();
		this.jumpButton.Update();

		
		// Synchronize sliders...

		if ((this.jumpPowerSlider != null) && (this.jumpLabel != null))
			{
			if (!this.jumpButton.IsCharging())
				{
				this.jumpLabel.text = "Jump";
				this.jumpLabel.color = Color.white;
				this.jumpPowerSlider.value = 0;
				}
			else
				{
				this.jumpLabel.text = "Jump is charging!";
				this.jumpLabel.color = Color.Lerp(Color.yellow, Color.red, this.jumpButton.GetCurPower());
				this.jumpPowerSlider.value = this.jumpButton.GetCurPower();
				}
				
			}

		
		if ((this.shootPowerSlider != null) && (this.shootLabel != null))
			{
			if (!this.shootButton.IsCharging())
				{
				this.shootLabel.text = "Shoot is neutral...";
				this.shootLabel.color = Color.white;
				this.shootPowerSlider.value = 0;
				}
			else
				{
				this.shootLabel.text = "Shoot is charging!";
				this.shootLabel.color = Color.Lerp(Color.yellow, Color.red, this.shootButton.GetCurPower());
				this.shootPowerSlider.value = this.shootButton.GetCurPower();
				}
				
			}



		// When the shoot button has been released...

		if (this.shootButton.JustReleased())
			{
			// Play the shoot sound effect with volume scaled by the charged power...

			this.audioSource.PlayOneShot(this.shootSound, this.shootButton.GetPowerAtRelease());
			}
		
		// When the jump button has been released...

		if (this.jumpButton.JustReleased())
			{
			// Play the shoot sound effect with volume scaled by the charged power...

			this.audioSource.PlayOneShot(this.jumpSound, this.jumpButton.GetPowerAtRelease());
			}
		
		}
}

