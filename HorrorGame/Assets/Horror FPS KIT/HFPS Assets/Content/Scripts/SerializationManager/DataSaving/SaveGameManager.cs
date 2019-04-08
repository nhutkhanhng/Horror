/*
namespace TW.SaveLoadManager - SaveGameManager.cs by ThunderWire Games (Script for saving and reading game data)
*/

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderWire.SaveLoadManager
{
    public class SaveGameManager
    {
        public static string filepath;
        public static string filename;

        public static Dictionary<string, List<string>> GameData = new Dictionary<string, List<string>>();

        public void Refresh()
        {
            GameData.Clear();
        }

        public void UpdateGameData(string Key, List<string> Value)
		{
            /*
			byte[] KeyBytes = Encoding.UTF8.GetBytes (Key);
            string e_Key = Convert.ToBase64String(KeyBytes);

            for(int i = 0; i < Value.Count; i++)
            {
                byte[] ValueBytes = Encoding.UTF8.GetBytes(Value[i]);
                string e_Value = Convert.ToBase64String(ValueBytes);
            }
            */

			GameData.Add(Key, Value);
		}
			
		public void SerializeGameData(string fileName)
		{
			filepath = Application.dataPath + "/Data/SaveGame/";
			filename = filepath + fileName + ".sav";

			if (!Directory.Exists(filepath)){
				Directory.CreateDirectory(filepath);
			}

			DoSerialize ();
		}

        public void SetFilename(string fileName)
        {
            filepath = Application.dataPath + "/Data/SaveGame/";

            if (fileName.Contains('.'))
            {
                filename = filepath + fileName;
            }
            else
            {
                filename = filepath + fileName + ".sav";
            }
        }

        static void DoSerialize()
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                foreach (KeyValuePair<string, List<string>> data in GameData)
                {
                    string key = data.Key.ToString();
                    string value = string.Join(",", data.Value.ToArray());
                    value = value.Replace(Environment.NewLine, " ");
                    value = value.Replace("\r\n", " ");
                    sw.WriteLine(key + "=" + value);
                }
            }
        }

        public string[] DeserializeArray(string key)
        {
            if (File.Exists(filename))
            {            
                using (StreamReader reader = new StreamReader(filename))
                {
                    string line = "";
                    string m_key = "";
                    List<string> value_list = new List<string>();
                    while (!string.IsNullOrEmpty(line = reader.ReadLine()))
                    {
                        string[] ln_Input = line.Split(new char[] { '=' });
                        m_key = ln_Input[0].Trim();
                        if (m_key == key)
                        {
                            if (ln_Input[1].Contains(','))
                            {
                                string[] value_split = ln_Input[1].Split(new char[] { ',' });

                                for (int i = 0; i < value_split.Length; i++)
                                {
                                    value_list.Add(value_split[i]);
                                }

                                return value_list.ToArray();
                            }
                            else
                            {
                                return new string[] { ln_Input[1] };
                            }
                        }
                        value_list.Clear();                
                    }
                }
            }
            return null;
        }
    }
}