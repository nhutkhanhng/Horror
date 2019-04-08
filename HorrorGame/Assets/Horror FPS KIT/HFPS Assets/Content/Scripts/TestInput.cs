using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyInputVR.Core;
using EasyInputVR.Misc;
using UnityEngine.UI;

public class TestInput : MonoBehaviour
{

	private static TestInput instance;

	public static TestInput Instance
	{
		get { return instance; }
	}

	public Text valueX;
	public Text valueY;
	public Text TouchTracking;

	public Text playerValueX;
	public Text playerValueY;
	public Text playerTouchTracking;
	public Text playerMoveDirection;


	public PlayerController playerController;
	
	private float ControllerInputX;
	private float ControllerInputY;
	private bool touching;


	public Text BackClick;
	public Text BackLongClick;
	public Text TriggerClick;
	public Text TriggerLongClick;
	public Text BatteryCount;

	public Text InteractCount;

	public Text RaycastObjName;

	public int dem;
	public int _dem;


	public GameObject Door;
	public  bool isOpen;
	void OnEnable()
	{
		EasyInputHelper.On_Touch += localTouch;
		EasyInputHelper.On_TouchEnd += localTouchEnd;
	}

	void OnDestroy()
	{
		EasyInputHelper.On_Touch -= localTouch;
		EasyInputHelper.On_TouchEnd -= localTouchEnd;
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	void Start()
	{
		BackClick.text = null;
		BackLongClick.text = null;
		TriggerClick.text = null;
		TriggerLongClick.text = null;
	}

	// Update is called once per frame
	private void Update()
	{
		if (!touching)
		{
			ControllerInputX = ControllerInputY = 0;
		}

		if (isOpen)
		{
			Door.SendMessage("UseObject", SendMessageOptions.DontRequireReceiver);
			isOpen = !isOpen;
		}

		valueX.text = ControllerInputX.ToString();
		valueY.text = ControllerInputY.ToString();
		TouchTracking.text = touching.ToString();


		playerValueX.text = playerController.ControllerInputX.ToString();
		playerValueY.text = playerController.ControllerInputY.ToString();
		playerTouchTracking.text = playerController.touching.ToString();
		playerMoveDirection.text = playerController.moveDirection.ToString();

		if (OculusGoInteractManager.Instance.BackClickInput())
			BackClick.text = "true";
		else BackClick.text = null;

		if (OculusGoInteractManager.Instance.BackLongClickInput())
			BackLongClick.text = "true";
		else BackLongClick.text = null;

		if (OculusGoInteractManager.Instance.TriggerClickInput())
			TriggerClick.text = "true";
		else TriggerClick.text = null;

		if (OculusGoInteractManager.Instance.TriggerLongClickInput())
			TriggerLongClick.text = "true";
		else TriggerLongClick.text = null;
		
		BatteryCount.text = Inventory.Instance.GetItemAmount(1).ToString();

		InteractCount.text = _dem + "-" + dem;

		RaycastObjName.text = InteractManager.Instance.RayToLayerInteract().ToString() +"-" + InteractManager.Instance.isMoveTrigger +"-"+InteractManager.Instance.useTexture ;
	}


	void localTouch(InputTouch touch)
	{
		touching = true;
		
		ControllerInputX = touch.currentTouchPosition.x;
		ControllerInputY = touch.currentTouchPosition.y;
	}

	void localTouchEnd(InputTouch touch)
	{
		touching = false;
	}

}
