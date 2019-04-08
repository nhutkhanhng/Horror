using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour {

	
	public List<GameObject> lstNPC = new List<GameObject>();
	private int i = 4;

	private bool isWin;
	private NPCHealth NpcHealth;

	private void Start()
	{
		NpcHealth = lstNPC[lstNPC.Count -1].transform.GetChild(1).GetComponent<NPCHealth>();
	}

	public void OnNextNPC()
	{
		if (i < lstNPC.Count - 1)
		{
			i += 1;
			lstNPC[i].SetActive(true);
		}
	}

	void Update()
	{
		if (!isWin)
		{
			var npcHeal = NpcHealth.Health;

			if (npcHeal <= 0)
			{
				isWin = true;
				StartCoroutine(ShowWin());
			}
		}
	}

	IEnumerator ShowWin()
	{
		yield return new WaitForSeconds(2.0f);
		HFPS_GameManager.Instance.txtWin.color = Color.red;
		StopCoroutine(ShowWin());
	}
}
