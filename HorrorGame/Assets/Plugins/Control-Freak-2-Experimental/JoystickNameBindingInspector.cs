#if false

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

using ControlFreak2Editor;
using ControlFreak2;
using ControlFreak2.Internal;
using ControlFreak2Editor.Internal;

namespace ControlFreak2Editor.Inspectors
{


// ----------------------
// Joystick Name Binding Inspector.
// ----------------------
public class JoystickNameBindingInspector
	{	
	private GUIContent				labelContent;
	private Object					undoObject;
	//private Editor					editor;
	private RigJoystickNameDrawer	joyNameField;



	// ------------------
	public JoystickNameBindingInspector(/*Editor editor,*/ Object undoObject, GUIContent labelContent)
		{	
		this.labelContent			= labelContent;
		//this.editor					= editor;
		this.undoObject				= undoObject;
		this.joyNameField			= new RigJoystickNameDrawer();
		}


	// ------------------
	public void Draw(JoystickNameBinding bind, InputRig rig)
		{
		string 		joystickName	= bind.joystickName;
		bool		bindingEnabled	= bind.enabled;
	
		EditorGUILayout.BeginVertical();
		if (bindingEnabled = EditorGUILayout.ToggleLeft(this.labelContent, bindingEnabled, GUILayout.MinWidth(30)))
			{
			EditorGUILayout.BeginHorizontal();

			GUILayout.Space(InputBindingGUIUtils.MARGIN);

			joystickName = this.joyNameField.Draw("Joystick", joystickName, rig);

			EditorGUILayout.EndHorizontal();

			GUILayout.Space(InputBindingGUIUtils.VERT_MARGIN);
			}
		EditorGUILayout.EndVertical();
		
		
		if ((bindingEnabled != bind.enabled) ||
			(joystickName	!=	bind.joystickName) )
			{		
			CFGUI.CreateUndo("Joystick Name Binding modification.", this.undoObject);
			
			bind.enabled		= bindingEnabled;
			bind.joystickName	= joystickName;
					
			CFGUI.EndUndo(this.undoObject);
			}

		}
	}

		
}
#endif

#endif
