using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyInputVR.Core;
using UnityEngine.UI;


//
///trigger button click: fire/flash
/// trigger button long click: reload
/// back button long click: invent
/// back button: interact
/// 
/// 
public enum GoInput
{
	None,
	BackClick,
	BackLong,
	TriggerClick,
	TriggerLong,
	TouchClick,
	TouchLongClick,
}
namespace EasyInputVR.Misc
{
	public class OculusGoInteractManager: MonoBehaviour
	{

		private static OculusGoInteractManager instance;

		public static OculusGoInteractManager Instance
		{
			get { return instance; }
		}

		private ButtonClick Click;
		//private ButtonClick LongClick;

		public GoInput goInput;

		void OnEnable()
		{
			EasyInputHelper.On_LongClickStart += OnLongClickStart;
			EasyInputHelper.On_ClickEnd += OnClickEnd;

			EasyInputHelper.On_LongClickEnd += OnLongClickEnd;
		}

		void OnDestroy()
		{
			EasyInputHelper.On_LongClickStart -= OnLongClickStart;
			EasyInputHelper.On_ClickEnd -= OnClickEnd;
			
			EasyInputHelper.On_LongClickEnd -= OnLongClickEnd;
		}

		
		
		void Awake()
		{
			if (instance == null)
				instance = this;
			else
			{
				Destroy(gameObject);
			}
		}

		void Start()
		{
			Click = null;
			goInput = GoInput.None;
			//LongClick = null;
		}

		void OnClickEnd(ButtonClick click)
		{
			if (click.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTrigger)
			{
				if (goInput != GoInput.TriggerLong)
				{
					goInput = GoInput.TriggerClick;
					Click = click;
				}
			}
			if (click.button == EasyInputConstants.CONTROLLER_BUTTON.Back)
			{
				if (goInput != GoInput.BackLong)
				{
					goInput = GoInput.BackClick;
					Click = click;
				}
			}

			if (click.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTouchClick)
			{
				goInput = GoInput.TouchClick;
				Click = click;
			}
		}
		
		void OnLongClickEnd(ButtonClick click)
		{
			Click = null;
			goInput = GoInput.None;
		}

		void OnLongClickStart(ButtonClick click)
		{
			if (click.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTrigger)
			{
				goInput = GoInput.TriggerLong;
				Click = click;

			}
			
			if (click.button == EasyInputConstants.CONTROLLER_BUTTON.Back)
			{
					goInput = GoInput.BackLong;
					Click = click;
			}
			
			if (click.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTouchClick)
			{
				goInput = GoInput.TouchLongClick;
				Click = click;
			}
		}



		/// <summary>
		/// // Click event
		/// </summary>
		/// <param name="click"></param>
		/// <returns></returns>

		public void ResetInput()
		{
			Click = null;
			goInput = GoInput.None;
		}
		
		public bool TriggerClickInput()
		{
			if (goInput == GoInput.TriggerClick)
			{
				Click = null;
				goInput = GoInput.None;
				return true;
			}
			return false;
		}
		
		public bool TriggerLongClickInput()
		{
			if (goInput == GoInput.TriggerLong)
			{
				Click = null;
				goInput = GoInput.None;
				return true;
			}
			
			return false;
		}

		public bool BackClickInput()
		{
			if (goInput == GoInput.BackClick)
			{
				Click = null;
				goInput = GoInput.None;
				return true;
			}

			return false;
		}
		
		public bool BackLongClickInput()
		{
			if (goInput == GoInput.BackLong)
			{
				Click = null;
				goInput = GoInput.None;
				return true;
			}

			return false;
		}

		public bool TouchClickInput()
		{
			if (goInput == GoInput.TouchClick)
			{
				Click = null;
				goInput = GoInput.None;
				return true;
			}

			return false;
		}
		public bool TouchLongClickInput()
		{
			if (goInput == GoInput.TouchLongClick)
			{
				Click = null;
				goInput = GoInput.None;
				return true;
			}

			return false;
		}
	}
}
