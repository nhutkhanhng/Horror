using System.Collections;
using System.Collections.Generic;
using EasyInputVR.Misc;
using UnityEngine;
using UnityEngine.AI;

public class MoveTrigger : MonoBehaviour
{

	public NavMeshAgent mNavMeshAgent;

	public bool mWalking = false;
	
	private GameObject mMoveParticle;
	private bool isCorrect;
	
	private void Start()
	{
		mMoveParticle = this.gameObject.transform.GetChild(0).gameObject;
	}

	void Update()
	{

		if (mNavMeshAgent.remainingDistance <= mNavMeshAgent.stoppingDistance)
		{
			mNavMeshAgent.isStopped = true;
			mWalking = false;
		}
		else
		{
			mNavMeshAgent.isStopped = false;
			mWalking = true;
		}
		

	}

	public void MoveTo(Transform destination)
	{
		OculusGoInteractManager.Instance.ResetInput();
		if (!mWalking)
		{
			mMoveParticle.SetActive(false);
			mNavMeshAgent.destination = destination.position;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		mMoveParticle.SetActive(false);
	}

	private void OnTriggerExit(Collider other)
	{
		mMoveParticle.SetActive(true);
	}
}

