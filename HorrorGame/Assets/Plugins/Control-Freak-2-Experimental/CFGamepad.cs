/*

using UnityEngine;

namespace ControlFreak2
{

[System.Serializable]
public class CFGamepad
	{
		
	public bool IsSuppprted { get { return false; } }

	
	public enum AxisId
		{
		LS_X,
		LS_Y,
		
		RS_X,
		RS_Y,
	
		DPAD_X,
		DPAD_Y,	
	
		L_TRIGGER,
		R_TRIGGER
		}
	
	public enum KeyId
		{
		FACE_TOP,
		FACE_RIGHT,
		FACE_BOTTOM,
		FACE_LEFT,
	
		START,
		SELECT,	
		
		L_BUMPER,
		R_BUMPER,
	
		L3,
		R3
		}
	
	
	public enum DigiKeyId
		{
		LS_UP,
		LS_RIGHT,	
		LS_DOWN,
		LS_LEFT,
	
		RS_UP,
		RS_RIGHT,	
		RS_DOWN,
		RS_LEFT,
	
		DPAD_UP,
		DPAD_RIGHT,	
		DPAD_DOWN,
		DPAD_LEFT,
			
		L_TRIGGER,	
		R_TRIGGER,
		
		L_BUMPER,
		R_BUMPER,	
	
		L3,
		R3,
	
		START,
		SELECT
		}
	
	public const KeyId 
		PS_SELECT	= KeyId.SELECT,
		PS_L1		= KeyId.L_BUMPER,
		PS_R1		= KeyId.R_BUMPER,
		
		XBOX_BACK	= KeyId.SELECT;
	
		
	
	public bool	
		haveBumpers,
		haveTriggers,
		haveDpad,
		haveStart,
		haveSelect,
		haveRightStick;
		
	
	
	public string GetName(KeyId key)
		{
		return "TEST";
		}
	
	public string GetSpriteTagNGUI(KeyId key)
		{
		return "NGUI-TAG";
		}
	
	public string GetSpriteTagDaikon(KeyId key)
		{
		return "DAIKON-TAG";
		}
	
	}
}
*/
