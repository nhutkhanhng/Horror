using System.IO;
using UnityEngine;
using ThunderWire.SaveLoadManager;
using UnityEngine.UI;

public class SaveFinder : MonoBehaviour {
    SaveGameManager handler = new SaveGameManager();

    public GameObject SavedGamePrefab;
    public Transform SavedGameContent;
    public Text EmptyText;

    private string filepath;
    private FileInfo[] fi;
    private DirectoryInfo di;

    void Start()
    {
        filepath = Application.dataPath + "/Data/SaveGame/";
        if (Directory.Exists(filepath))
        {
            di = new DirectoryInfo(filepath);
            fi = di.GetFiles("Save?.sav");
            if (fi.Length > 0)
            {
                EmptyText.gameObject.SetActive(false);
                for (int i = 0; i < fi.Length; i++)
                {
                    handler.SetFilename(fi[i].Name);
                    GameObject savedGame = Instantiate(SavedGamePrefab);
                    savedGame.transform.SetParent(SavedGameContent);
                    savedGame.transform.localScale = new Vector3(1, 1, 1);
                    string scene = handler.DeserializeArray("scene")[0];
                    string date = handler.DeserializeArray("dateTime")[0];
                    savedGame.GetComponent<SavedGame>().SetSavedGame(fi[i].Name, scene, date);
                }
            }
            else
            {
                EmptyText.gameObject.SetActive(true);
            }
        }
        else
        {
            Directory.CreateDirectory(filepath);
        }
    }
}
