using System.Collections;
using System.Collections.Generic;
using ControlFreak2;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class OnOffUIPanel : MonoBehaviour
{
	private static OnOffUIPanel instance;

	public static OnOffUIPanel Instance
	{
		get { return instance; }
	}

	[Header("Interact Group")]
	public GameObject InteractGroup;
	public GameObject UseBtn;
	public GameObject ExamineBtn;
	public GameObject PutDownBtn;
	
	public Text lblExam;
	public Text lblUse;
	public TouchButton TouchEventUseBtn;
	public GameObject RotateTrackpad;

	[Header("Weapon Group")] 
	public GameObject PutbackBtn;

	public GameObject GlockGroupBtn;
	public GameObject FlashBtn;
	public GameObject SwitchBtn;
	
	[Header("Other Group")]
	public GameObject PlayerGroupBtn;

	[Header("OtherBtn")] 
	public GameObject InventBtn;
	
	void Awake()
	{
		if (instance == null)
			instance = this;
		else Destroy(gameObject);
	}
	
	public void OnExamineClick(bool isShow)
	{
		RotateTrackpad.SetActive(isShow);
		PutDownBtn.SetActive(isShow);
		SwitchBtn.SetActive(!isShow);
		PlayerGroupBtn.SetActive(!isShow);
		InventBtn.SetActive(!isShow);
	}

	public void ShowUse(bool isShow, string name = null, Transform position = null)
	{
		//if (!string.IsNullOrEmpty(name)) lblUse.text = name;
		TouchEventUseBtn.pressBinding.Clear();

		var CacheTransform = InteractGroup.transform;
		
		if (name != "Examine")
			TouchEventUseBtn.pressBinding.AddKey(KeyCode.F6);
		else TouchEventUseBtn.pressBinding.AddKey(KeyCode.F7);

		if (position != null)
		{
			CacheTransform.position = Camera.main.WorldToScreenPoint(position.position);
		}
		
		UseBtn.SetActive(isShow);
	}

	public void UpdateInteractPosition(Transform position = null)
	{
		if(!UseBtn.activeSelf)
			UseBtn.SetActive(true);
		var CacheTransform = InteractGroup.transform;
		
		if (position != null)
		{
			CacheTransform.position = Camera.main.WorldToScreenPoint(position.position);
		}
	}
	
	public void ShowExam(bool isShow, string name = null,Transform position = null)
	{		
		//if (!string.IsNullOrEmpty(name)) lblExam.text = name;
		var CacheTransform = InteractGroup.transform;
		
		if (position != null)
		{
			CacheTransform.position = Camera.main.WorldToScreenPoint(position.position);
		}
		ExamineBtn.SetActive(isShow);
	}

	public void ShowGlockBtn(bool isShow)
	{
		GlockGroupBtn.SetActive(isShow);
		FlashBtn.SetActive(!isShow);
	}

	public void ShowFlashBtn(bool isShow)
	{
		FlashBtn.SetActive(isShow);
		GlockGroupBtn.SetActive(!isShow);
	}

	public void ShowPutBack(bool isShow)
	{
		PutbackBtn.SetActive(isShow);
	}

	public void HideAllWeaponBtn(bool isShow)
	{
		FlashBtn.SetActive(!isShow);
		GlockGroupBtn.SetActive(!isShow);
	}
}
