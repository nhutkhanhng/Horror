#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using ControlFreak2;

namespace ControlFreak2Editor
{
public static class LegacyControllerConverter 
	{
	private const string 
		DIALOG_TITLE = "Control Freak Legacy Controller Converter";


	// --------------------
	[MenuItem("Control Freak 2/CF 1.x Migration Tools/Convert Legacy Controller")]
	static public void MenuItemConvertSelected()
		{	
		TouchController ctrl = GetSelectedTouchController();

		if (ctrl == null)
			EditorUtility.DisplayDialog(DIALOG_TITLE, "Select Control Freak 1.x controller before using this tool!", "OK");
		//else if (IsPrefab(ctrl))
		//	EditorUtility.DisplayDialog(DIALOG_TITLE, "This tool  can't be used on prefabs!", "OK");
		else
			ConvertController(ctrl);
		}

	// --------------------
	[MenuItem("Control Freak 2/CF 1.x Migration Tools/Convert Legacy Controller", true)]
	static private bool MenuItemConvertSelectedValidation()
		{
		return (GetSelectedTouchController() != null); 
		}
	
	// ------------------------
	static private TouchController GetSelectedTouchController()
		{
		if ((Selection.transforms == null) || (Selection.transforms.Length == 0))
			return null;

		for (int i = 0; i < Selection.transforms.Length; ++i)
			{
			TouchController c = Selection.transforms[i].GetComponentInChildren<TouchController>();
			if ((c != null) && !IsPrefab(c))
				{
				return c;
				}
			}

		return null;
		}

