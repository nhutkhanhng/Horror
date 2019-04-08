using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{

	private static Hint instance;

	public static Hint Instance
	{
		get { return instance; }
	}
	
	public Dictionary<int, List<string>> dicMess = new Dictionary<int, List<string>>();

	void Awake()
	{
		if (instance == null)
			instance = this;
		else Destroy(gameObject);
	}
	
	public void AddMess(int questID, string mess)
	{
		if (mess == null) return;
		if(questID < 0) return;
		List<string> lst = new List<string>();
		List<string> _lst = new List<string>();


		if (dicMess.TryGetValue(questID, out _lst))
		{
			_lst.Add(mess);
			dicMess[questID] = _lst;
		}
		else
		{
			lst.Add(mess);
			dicMess.Add(questID,lst);
		}
	}

	public void OnHintClick()
	{
		int id = QuestManager.Instance.IdQuestFinish;
		List<string> lst= new List<string>();
		if (dicMess.TryGetValue(id, out lst))
		{
			HFPS_GameManager.Instance.ClearAllMess();
			foreach (var iter in lst)
			{
				HFPS_GameManager.Instance.AddMessage(iter);
			}
		}
		
	}
}
