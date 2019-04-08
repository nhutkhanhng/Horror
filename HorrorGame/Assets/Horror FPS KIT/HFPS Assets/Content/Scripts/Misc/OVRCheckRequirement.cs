using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public enum ControllerStt
{
	connected,
	no_connected,
}

public class OVRCheckRequirement : MonoBehaviour
{

	public GameObject GameUI;
	public GameObject DetectController;
	public GameObject LaserPointer;

	public ControllerStt isControllerConnected;

	public Transform Parent;

	public Transform SubParent;

	public Transform Canvas;
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
			LaserPointer.SetActive(true);
			//GameUI.SetActive(true);
			DetectController.SetActive(false);
			
			if(Canvas != null)
				Canvas.SetParent(Parent);
		}
		else
		{
			LaserPointer.SetActive(false);
			//GameUI.SetActive(false);
			DetectController.SetActive(true);
			
			if(Canvas != null)
				Canvas.SetParent(SubParent);
		}
	}
}