	// --------------
	static private InputRig ConvertController(TouchController ctrl)
		{
		if (ctrl == null) 
			return null;

		
		ctrl.InitController();


		Vector2 
			prioRange = GetTouchControllerPriorityRange(ctrl);

		InputRig 
			rig = TouchControlWizardUtils.CreateRig("CF2-" + ctrl.name);

		TouchControlPanel 
			panel = TouchControlWizardUtils.CreatePanelOnCanvas(rig, "");

		Canvas 
			canvas = panel.GetComponentInParent<Canvas>();

		canvas.SendMessage("Update");

		// Recreate joysticks...

		if ((ctrl.sticks != null) && (ctrl.sticks.Length > 0))
			{
			for (int i = 0; i < ctrl.sticks.Length; ++i)
				{
				TouchStick srcStick = ctrl.sticks[i];
				if (srcStick == null)
					continue;

				ControlFreak2.TouchJoystick	
					dstStick = null;

				if (srcStick.dynamicMode)
					{
					dstStick = (ControlFreak2.TouchJoystick)TouchControlWizardUtils.CreateDynamicControlWithRegion(typeof(ControlFreak2.TouchJoystick), panel, 
						srcStick.name, 0.1f, FixGUIRect(srcStick.dynamicRegion), 
						PriorityToDepth(srcStick.prio, prioRange), PriorityToDepth(srcStick.dynamicRegionPrio, prioRange)); 

					CopyTouchControlPosition(dstStick, ctrl, srcStick);
					}
				else
					{
					dstStick = (ControlFreak2.TouchJoystick)TouchControlWizardUtils.CreateStaticControl(typeof(ControlFreak2.TouchJoystick), panel, panel.transform, 
						srcStick.name, GetLayoutBoxAnchorPos(ctrl, srcStick.layoutBoxId), Vector2.zero, 0.1f, PriorityToDepth(srcStick.prio, prioRange));

					CopyTouchControlPosition(dstStick, ctrl, srcStick);
					}

				// Transfer general properties...

				TransferBasicProperties(srcStick, dstStick);

					
				dstStick.shape			= TouchControl.Shape.Circle;
				dstStick.config.blockX	= srcStick.disableX;
				dstStick.config.blockY	= srcStick.disableY;

				dstStick.fadeOutDelay		= srcStick.dynamicFadeOutDelay;
				dstStick.fadeOutDuration	= srcStick.dynamicFadeOutDuration;
				dstStick.fadeOutTargetAlpha	= 0;
				dstStick.fadeOutWhenReleased = srcStick.dynamicMode;


				// Transfer Bindings...

				dstStick.pressBinding.Clear();
				

				// Transfer axes...

				if (srcStick.enableGetAxis)	
					{
					if (!string.IsNullOrEmpty(srcStick.axisHorzName))
						{
						dstStick.joyStateBinding.horzAxisBinding.Clear();
						dstStick.joyStateBinding.horzAxisBinding.AddTarget().SetSingleAxis(srcStick.axisHorzName, srcStick.axisHorzFlip);
						dstStick.joyStateBinding.horzAxisBinding.Enable();
						}

					if (!string.IsNullOrEmpty(srcStick.axisVertName))
						{
						dstStick.joyStateBinding.vertAxisBinding.Clear();
						dstStick.joyStateBinding.vertAxisBinding.AddTarget().SetSingleAxis(srcStick.axisVertName, srcStick.axisVertFlip);
						dstStick.joyStateBinding.vertAxisBinding.Enable();
						}
					}

				// Pressed button...

				if (srcStick.enableGetButton)
					{
					if (!string.IsNullOrEmpty(srcStick.getButtonName))
						{
						dstStick.pressBinding.ClearAxes();
						dstStick.pressBinding.AddAxis().SetAxis(srcStick.getButtonName, true);		
						dstStick.pressBinding.Enable();
						}
					}

				if (srcStick.enableGetKey)
					{
					// Pressed key...

					if ((srcStick.getKeyCodePress != KeyCode.None) || (srcStick.getKeyCodePressAlt != KeyCode.None))
						{
						dstStick.pressBinding.Enable();

						if (srcStick.getKeyCodePress	!= KeyCode.None) dstStick.pressBinding.AddKey(srcStick.getKeyCodePress);
						if (srcStick.getKeyCodePressAlt	!= KeyCode.None) dstStick.pressBinding.AddKey(srcStick.getKeyCodePressAlt);
						}

					dstStick.joyStateBinding.dirBinding.bindDiagonals = false;

					// Digital Up keys...

					if ((srcStick.getKeyCodeUp != KeyCode.None) || (srcStick.getKeyCodeUpAlt != KeyCode.None))
						{
						dstStick.joyStateBinding.dirBinding.dirBindingU.Clear();
						dstStick.joyStateBinding.dirBinding.dirBindingU.Enable();
						if (srcStick.getKeyCodeUp	!= KeyCode.None) dstStick.joyStateBinding.dirBinding.dirBindingU.AddKey(srcStick.getKeyCodeUp);
						if (srcStick.getKeyCodeUpAlt!= KeyCode.None) dstStick.joyStateBinding.dirBinding.dirBindingU.AddKey(srcStick.getKeyCodeUpAlt);
						}

					// Digital Down keys...

					if ((srcStick.getKeyCodeDown != KeyCode.None) || (srcStick.getKeyCodeDownAlt != KeyCode.None))
						{
						dstStick.joyStateBinding.dirBinding.dirBindingD.Clear();
						dstStick.joyStateBinding.dirBinding.dirBindingD.Enable();
						if (srcStick.getKeyCodeDown		!= KeyCode.None) dstStick.joyStateBinding.dirBinding.dirBindingD.AddKey(srcStick.getKeyCodeDown);
						if (srcStick.getKeyCodeDownAlt	!= KeyCode.None) dstStick.joyStateBinding.dirBinding.dirBindingD.AddKey(srcStick.getKeyCodeDownAlt);
						}

					// Digital Right keys...

					if ((srcStick.getKeyCodeRight != KeyCode.None) || (srcStick.getKeyCodeRightAlt != KeyCode.None))
						{
						dstStick.joyStateBinding.dirBinding.dirBindingR.Clear();
						dstStick.joyStateBinding.dirBinding.dirBindingR.Enable();
						if (srcStick.getKeyCodeRight	!= KeyCode.None) dstStick.joyStateBinding.dirBinding.dirBindingR.AddKey(srcStick.getKeyCodeRight);
						if (srcStick.getKeyCodeRightAlt	!= KeyCode.None) dstStick.joyStateBinding.dirBinding.dirBindingR.AddKey(srcStick.getKeyCodeRightAlt);
						}

					// Digital Left keys...

					if ((srcStick.getKeyCodeLeft != KeyCode.None) || (srcStick.getKeyCodeLeftAlt != KeyCode.None))
						{
						dstStick.joyStateBinding.dirBinding.dirBindingL.Clear();
						dstStick.joyStateBinding.dirBinding.dirBindingL.Enable();
						if (srcStick.getKeyCodeLeft		!= KeyCode.None) dstStick.joyStateBinding.dirBinding.dirBindingL.AddKey(srcStick.getKeyCodeLeft);
						if (srcStick.getKeyCodeLeftAlt	!= KeyCode.None) dstStick.joyStateBinding.dirBinding.dirBindingL.AddKey(srcStick.getKeyCodeLeftAlt);
						}
					}



				// Create animators...

				TouchJoystickSpriteAnimator 	
					baseAnimator = TouchControlWizardUtils.CreateTouchJoystickSimpleAnimator(dstStick, "-Base", 
						TouchControlWizardUtils.GetDefaultAnalogJoyBaseSprite(srcStick.name), 1.0f, 0);

				//baseAnimator.animateScale				= true;
				baseAnimator.animateTransl				= false;
				baseAnimator.spriteNeutral.color		= srcStick.releasedBaseColor;
				baseAnimator.spriteNeutralPressed.color = srcStick.pressedBaseColor;
				baseAnimator.spriteMode 				= TouchJoystickSpriteAnimator.SpriteMode.Simple;
				baseAnimator.spriteNeutralPressed.scale	= srcStick.pressedBaseScale;
				baseAnimator.spriteNeutralPressed.enabled = true;


				TouchJoystickSpriteAnimator 	
					hatAnimator = TouchControlWizardUtils.CreateTouchJoystickSimpleAnimator(dstStick, "-Hat", 
						TouchControlWizardUtils.GetDefaultAnalogJoyBaseSprite(srcStick.name), srcStick.releasedHatScale, srcStick.hatMoveScale);

				//hatAnimator.animateScale				= true;
				hatAnimator.animateTransl				= true;
				hatAnimator.spriteNeutral.color			= srcStick.releasedHatColor;
				hatAnimator.spriteNeutralPressed.color	= srcStick.pressedHatColor;
				hatAnimator.spriteMode 					= TouchJoystickSpriteAnimator.SpriteMode.Simple;
				hatAnimator.spriteNeutralPressed.scale	= srcStick.pressedHatScale;
				hatAnimator.spriteNeutralPressed.enabled= true;
				
					
				}
			}

		// Recreate touch zones...

		if ((ctrl.touchZones != null) && (ctrl.touchZones.Length > 0))
			{
			for (int i = 0; i < ctrl.touchZones.Length; ++i)
				{
				TouchZone srcZone = ctrl.touchZones[i];
				if (srcZone == null)
					continue;

				ControlFreak2.TouchControl	
					dstControl = null;
				ControlFreak2.SuperTouchZone
					dstZone = null;
				ControlFreak2.TouchButton
					dstButton = null;

				System.Type 
					dstType = (IsSimpleTouchZone(srcZone) ? typeof(ControlFreak2.TouchButton) : typeof(ControlFreak2.SuperTouchZone));
	
				// Create relative region...

				if (srcZone.shape == TouchController.ControlShape.SCREEN_REGION)
					{
					dstControl = TouchControlWizardUtils.CreateStretchyControl(dstType, panel, panel.transform, 
						srcZone.name, FixGUIRect(srcZone.regionRect), PriorityToDepth(srcZone.prio, prioRange));
	
					dstControl.shape = TouchControl.Shape.Rectangle;
					}

				// Create static size control...
				else
					{
					//dstControl = TouchControlWizardUtils.CreateStaticControl(dstType, panel, panel.transform, 
					//	srcZone.name, GetAnchorPos(FixGUIRect(srcZone.regionRect), PriorityToDepth(srcZone.prio, prioRange));
	
					dstControl = TouchControlWizardUtils.CreateStaticControl(dstType, panel, panel.transform, 
						srcZone.name, GetLayoutBoxAnchorPos(ctrl, srcZone.layoutBoxId), Vector2.zero, 10, PriorityToDepth(srcZone.prio, prioRange));

					dstControl.shape = (srcZone.shape == TouchController.ControlShape.CIRCLE) ? TouchControl.Shape.Ellipse : TouchControl.Shape.Rectangle;

					CopyTouchControlPosition(dstControl, ctrl, srcZone);
					}

				// Transfer general properties...

				dstZone		= dstControl as SuperTouchZone;
				dstButton	= dstControl as TouchButton;


				TransferBasicProperties(srcZone, dstControl);


				// Touch-zone-specific properties...

				if (dstZone != null)
					{
					dstZone.maxFingers				= (srcZone.enableSecondFinger ? 2 : 1);
					dstZone.strictMultiTouch		= srcZone.strictTwoFingerStart;

					dstZone.startTwistWhenPinching	= srcZone.startTwistWhenPinching;
					dstZone.startTwistWhenDragging	= srcZone.startTwistWhenDragging;
					dstZone.startPinchWhenTwisting	= srcZone.startPinchWhenTwisting;
					dstZone.startPinchWhenDragging	= srcZone.startPinchWhenDragging;
					dstZone.startDragWhenTwisting	= srcZone.startDragWhenTwisting;
					dstZone.startDragWhenPinching	= srcZone.startDragWhenPinching;
					dstZone.noTwistAfterPinch		= srcZone.noTwistAfterPinch;
					dstZone.noTwistAfterDrag		= srcZone.noTwistAfterDrag;
					dstZone.noPinchAfterTwist		= srcZone.noPinchAfterTwist;
					dstZone.noPinchAfterDrag		= srcZone.noPinchAfterDrag;
					dstZone.noDragAfterTwist		= srcZone.noDragAfterTwist;
					dstZone.noDragAfterPinch		= srcZone.noDragAfterPinch;
					dstZone.freezeTwistWhenTooClose	= srcZone.freezeTwistWhenTooClose;

					switch (srcZone.gestureDetectionOrder)
						{
						case TouchZone.GestureDetectionOrder.DRAG_PINCH_TWIST : dstZone.gestureDetectionOrder = SuperTouchZone.GestureDetectionOrder.SwipePinchTwist; break;
						case TouchZone.GestureDetectionOrder.DRAG_TWIST_PINCH : dstZone.gestureDetectionOrder = SuperTouchZone.GestureDetectionOrder.SwipeTwistPinch; break;
						case TouchZone.GestureDetectionOrder.PINCH_DRAG_TWIST : dstZone.gestureDetectionOrder = SuperTouchZone.GestureDetectionOrder.PinchSwipeTwist; break;
						case TouchZone.GestureDetectionOrder.PINCH_TWIST_DRAG : dstZone.gestureDetectionOrder = SuperTouchZone.GestureDetectionOrder.PinchTwistSwipe; break;
						case TouchZone.GestureDetectionOrder.TWIST_DRAG_PINCH : dstZone.gestureDetectionOrder = SuperTouchZone.GestureDetectionOrder.TwistSwipePinch; break;
						case TouchZone.GestureDetectionOrder.TWIST_PINCH_DRAG : dstZone.gestureDetectionOrder = SuperTouchZone.GestureDetectionOrder.TwistPinchSwipe; break;
						}


					// Bindings...

					ControlFreak2.Internal.TouchGestureStateBinding
						dstBindingOne = dstZone.multiFingerConfigs[0].binding,
						dstBindingTwo = dstZone.multiFingerConfigs[1].binding;

					dstBindingOne.rawPressBinding.Clear();
					dstBindingTwo.rawPressBinding.Clear();
	
					if (srcZone.emulateMouse)
						{
						dstBindingOne.rawPressMousePosBinding.Enable();
						}

					// Swipe deltas...

					if (srcZone.enableGetAxis)
						{
						if (!string.IsNullOrEmpty(srcZone.axisHorzName))
							{
							dstBindingOne.normalPressSwipeHorzAxisBinding.Clear();
							dstBindingOne.normalPressSwipeHorzAxisBinding.Enable();
							dstBindingOne.normalPressSwipeHorzAxisBinding.AddTarget().SetSingleAxis(srcZone.axisHorzName, srcZone.axisHorzFlip);
							}

						if (!string.IsNullOrEmpty(srcZone.axisVertName))
							{
							dstBindingOne.normalPressSwipeVertAxisBinding.Clear();
							dstBindingOne.normalPressSwipeVertAxisBinding.Enable();
							dstBindingOne.normalPressSwipeVertAxisBinding.AddTarget().SetSingleAxis(srcZone.axisVertName, srcZone.axisVertFlip);
							}
						}

					// Pressed buttons...
		
					if (srcZone.enableGetButton)
						{
						if (!string.IsNullOrEmpty(srcZone.getButtonName))
							{
							dstBindingOne.rawPressBinding.Enable();
							dstBindingOne.rawPressBinding.AddAxis().SetAxis(srcZone.getButtonName, true);
							}

						if (!string.IsNullOrEmpty(srcZone.getButtonMultiName))
							{
							dstBindingTwo.rawPressBinding.Enable();
							dstBindingTwo.rawPressBinding.AddAxis().SetAxis(srcZone.getButtonMultiName, true);
							}
						}

					// Pressed keys...
		
					if (srcZone.enableGetKey)
						{
						if ((srcZone.getKeyCode != KeyCode.None) || (srcZone.getKeyCodeAlt != KeyCode.None))
							{
							dstBindingOne.rawPressBinding.Enable();

							if (srcZone.getKeyCode != KeyCode.None) 
								dstBindingOne.rawPressBinding.AddKey(srcZone.getKeyCode);
							if (srcZone.getKeyCodeAlt != KeyCode.None) 
								dstBindingOne.rawPressBinding.AddKey(srcZone.getKeyCodeAlt);
							}

						if ((srcZone.getKeyCodeMulti != KeyCode.None) || (srcZone.getKeyCodeMultiAlt != KeyCode.None))
							{
							dstBindingTwo.rawPressBinding.Enable();

							if (srcZone.getKeyCodeMulti != KeyCode.None) 
								dstBindingTwo.rawPressBinding.AddKey(srcZone.getKeyCodeMulti);
							if (srcZone.getKeyCodeMultiAlt != KeyCode.None) 
								dstBindingTwo.rawPressBinding.AddKey(srcZone.getKeyCodeMultiAlt);
							}
						}


					// Create the animator...
		
					if (!srcZone.disableGui && ((srcZone.pressedImg != null) || (srcZone.releasedImg != null)))
						{
						SuperTouchZoneSpriteAnimator
							zoneAnimator = TouchControlWizardUtils.CreateSuperTouchZoneAnimator(dstZone, "-Sprite", 
								TouchControlWizardUtils.GetDefaultButtonSprite(srcZone.name), 1);
	// TODO : Set sprite mode to simple!!!

						//zoneAnimator.animateScale 			= true;
						zoneAnimator.spriteRawPress.enabled	= true;
						zoneAnimator.spriteRawPress.scale	= srcZone.pressedScale;
						zoneAnimator.spriteRawPress.color	= srcZone.pressedColor;
						zoneAnimator.spriteNeutral.color		= srcZone.releasedColor;			
					
						} 

				
					}


				// Button-specific properties...

				if (dstButton != null)
					{
					// Bindings...
			
					dstButton.pressBinding.Clear();


					// Pressed buttons...
		
					if (srcZone.enableGetButton)
						{
						if (!string.IsNullOrEmpty(srcZone.getButtonName))
							{
							dstButton.pressBinding.Enable();
							dstButton.pressBinding.AddAxis().SetAxis(srcZone.getButtonName, true);
							}
						}

					// Pressed keys...
		
					if (srcZone.enableGetKey)
						{
						if ((srcZone.getKeyCode != KeyCode.None) || (srcZone.getKeyCodeAlt != KeyCode.None))
							{
							dstButton.pressBinding.Enable();

							if (srcZone.getKeyCode != KeyCode.None) 
								dstButton.pressBinding.AddKey(srcZone.getKeyCode);
							if (srcZone.getKeyCodeAlt != KeyCode.None) 
								dstButton.pressBinding.AddKey(srcZone.getKeyCodeAlt);
							}

						}


					// Create the animator...
		
					if (!srcZone.disableGui && ((srcZone.pressedImg != null) || (srcZone.releasedImg != null)))
						{
						TouchButtonSpriteAnimator
							buttonAnimator = TouchControlWizardUtils.CreateButtonAnimator(dstButton, "-Sprite", 
								TouchControlWizardUtils.GetDefaultButtonSprite(srcZone.name), 1);
	
						//buttonAnimator.animateScale 		= true;
						//buttonAnimator.spriteMode			= TouchButtonSpriteAnimator.SpriteMode.SINGLE;
					
						buttonAnimator.spritePressed.enabled = true;
						buttonAnimator.spritePressed.scale	= srcZone.pressedScale;
						buttonAnimator.spritePressed.color	= srcZone.pressedColor;
						buttonAnimator.spriteNeutral.color	= srcZone.releasedColor;						
						} 
	
					}

				}
			}

		Undo.RegisterCreatedObjectUndo(rig, "Convert Legacy Controller");

		return rig;		
		}

	
	// -------------------
	static private bool IsSimpleTouchZone(TouchZone zone)
		{
		return (
			!zone.emulateMouse &&
			!zone.enableSecondFinger &&
			!zone.enableGetAxis);
		}



