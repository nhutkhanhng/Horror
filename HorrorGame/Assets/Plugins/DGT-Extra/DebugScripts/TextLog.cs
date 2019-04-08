using UnityEngine;
using System.IO;

namespace ControlFreak2
{
public struct TextLog 
	{
	public string 			filePath;
	public System.DateTime	startDate;
	
	
	// ----------------------
	public bool Init(string filePath, bool autoPath = false)
		{
		if (autoPath)
			filePath = Path.Combine(Application.persistentDataPath, filePath);
	
		this.filePath 	= filePath;
		this.startDate 	= System.DateTime.Now;
	
		return this.Log("-------------\n" +
			"Log start : " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + 
			"\n--------------\n");
		
		//StreamWriter fp = new StreamWriter(this.filePath);
		//if (fp == null)
		//	return false;
	
		//fp.Close();
		//return true; 
		}
	
	
	// -------------------------
	public bool Log(string msg, bool autoTime = true)
		{
		if (this.filePath == null)
			return false;
		FileStream fileStream = new FileStream(this.filePath, FileMode.Append);
		StreamWriter fp = new StreamWriter(fileStream); //this.filePath, true, System.Text.Encoding.UTF8);
		if (fp == null)
			return false;
			
		if (autoTime)
			{
			System.TimeSpan t = (System.DateTime.Now - this.startDate);
			//msg = Time.time.ToString("0000.00") + " : " + msg;
			//msg = ((t.Minutes * 60) + (t.Hours * 3600)).ToString("00") + ":" + t.Minutes.ToString("00") + "." + t.Milliseconds.ToString("000") + 
			msg = ((t.Minutes * 60) + (t.Hours * 3600) + t.Seconds).ToString("0000") + "." + t.Milliseconds.ToString("000") + 
				" : " + msg;
			}
	
		fp.Write(msg);
		fp.Write("\n");
		
		fp.Dispose();	//.Close();
	
		return true;
		}
	
	}
}
	
