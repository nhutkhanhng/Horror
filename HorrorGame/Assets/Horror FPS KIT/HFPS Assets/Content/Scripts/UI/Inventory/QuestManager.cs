using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Quest
{
	public int QuestID;
	public int curItemQuest;
	public List<int> lstIDItemQuest = new List<int>();
	public List<GameObject> lstItem = new List<GameObject>();
	public int TotalQuestStep;
	public int curQuestStep;
	public bool isFinish;
	public UnityEvent OnFinish;
	public string FinishMess;
}
public class QuestManager : MonoBehaviour
{

	private static QuestManager instance;

	public static QuestManager Instance
	{
		get { return instance; }
	}
	
	public Dictionary<int,Quest> DcQuest = new Dictionary<int, Quest>();
	
	
	public List<Quest> AllQuest = new List<Quest>();

	public int IdQuestFinish;
	
	private void Awake()
	{
		if (instance == null)
			instance = this;
		else Destroy(gameObject);
	}

	private void Start()
	{
		foreach (var iter in AllQuest)
		{
			DcQuest[iter.QuestID] = iter;
		}
	}

	public Quest GetQuestInfo(int idQuest)
	{
		Quest s;
		if (DcQuest.TryGetValue(idQuest, out s))
		{
			return s;
		}

		return null;
	}

	public void CompleteQuest(int idQuest)
	{
		Quest s;
		if (DcQuest.TryGetValue(idQuest, out s))
		{
			IdQuestFinish += 1;
			s.curQuestStep = s.TotalQuestStep;
			s.isFinish = true;
			s.OnFinish.AddListener(_Debug);
			s.OnFinish.Invoke();
		}
	}

	public void RecordQuestItem(ItemID item = null, int questID = -1, string name = null)
	{
		Quest s;
		int id = 0;
		if (questID != -1)
			id = questID;
		else if (item != null)
			id = item.QuestID;

		var _id = IdQuestFinish * 1000 + 10000;
		if(id != _id)
			return;
		
		if (DcQuest.TryGetValue(id, out s))
		{
			if (s.curQuestStep < s.TotalQuestStep - 1)
			{
				if (item != null)
				{
					if (item.itemID == s.lstIDItemQuest[s.curQuestStep])
					{
						s.curQuestStep += 1;
						s.curItemQuest = item.itemID;
						s.lstItem[s.curQuestStep].SetActive(true);
					}
				}
				else if (questID != -1)
				{
					if (s.lstItem[s.curQuestStep].name == name)
					{
						s.curQuestStep += 1;
						s.lstItem[s.curQuestStep].SetActive(true);
						s.lstItem[s.curQuestStep-1].SetActive(false);
					}
					
				}
			}
			else
			{		
				s.curQuestStep += 1;
				
				if(item != null)
					s.curItemQuest = item.itemID;
				
				s.isFinish = true;
				s.OnFinish.AddListener(_Debug);
				s.OnFinish.Invoke();
			}
		}
		else Debug.LogError("WTF! CANT FIND QUEST");
	}

	void _Debug()
	{
		Quest s;
		var _id = IdQuestFinish * 1000 + 10000;
		if (DcQuest.TryGetValue(_id, out s))
		{
			IdQuestFinish += 1;

			HFPS_GameManager.Instance.AddMessage(s.FinishMess,IdQuestFinish);
		}
	}
}