	// -----------------------
	static private Rect FixGUIRect(Rect r)
		{
		//return new Rect(r.x, (1.0f - r.y), r.width, r.height);
		return new Rect(r.x, (r.y), r.width, r.height);
		}



	// -------------------
	static private void CopyTouchControlPosition(ControlFreak2.TouchControl dstControl, TouchController srcController, TouchableControl srcControl)
		{
		Rect displayRect = (srcControl is TouchZone) ? 
			((TouchZone)srcControl).GetDisplayRect(false) : ((TouchStick)srcControl).GetBaseDisplayRect(false); 
	
		Rect screenRect = new Rect(0, 0, Screen.width, Screen.height); //srcController.GetScreenEmuRect();
		//Rect screenRect = srcController.GetScreenEmuRect();
		
		


//		Canvas 
//			canvas 		= dstControl.canvas;
		RectTransform 
			tr 			= (RectTransform)dstControl.transform,
			canvasTr 	= (RectTransform)dstControl.canvas.transform;


		Rect canvasSpaceRect = new Rect(
			((displayRect.x / screenRect.width) * canvasTr.rect.width) + canvasTr.rect.x, 
			((1.0f - ((displayRect.y + displayRect.height) / screenRect.height)) * canvasTr.rect.height) + canvasTr.rect.y,
			((displayRect.width / screenRect.width) * canvasTr.rect.width), 
			((displayRect.height / screenRect.height) * canvasTr.rect.height));

			//Debug.Log("normal[" + srcControl.name  + "] : " + (		 new Rect(
			//((displayRect.x / screenRect.width)), 
			//(1.0f - ((displayRect.y + displayRect.height) / screenRect.height)),
			//((displayRect.width / screenRect.width) ), 
			//	((displayRect.height / screenRect.height)))) + " raw:" + displayRect + " scrW: " + Screen.width + " scrH: " + Screen.height);

		tr.pivot = new Vector2(0.5f, 0.5f);
			
	
		tr.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, canvasSpaceRect.width);
		tr.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, canvasSpaceRect.height);

	
		Vector3 worldPos = dstControl.transform.position;
	
//Debug.Log("Fix[" + srcControl.name + "] -> [" + dstControl.name + "] src[" + displayRect + "] dst[" + canvasSpaceRect + "] screen[" + screenRect + "] canvasRect: " + canvasTr.rect);

				

