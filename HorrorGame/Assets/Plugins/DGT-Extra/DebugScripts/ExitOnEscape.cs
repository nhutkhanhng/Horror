using UnityEngine;
using System.Collections;

namespace ControlFreak2
{
public class ExitOnEscape : MonoBehaviour 
	{
	void Update()
		{
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
		}	
	
	}
}
