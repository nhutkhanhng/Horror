using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieWalkSound : MonoBehaviour
{


	public AudioSource source;

	public AudioClip walk;
	public AudioClip run;


	public void Walk()
	{
		source.PlayOneShot(walk);
	}
	
	public void Run()
	{
		source.PlayOneShot(run);
	}
}
