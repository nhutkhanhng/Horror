/*
 * HFPS_GameManager.cs - script is written by ThunderWire Games
 * This script controls all game actions
 * ver. 1.2
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using EasyInputVR.Misc;
using UnityEngine.SceneManagement;
using ThunderWire.Configuration;
using ThunderWire.Parser;

public enum spriteType
{
    Interact, Grab, Examine
}



public class HFPS_GameManager : MonoBehaviour {
    ConfigManager config = new ConfigManager();
    ParsePrimitives parser = new ParsePrimitives();

    private List<string> inputKeyCache = new List<string>();    
    [Header("Config Setup")]
    public string ConfigName = "GameConfig";
    public bool showDebug;

    private string configName;
    private bool UsePlayerPrefs;
    private bool refreshStatus = false;

    [Header("Win")] public Text txtWin;
    
    [Header("Main")]
    public GameObject Player;
    public InputManager inputManager;
    public Inventory inventoryScript;
    public InteractManager interactManager;

    [HideInInspector]
    public ScriptManager scriptManager;

    [HideInInspector]
    public HealthManager healthManager;

    [Header("Cursor")]
    public bool m_ShowCursor = false;

    [Header("Game Panels")]
    public GameObject PauseGamePanel;
    public GameObject MainGamePanel;
    public GameObject TabButtonPanel;

    [Header("Pause UI")]
    public KeyCode ShowPauseMenuKey;
    public bool reallyPause = false;
    [HideInInspector] public bool isPaused = false;

    [Header("Paper UI")]
    public GameObject PaperTextUI;
    public Text PaperReadText;

    [Header("Flashlight & Battery")]
    public GameObject BatteryUI;

    public Image BatteryPercentSpr;

    public Slider BatterySlider;

    [Header("Gun & Amor")]
    public GameObject GunUI;

    public Text GunAmor;
    

    [Header("Valve UI")]
    public Slider ValveSlider;

    private float slideTime;
    private float slideValue;

    [Header("Saving Notification UI")]
    public GameObject saveNotification;

    [Header("Notification UI")]
    public GameObject NotificationPanel;
    public GameObject NotificationPrefab;
    public Sprite WarningSprite;

    private List<GameObject> Notifications = new List<GameObject>();

    [Header("Hints UI")]
    public Text HintText;

    [Header("Crosshair")]
    public Image Crosshair;

    [Header("Health")]
    public Text HealthText;

    [Header("Down Examine Buttons")]
    public GameObject DownExamineUI;
    public GameObject ExamineButton1;
    public GameObject ExamineButton2;
    public GameObject ExamineButton3;

    [Header("Down Grab Buttons")]
    public GameObject DownGrabUI;
    public GameObject GrabButton1;
    public GameObject GrabButton2;
    public GameObject GrabButton3;
    public GameObject GrabButton4;

    public Sprite DefaultSprite;

    [HideInInspector]
    public bool isHeld;

    [HideInInspector]
    public bool canGrab;

    private float fadeHint;
    private bool startFadeHint = false;

    public KeyCode GrabKey;
    public KeyCode ThrowKey;
    public KeyCode RotateKey;
    public KeyCode InventoryKey;

    private bool isOverlapping;
    private bool isPressed;
    private bool isSet;

    private static HFPS_GameManager instance;

    public static HFPS_GameManager Instance
    {
        get { return instance; }
    }

    void SetKeys()
    {    
        isSet = true;
    }

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        healthManager = Camera.main.transform.root.gameObject.GetComponent<HealthManager>();
        scriptManager = Player.transform.GetChild(0).transform.GetChild(0).GetComponent<ScriptManager>();
    }

    void Start()
    {
        configName = PlayerPrefs.GetString("GameConfig");
        if (PlayerPrefs.HasKey("UsePlayerPrefs"))
        {
            UsePlayerPrefs = parser.ParseType<bool>(PlayerPrefs.GetString("UsePlayerPrefs"));
        }

        if (UsePlayerPrefs)
        {
            if (config.ExistFile(configName))
            {
                config.Setup(showDebug, configName);
            }
            else
            {
                if (config.ExistFile(ConfigName))
                {
                    config.Setup(showDebug, ConfigName);
                }
                else
                {
                    Debug.LogError("\"" + configName + "\"" + " and " + "\"" + ConfigName + "\"" + " does not exist, try launching scene from Main Menu");
                    Debug.LogError("Player will not move if GameConfig does not exist in Data folder");
                }
            }
        }
        else
        {
            if (config.ExistFile(ConfigName))
            {
                config.Setup(showDebug, ConfigName);
            }
            else
            {
                Debug.LogError("\"" + ConfigName + "\"" + " does not exist, try launching scene from Main Menu or run scene again");
                Debug.LogError("Player will not move if GameConfig does not exist in Data folder");
            }
        }

        for (int i = 0; i < config.ConfigKeysCache.Count; i++)
        {
            inputKeyCache.Add(config.ConfigKeysCache[i]);
        }

        TabButtonPanel.SetActive(false);
        saveNotification.SetActive(false);
        HideSprites(spriteType.Interact);
        HideSprites(spriteType.Grab);
        HideSprites(spriteType.Examine);
        Unpause();

        if (m_ShowCursor) {
            ControlFreak2.CFCursor.visible = (true);
            ControlFreak2.CFCursor.lockState = CursorLockMode.None;
        } else {
            ControlFreak2.CFCursor.visible = (false);
            ControlFreak2.CFCursor.lockState = CursorLockMode.Locked;
        }

        if (ContainsSection("Game"))
        {
            float volume = float.Parse(Deserialize("Game", "Volume"));
            AudioListener.volume = volume;
        }
        
        AddMessage("Escape from the current room",0);
    }

    void Update()
    {
        HintText.gameObject.GetComponent<CanvasRenderer>().SetAlpha(fadeHint);

        int AmorAmount = inventoryScript.GetItemAmount(21);
        GunAmor.text = AmorAmount.ToString();
        if (AmorAmount > 0) GunAmor.color = Color.white;
        else GunAmor.color = Color.red;
        
        if (inputManager.InputsCount() > 0 && !isSet)
        {
            SetKeys();
        }

        if (inputManager.GetRefreshStatus() && isSet) {
            isSet = false;
        }

        if (ContainsSection("Game") && GetRefreshStatus())
        {
            float volume = float.Parse(Deserialize("Game", "Volume"));
            AudioListener.volume = volume;
        }

        //Fade Out Hint
        if (fadeHint > 0 && startFadeHint)
        {
            fadeHint -= Time.deltaTime;
        }
        else
        {
            startFadeHint = false;
        }

        if (ControlFreak2.CF2Input.GetKeyDown(ShowPauseMenuKey) && !isPressed) {
            isPressed = true;
            PauseGamePanel.SetActive(!PauseGamePanel.activeSelf);
            MainGamePanel.SetActive(!MainGamePanel.activeSelf);
            isPaused = !isPaused;
        } else if (isPressed) {
            isPressed = false;
        }

        if (PauseGamePanel.activeSelf && isPaused && isPressed) {
            LockStates(true, true, true, true, 3);
            scriptManager.pFunctions.enabled = false;
            if (reallyPause)
            {
                Time.timeScale = 0;
            }
        } else if (isPressed) {
            LockStates(false, true, true, true, 3);
            scriptManager.pFunctions.enabled = true;
            if (reallyPause)
            {
                Time.timeScale = 1;
            }
        }

        if (OculusGoInteractManager.Instance.TriggerLongClickInput() && !isPressed && !isPaused && !isOverlapping) {
            isPressed = true;
            TabButtonPanel.SetActive(!TabButtonPanel.activeSelf);
        } else if (isPressed) {
            isPressed = false;
        }

        
        if (Input.GetKeyDown(KeyCode.Tab) && !isPressed && !isPaused && !isOverlapping) {
            isPressed = true;
            TabButtonPanel.SetActive(!TabButtonPanel.activeSelf);
        } else if (isPressed) {
            isPressed = false;
        }

        if (TabButtonPanel.activeSelf && isPressed) {
            scriptManager.pFunctions.enabled = false;
            LockStates(true, true, true, true, 0);
            HideSprites(spriteType.Interact);
            HideSprites(spriteType.Grab);
            HideSprites(spriteType.Examine);
        } else if (isPressed) {
            LockStates(false, true, true, true, 0);
            scriptManager.pFunctions.enabled = true;
        }

        if (Notifications.Count > 4) {
            Destroy(Notifications[0]);
            Notifications.RemoveAll(GameObject => GameObject == null);
            Debug.Log("Destroy");
        }
    }

    public void OnInventClick()
    {
        if (!isPressed && !isPaused && !isOverlapping) {
            isPressed = true;
            TabButtonPanel.SetActive(!TabButtonPanel.activeSelf);
        } 
        
        if (TabButtonPanel.activeSelf && isPressed) {
            scriptManager.pFunctions.enabled = false;
            LockStates(true, true, true, true, 0);
            HideSprites(spriteType.Interact);
            HideSprites(spriteType.Grab);
            HideSprites(spriteType.Examine);
        } else if (isPressed) {
            LockStates(false, true, true, true, 0);
            scriptManager.pFunctions.enabled = true;
        }
    }
    
    public void Unpause()
    {
        LockStates(false, true, true, true, 3);
        scriptManager.pFunctions.enabled = true;
        PauseGamePanel.SetActive(false);
        MainGamePanel.SetActive(true);
        isPaused = false;
        if (reallyPause)
        {
            Time.timeScale = 1;
        }
    }

    public void ChangeScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void LockStates(bool LockState, bool Interact, bool Controller, bool CursorVisible, int BlurLevel) {
        switch (LockState) {
            case true:
                Player.transform.GetChild(0).GetChild(0).GetComponent<MouseLook>().enabled = false;
                if (Interact) {
                    Player.transform.GetChild(0).GetChild(0).GetComponent<InteractManager>().inUse = true;
                }
                if (Controller) {
                    Player.GetComponent<PlayerController>().controllable = false;
                }
               
                if (CursorVisible) {
                    ShowCursor(true);
                }
                break;
            case false:
                Player.transform.GetChild(0).GetChild(0).GetComponent<MouseLook>().enabled = true;
                if (Interact) {
                    Player.transform.GetChild(0).GetChild(0).GetComponent<InteractManager>().inUse = false;
                }
                if (Controller) {
                    Player.GetComponent<PlayerController>().controllable = true;
                }
                
                if (CursorVisible) {
                    ShowCursor(false);
                }
                break;
        }
    }

    public void ClearAllMess()
    {
        Notifications.RemoveAll(GameObject => GameObject == null);
        int count = NotificationPanel.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(NotificationPanel.transform.GetChild(i).gameObject);
        }
    }
    
    public void UIPreventOverlap(bool State)
    {
        isOverlapping = State;
    }

    public void MouseLookState(bool State)
    {
        switch (State) {
            case true:
                Player.transform.GetChild(0).GetChild(0).GetComponent<MouseLook>().enabled = true;
                break;
            case false:
                Player.transform.GetChild(0).GetChild(0).GetComponent<MouseLook>().enabled = false;
                break;
        }
    }

    public void ShowCursor(bool state)
    {
        switch (state) {
            case true:
                ControlFreak2.CFCursor.visible = (true);
                ControlFreak2.CFCursor.lockState = CursorLockMode.None;
                break;
            case false:
                ControlFreak2.CFCursor.visible = (false);
                ControlFreak2.CFCursor.lockState = CursorLockMode.Locked;
                break;
        }
    }

    public void AddPickupMessage(string itemName)
    {
        GameObject PickupMessage = Instantiate(NotificationPrefab, NotificationPanel.transform);
        
        if(Notifications.Count > 0)
            Notifications[Notifications.Count -1].GetComponent<ItemPickupNotification>().FadeCurMess();
        Notifications.Add(PickupMessage);
        
        PickupMessage.transform.localScale = Vector3.one;

        PickupMessage.transform.SetParent(NotificationPanel.transform);
        PickupMessage.GetComponent<ItemPickupNotification>().SetPickupNotification(itemName);
    }

    public void AddMessage(string message, int Id = -1)
    {
        if (Id >= 0)
        {
            Hint.Instance.AddMess(Id,message);
        }
        GameObject Message = Instantiate(NotificationPrefab, NotificationPanel.transform);
        
        if(Notifications.Count > 0)
            Notifications[Notifications.Count -1].GetComponent<ItemPickupNotification>().FadeCurMess();
        Notifications.Add(Message);
        
        Message.GetComponent<ItemPickupNotification>().SetNotification(message);
    }

    public void WarningMessage(string warning)
    {
        GameObject Message = Instantiate(NotificationPrefab, NotificationPanel.transform);
        
        if(Notifications.Count > 0)
            Notifications[Notifications.Count -1].GetComponent<ItemPickupNotification>().FadeCurMess();
        Notifications.Add(Message);
        
        Message.transform.localScale = Vector3.one;
        Message.transform.SetParent(NotificationPanel.transform);
        Message.GetComponent<ItemPickupNotification>().SetNotificationIcon(warning, WarningSprite);
    }

    public void ShowHint(string hint)
    {
        StopCoroutine(FadeWaitHint());
        fadeHint = 1f;
        startFadeHint = false;
        HintText.gameObject.SetActive(true);
        HintText.text = hint;
        HintText.color = Color.white;
        StartCoroutine(FadeWaitHint());
    }

    IEnumerator FadeWaitHint()
    {
        yield return new WaitForSeconds(3f);
        startFadeHint = true;
    }

    public void NewValveSlider(float start, float time)
    {
        ValveSlider.gameObject.SetActive(true);
        StartCoroutine(MoveValveSlide(start, 10f, time));
    }

    public void DisableValveSlider()
    {
        ValveSlider.gameObject.SetActive(false);
        StopCoroutine(MoveValveSlide(0,0,0));
    }

    public IEnumerator MoveValveSlide(float start, float end, float time)
    {
        var currentValue = start;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / (time * 10);
            ValveSlider.value = Mathf.Lerp(currentValue, end, t);
            yield return null;
        }
    }

    public void ShowSaveNotification(float time)
    {
        StartCoroutine(FadeInSave(time));
    }

    IEnumerator FadeInSave(float t)
    {
        saveNotification.SetActive(true);
        yield return new WaitForSeconds(t);
        StartCoroutine(FadeOutSave());
    }

    IEnumerator FadeOutSave()
    {
        saveNotification.GetComponent<Image>().CrossFadeAlpha(0.1f, 0.5f, true); ;
        saveNotification.transform.GetChild(0).GetComponent<Text>().CrossFadeAlpha(0.1f, 0.5f, true);
        yield return new WaitForSeconds(0.5f);
        saveNotification.SetActive(false);
    }

    public bool CheckController()
	{
		return Player.GetComponent<PlayerController> ().controllable;
	}

    public void ShowInteractSprite(int num, string name, string Key, Transform pos = null)
    {
		if (!isHeld) {
			switch (num) {
				case 1:
				    switch (name)
				    {
				        case "Unlock":
				            OnOffUIPanel.Instance.ShowUse(true, name, pos);
				            break;
				        case "Use":
				            OnOffUIPanel.Instance.ShowUse(true, name, pos);
				            break;
				        case "Grab":
				            OnOffUIPanel.Instance.ShowUse(true, name, pos);
				            break;
				        case "Examine":
				            OnOffUIPanel.Instance.ShowUse(true, name, pos);
				            break;
				    }
				    break;
				case 2:
				    OnOffUIPanel.Instance.ShowExam(true, name, pos);
				break;
			}
		}
    }

    public void ShowExamineSprites(string UseKey, string ExamineKey)
    {
        SetKeyCodeSprite(ExamineButton1.transform, UseKey);
        SetKeyCodeSprite(ExamineButton2.transform, RotateKey.ToString());
        SetKeyCodeSprite(ExamineButton3.transform, ExamineKey);
        DownExamineUI.SetActive(true);
        OnOffUIPanel.Instance.OnExamineClick(true);
    }

    public void ShowGrabSprites()
    {
        SetKeyCodeSprite(GrabButton1.transform, GrabKey.ToString());
        SetKeyCodeSprite(GrabButton2.transform, RotateKey.ToString());
        SetKeyCodeSprite(GrabButton3.transform, ThrowKey.ToString());
        GrabButton4.SetActive(true); //ZoomKey
        DownGrabUI.SetActive(true);
        OnOffUIPanel.Instance.OnExamineClick(true);
    }

    private void SetKeyCodeSprite(Transform Button, string Key)
    {
        if (Key == "Mouse0" || Key == "Mouse1" || Key == "Mouse2")
        {
            Button.GetChild(1).GetComponent<Text>().text = Key;
            Button.GetChild(0).GetComponent<Image>().sprite = GetKeySprite(Key);
            Button.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(25, 25);
            Button.GetChild(0).GetComponent<Image>().type = Image.Type.Simple;
            Button.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            Button.GetChild(1).GetComponent<Text>().text = Key;
            Button.GetChild(0).GetComponent<Image>().sprite = DefaultSprite;
            Button.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(34, 34);
            Button.GetChild(0).GetComponent<Image>().type = Image.Type.Sliced;
            Button.GetChild(1).gameObject.SetActive(true);
        }
        if(Key == "None")
        {
            Button.GetChild(1).GetComponent<Text>().text = "NA";
            Button.GetChild(0).GetComponent<Image>().sprite = DefaultSprite;
            Button.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(34, 34);
            Button.GetChild(0).GetComponent<Image>().type = Image.Type.Sliced;
            Button.GetChild(1).gameObject.SetActive(true);
        }
    }

	public void HideSprites(spriteType type)
	{
	    switch (type)
	    {
	        case spriteType.Interact:
	            OnOffUIPanel.Instance.ShowUse(false);
	            OnOffUIPanel.Instance.ShowExam(false);
	            break;
	        case spriteType.Grab:
	            DownGrabUI.SetActive(false);
	            OnOffUIPanel.Instance.OnExamineClick(false);
	            break;
	        case spriteType.Examine:
	            DownExamineUI.SetActive(false);
	            OnOffUIPanel.Instance.OnExamineClick(false);
	            break;
	    }
	}

	public Sprite GetKeySprite(string Key)
	{
		return Resources.Load<Sprite>(Key);
	}

    public string Deserialize(string Section, string Key)
    {
        return config.Deserialize(Section, Key);
    }

    public void Serialize(string Section, string Key, string Value)
    {
        config.Serialize(Section, Key, Value);
    }

    public bool ContainsSection(string Section)
    {
        return config.ContainsSection(Section);
    }

    public bool ContainsSectionKey(string Section, string Key)
    {
        return config.ContainsSectionKey(Section, Key);
    }

    public bool ContainsKeyValue(string Section, string Key, string Value)
    {
        return config.ContainsKeyValue(Section, Key, Value);
    }

    public void RemoveSectionKey(string Section, string Key)
    {
        config.RemoveSectionKey(Section, Key);
    }

    public bool ExistFile(string file)
    {
        return config.ExistFile(file);
    }

    public string GetKey(int index)
    {
        return inputKeyCache[index];
    }

    public int GetKeysCount()
    {
        return config.ConfigKeysCache.Count;
    }

    public int GetKeysSectionCount(string Section)
    {
        return config.GetSectionKeys(Section);
    }

    public void Refresh()
    {
        StartCoroutine(WaitRefresh());
    }

    IEnumerator WaitRefresh()
    {
        refreshStatus = true;
        yield return new WaitForSeconds(1f);
        refreshStatus = false;
    }

    public bool GetRefreshStatus()
    {
        return refreshStatus;
    }
}