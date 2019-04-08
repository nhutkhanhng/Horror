using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CheckRequireBeforeLoading : MonoBehaviour
{

	public GameObject DetectController;

	public ControllerStt isControllerConnected;



	// Use this for initialization

	// Update is called once per frame
	void Update()
	{
		if (OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote))
		{
			isControllerConnected = ControllerStt.connected;
		}
		else if (OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote))
		{
			isControllerConnected = ControllerStt.connected;
		}
		else isControllerConnected = ControllerStt.no_connected;

		if (isControllerConnected == ControllerStt.connected)
		{
			DetectController.SetActive(false);
			
			Application.LoadLevel("HorrorUnderground");
		}
		else
		{
			DetectController.SetActive(true);

		}
	}
}

