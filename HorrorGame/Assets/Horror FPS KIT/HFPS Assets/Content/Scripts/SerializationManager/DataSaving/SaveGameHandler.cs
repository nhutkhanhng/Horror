using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ThunderWire.SaveLoadManager;
using ThunderWire.Parser;

public class SaveGameHandler : MonoBehaviour {
    SaveGameManager saveManager = new SaveGameManager();
    ParsePrimitives parser = new ParsePrimitives();

    public ItemSwitcher switcher;

    private bool loadGame;

    //Main PlayerData list
    private string[] PlayerData;

    private SaveObject[] saveObjects;

    void Start()
    {
        //switcher = Camera.main.transform.parent.parent.GetComponent<ScriptManager>().itemSwitcher;

        saveObjects = FindObjectsOfType<SaveObject>();

        if (PlayerPrefs.HasKey("LoadGame"))
        {
            loadGame = Convert.ToBoolean(PlayerPrefs.GetInt("LoadGame"));
            if (loadGame && PlayerPrefs.HasKey("LoadSaveName"))
            {
                string filename = PlayerPrefs.GetString("LoadSaveName");
                if (File.Exists(Application.dataPath + "/Data/SaveGame/" + filename))
                {
                    saveManager.SetFilename(filename);
                }
                else
                {
                    Debug.Log("No load found: " + filename);
                    loadGame = false;
                }
            }
        }

        /* LOAD SECTION */
        if (loadGame && PlayerPrefs.HasKey("LoadSaveName"))
        {
            PlayerData = saveManager.DeserializeArray("playerData");

            Vector3 pos = new Vector3(float.Parse(PlayerData[0]), float.Parse(PlayerData[1]), float.Parse(PlayerData[2]));
            float y = float.Parse(PlayerData[3]);

            Camera.main.transform.root.position = pos;
            Camera.main.transform.parent.parent.GetComponent<MouseLook>().SetRotation(y);
            Camera.main.transform.root.GetComponent<HealthManager>().Health = float.Parse(saveManager.DeserializeArray("playerHealth")[0]);

            //Load ItemSwitcher
            int switchID = int.Parse(saveManager.DeserializeArray("activeItem")[0]);
            if (switchID != -1)
            {
                switcher.SetItemSwitcher(switchID);
            }

            //Load ItemSwitcher Item Data
            for (int i = 0; i < switcher.ItemList.Count; i++)
            {
                if (switcher.ItemList[i].GetComponent<SaveHelper>())
                {
                    string[] switcherItemData = saveManager.DeserializeArray("switcheritem_" + switcher.ItemList[i].name);
                    switcher.ItemList[i].GetComponent<SaveHelper>().LoadSavedValues(switcherItemData.ToList());
                }
            }

            //Loading Inventory Data
            StartCoroutine(loadInv());

            //Setting data to saved objects
            for (int i = 0; i < saveObjects.Length; i++)
            {
                saveObjects[i].SetObjectData(saveManager.DeserializeArray(saveObjects[i].uniqueName));
            }
        }
    }

    System.Collections.IEnumerator loadInv()
    {
        yield return new WaitUntil(() => GetComponent<Inventory>().slots.Count > 0);

        int slotsCount = int.Parse(saveManager.DeserializeArray("inv_slots_count")[0]);
        int neededSlots = slotsCount - GetComponent<Inventory>().slots.Count;

        if(neededSlots != 0)
        {
            GetComponent<Inventory>().ExpandSlots(neededSlots);
        }

        for (int i = 0; i < GetComponent<Inventory>().slots.Count; i++)
        {
            string[] invData = saveManager.DeserializeArray("inv_slot_" + i);
            if (invData[0] != "null")
            {
                GetComponent<Inventory>().AddSlotItem(int.Parse(invData[2]), int.Parse(invData[0]), int.Parse(invData[1]));
            }
        }
    }

    /* SAVE SECTION */
    public void Save()
    {
        saveManager.Refresh();
        saveManager.UpdateGameData("scene", new List<string>() { UnityEngine.SceneManagement.SceneManager.GetActiveScene().name});
        saveManager.UpdateGameData("dateTime", new List<string>() { DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
        saveManager.UpdateGameData("playerData", new List<string>()
        {
            Camera.main.transform.root.position.x.ToString(),
            Camera.main.transform.root.position.y.ToString(),
            Camera.main.transform.root.position.z.ToString(),
            Camera.main.transform.parent.parent.GetComponent<MouseLook>().rotationX.ToString()
        });
        saveManager.UpdateGameData("playerHealth", new List<string>() { Camera.main.transform.root.GetComponent<HealthManager>().Health.ToString() });

        //Save Item Switcher Current Item
        saveManager.UpdateGameData("activeItem", new List<string>() { switcher.currentItem.ToString() });

        //Save Item Switcher Item Data
        for (int i = 0; i < switcher.ItemList.Count; i++)
        {
            if (switcher.ItemList[i].GetComponent<SaveHelper>())
            {
                switcher.ItemList[i].GetComponent<SaveHelper>().CallScriptGetValues();
                saveManager.UpdateGameData("switcheritem_" + switcher.ItemList[i].name, switcher.ItemList[i].GetComponent<SaveHelper>().HandlerGetValues());
            }
        }

        //Save Inventory Slots Count
        saveManager.UpdateGameData("inv_slots_count", new List<string>() { GetComponent<Inventory>().slots.Count.ToString() });

        //Save Inventory Data
        for (int i = 0; i < GetComponent<Inventory>().slots.Count; i++)
        {
            if(GetComponent<Inventory>().slots[i].transform.childCount > 0)
            {
                InventoryItemData itemData = GetComponent<Inventory>().slots[i].transform.GetChild(0).GetComponent<InventoryItemData>();
                saveManager.UpdateGameData("inv_slot_" + i, new List<string>() {
                    itemData.item.ID.ToString(),
                    itemData.amount.ToString(),
                    itemData.slotID.ToString()
                });
            }
            else
            {
                saveManager.UpdateGameData("inv_slot_" + i, new List<string>() { "null" });
            }
        }

        //Save objects with SaveObject.cs script
        for (int i = 0; i < saveObjects.Length; i++)
        {
            foreach(KeyValuePair<string, List<string>> data in saveObjects[i].GetObjectData())
            {
                saveManager.UpdateGameData(data.Key, data.Value);
            }
        }

        SaveGame();
    }

    void SaveGame()
    {
        string filepath = Application.dataPath + "/Data/SaveGame/";

        if (Directory.Exists(filepath))
        {
            DirectoryInfo di = new DirectoryInfo(filepath);
            FileInfo[] fi = di.GetFiles("Save?.sav");

            if (fi.Length > 0)
            {
                string SaveName = "Save" + fi.Length;
                saveManager.SerializeGameData(SaveName);
                Debug.Log("Serilaize Contains");
            }
            else
            {
                saveManager.SerializeGameData("Save0");
                Debug.Log("Serilaize No Contains");
            }
        }
        else
        {
            saveManager.SerializeGameData("Save0");
            Debug.Log("Serilaize No Directory");
        }
    }
}
