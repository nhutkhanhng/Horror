using System.Collections;
using System.Collections.Generic;
using EasyInputVR.Misc;
using UnityEngine;
using UnityEngine.UI;

public class QuitFunction : MonoBehaviour
{

	public GameObject QuitObj;
	public GameObject QuitToMainObj;

	public GameObject InventTab;

	public Transform TransformParent;
	public Transform SubTransformParent;

	public GameObject CanvasObj;

	private Transform oldTransform;

	private Transform m_oldTransform;
	// Use this for initialization
	void Start () {
		 oldTransform = CanvasObj.transform;
		 m_oldTransform = QuitToMainObj.transform;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (OculusGoInteractManager.Instance.BackClickInput())
		{
			if (InventTab != null)
			{
				if (InventTab.activeSelf)
				{
					InventTab.SetActive(false);
				}
				else
				{
					if (QuitObj != null)
					{
						QuitObj.SetActive(!QuitObj.activeSelf);
					}

					if (QuitToMainObj != null)
					{
						QuitToMainObj.SetActive(!QuitToMainObj.activeSelf);
						if (QuitToMainObj.activeSelf)
						{
							CanvasObj.transform.SetParent(SubTransformParent);
						}
						else
						{
							CanvasObj.transform.SetParent(TransformParent);
							CanvasObj.transform.position = oldTransform.position;
							CanvasObj.transform.rotation = oldTransform.rotation;
							CanvasObj.transform.localScale = oldTransform.localScale;
							QuitToMainObj.transform.position = m_oldTransform.position;
							QuitToMainObj.transform.rotation = m_oldTransform.rotation;
							QuitToMainObj.transform.localScale = m_oldTransform.localScale;
						}
					}
				}
			}
			else
			{
				if (QuitObj != null)
				{
					QuitObj.SetActive(!QuitObj.activeSelf);
				}

				if (QuitToMainObj != null)
				{
					QuitToMainObj.SetActive(!QuitToMainObj.activeSelf);
					if (QuitToMainObj.activeSelf)
					{
						CanvasObj.transform.SetParent(SubTransformParent);
					}
					else
					{
						CanvasObj.transform.SetParent(TransformParent);
						CanvasObj.transform.position = oldTransform.position;
						CanvasObj.transform.rotation = oldTransform.rotation;
						CanvasObj.transform.localScale = oldTransform.localScale;
						QuitToMainObj.transform.position = m_oldTransform.position;
						QuitToMainObj.transform.rotation = m_oldTransform.rotation;
						QuitToMainObj.transform.localScale = m_oldTransform.localScale;
					}
				}
			}
		}
		
		
	}

	public void OnAppQuit()
	{
		Application.Quit();
	}

	public void OnReturnToMainMenu()
	{
		Application.LoadLevel("HorrorUnderground");
	}
}
