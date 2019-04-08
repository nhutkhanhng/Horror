using UnityEngine;
using UnityEngine.UI;

public class SavedGame : MonoBehaviour {

    public SceneLoader loader;

    public Text sceneName;
    public Text dateTime;

    private string scene;
    private string save;
	
    void Start()
    {
        loader = transform.root.GetComponent<SceneLoader>();
    }

	public void SetSavedGame (string SaveName, string SceneName, string DateTime) {
        sceneName.text = SceneName;
        dateTime.text = DateTime;
        scene = SceneName;
        save = SaveName;
        PlayerPrefs.SetString("LoadSaveName", SaveName);
    }

    public void LoadSavedGame()
    {
        PlayerPrefs.SetInt("LoadGame", 1);
        PlayerPrefs.SetString("LoadSaveName", save);
        loader.LoadLevelAsync(scene);
    }
}
