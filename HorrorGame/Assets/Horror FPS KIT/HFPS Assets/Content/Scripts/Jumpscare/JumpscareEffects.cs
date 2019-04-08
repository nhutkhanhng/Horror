using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class JumpscareEffects : MonoBehaviour {

	private GameObject CameraShake;

	public float scareChromaticAberration;
	public float scareVignette;
	public float lerpScareSpeed;

	private float LerpSpeed = 1f;
	private float ScareWaitSec;
	private float defaultVolume;

	public AudioSource PlayerBreath;
	private bool isFeelingBetter;
	private bool Effects;


	void Start () {
		CameraShake = Camera.main.transform.root.GetChild (0).gameObject;
//		PlayerBreath = transform.root.GetChild (1).transform.GetChild (1).gameObject.GetComponent<AudioSource> ();
		defaultVolume = PlayerBreath.volume;
	}

	void Update()
	{
		if (isFeelingBetter) {
			if (PlayerBreath.volume > 0.01f) {
				PlayerBreath.volume = Mathf.Lerp (PlayerBreath.volume, 0f, LerpSpeed * Time.deltaTime);
			}
			if(PlayerBreath.volume <= 0.01f){
				PlayerBreath.Stop ();
				StopCoroutine (ScareBreath ());
				StopCoroutine (WaitEffects ());
				isFeelingBetter = false;
			}
		}
	}

	public void Scare(float sec)
	{
		CameraShake.GetComponent<Animation> ().Play ("CameraShake");
		ScareWaitSec = sec;
		Effects = true;
		StartCoroutine (ScareBreath ());
		StartCoroutine (WaitEffects ());
	}

	IEnumerator WaitEffects()
	{
		yield return new WaitForSeconds (5f);
		Effects = false;
	}

	IEnumerator ScareBreath()
	{
		PlayerBreath.volume = defaultVolume;
		PlayerBreath.Play();
		yield return new WaitForSeconds (ScareWaitSec);
		isFeelingBetter = true;
	}
}