		Vector3 centerInWorldSpace = canvasTr.TransformPoint(new Vector3(canvasSpaceRect.center.x, canvasSpaceRect.center.y, 0));

//		worldPos.x = ((canvasTr.rect.x + canvasSpaceRect.center.x) * canvasTr.localScale.x) + canvasTr.position.x ; //+ canvasTr.rect.x ; //+ (canvasSpaceRect.width * 0.5f);
//		worldPos.y = ((canvasTr.rect.y + canvasSpaceRect.center.y - (canvasSpaceRect.height * 1f)) * canvasTr.localScale.y) + canvasTr.position.y;

		worldPos.x = centerInWorldSpace.x;
		worldPos.y = centerInWorldSpace.y;

		dstControl.transform.position = worldPos;		


		}


	// -----------------------
	static private bool IsPrefab(TouchController c)
		{
		return !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(c));
		}



	// ----------------------	
	static private Vector2 GetLayoutBoxAnchorPos(TouchController ctrl, int layoutBox)
		{
		if ((layoutBox >= 0) && (ctrl.layoutBoxes != null) && (layoutBox < ctrl.layoutBoxes.Length))
			{
			switch (ctrl.layoutBoxes[layoutBox].anchor)			
				{
				case TouchController.LayoutAnchor.BOTTOM_CENTER	: return new Vector2(0.5f, 0);
				case TouchController.LayoutAnchor.BOTTOM_LEFT	: return new Vector2(0.0f, 0);
				case TouchController.LayoutAnchor.BOTTOM_RIGHT	: return new Vector2(1.0f, 0);
				case TouchController.LayoutAnchor.MID_CENTER	: return new Vector2(0.5f, 0.5f);
				case TouchController.LayoutAnchor.MID_LEFT		: return new Vector2(0.0f, 0.5f);
				case TouchController.LayoutAnchor.MID_RIGHT		: return new Vector2(1.0f, 0.5f);
				case TouchController.LayoutAnchor.TOP_CENTER	: return new Vector2(0.5f, 1);
				case TouchController.LayoutAnchor.TOP_LEFT		: return new Vector2(0.0f, 1);
				case TouchController.LayoutAnchor.TOP_RIGHT		: return new Vector2(1.0f, 1);
				}
			}

		return new Vector2(1, 0);
		}
	
	

	// --------------------
	static private void TransferBasicProperties(TouchableControl src, ControlFreak2.TouchControl dst)	
		{
		dst.dontAcceptSharedTouches = !src.acceptSharedTouches;		

		TouchZone zone = src as TouchZone;
		if (zone != null)
			dst.shareTouch = zone.nonExclusiveTouches;
		}


	// ---------------------
	static private float PriorityToDepth(int prio, Vector2 prioRange)
		{
		if (prioRange.x >= prioRange.y)
			return 50;

		return Mathf.Lerp(100, 10, (((float)prio - prioRange.x) / (prioRange.y - prioRange.x)));
		}


	// ------------------
	static private Vector2 GetTouchControllerPriorityRange(TouchController ctrl)
		{
		Vector2 prioRange = Vector2.zero;

		for (int i = 0; i < ctrl.GetControlCount(); ++i)
			{
			TouchableControl c = ctrl.GetControl(i);
			if (c == null)
				continue;
	
			if (i == 0)
				prioRange.x = prioRange.y = c.prio;
			else 
				{
				prioRange.x = Mathf.Min(prioRange.x, c.prio);
				prioRange.y = Mathf.Max(prioRange.y, c.prio);
				}

			TouchStick stick = c as TouchStick;
			if ((stick != null) && stick.dynamicMode)
				{
				prioRange.x = Mathf.Min(prioRange.x, stick.dynamicRegionPrio);
				prioRange.y = Mathf.Max(prioRange.y, stick.dynamicRegionPrio);
				}
			}

		return prioRange;
		}

	}
}

#endif
