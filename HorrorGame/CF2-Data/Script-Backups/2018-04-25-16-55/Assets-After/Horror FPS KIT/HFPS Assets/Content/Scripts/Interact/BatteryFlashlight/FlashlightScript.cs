/*
 * FlashlightScript.cs - script is written by ThunderWire Games
 * Script for Flashlight Controls
 * Updated ver 1.2
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

	[System.Serializable]
	public class BatterySpritesClass{
		public Sprite Battery_0_red;
		public Sprite Battery_5;
		public Sprite Battery_10;
		public Sprite Battery_15;
		public Sprite Battery_20;
		public Sprite Battery_25;
		public Sprite Battery_30;
		public Sprite Battery_35;
		public Sprite Battery_40;
		public Sprite Battery_45;
		public Sprite Battery_50;
		public Sprite Battery_55;
		public Sprite Battery_60;
		public Sprite Battery_65;
		public Sprite Battery_70;
		public Sprite Battery_75;
		public Sprite Battery_80;
		public Sprite Battery_85;
		public Sprite Battery_90;
		public Sprite Battery_95;
		public Sprite Battery_100;
	}

public class FlashlightScript : MonoBehaviour
{

    public ScriptManager scriptManager;
    private InputManager inputManager;
    private ItemSwitcher switcher;
    private HFPS_GameManager gameManager;

    [Header("Setup")]
    public BatterySpritesClass BatterySprites = new BatterySpritesClass();
    public int BatteryInvID;
    public int FlashlightInvID;

    public AudioClip ReloadBatteriesSound;

    private GameObject BatteryUI;
    private Image BatteryPercentSpr;
    private Slider BatterySlider;

    [Header("Flashlight Animations")]
    public bool useAnimation;
    public GameObject FlashlightGO;

    public string DrawAnimation;
    public string HideAnimation;
    public string IdleAnimation;

    public float DrawSpeed;
    public float HideSpeed;

    [Header("Flashlight Settings")]
    public Light Flashlight;
    public AudioClip ClickSound;
    public float batteryLifeInSec = 140f;
    public float batteryPercentage = 100;

    [HideInInspector]
    public KeyCode FlashlightKey;

    [HideInInspector]
    public bool PickedFlashlight = false;

    private int Batteries;
    private int MaxBatteries;

    [HideInInspector]
    public bool canPickup;

    [HideInInspector]
    public bool canReload;

    private bool hide;
    private bool eventOn;
    private bool on;
    private bool playAnim;
    private bool isPlayed = true;

    void Start()
    {
        inputManager = scriptManager.inputManager;
        switcher = transform.parent.parent.gameObject.GetComponent<ItemSwitcher>();
        gameManager = scriptManager.GameManager;
        BatteryUI = gameManager.BatteryUI;
        BatterySlider = gameManager.BatterySlider;
        BatteryPercentSpr = gameManager.BatteryPercentSpr;
        BatterySlider.value = Flashlight.intensity;
        BatteryUI.SetActive(false);
    }

    public void ReloadBattery()
    {
        if (FlashlightGO.activeSelf)
        {
            if (Batteries > 0 && batteryPercentage < 90.0f)
            {
                batteryPercentage = 100;
                if (ReloadBatteriesSound)
                {
                    AudioSource.PlayClipAtPoint(ReloadBatteriesSound, transform.position, 0.75f);
                }
            }
        }
    }

    public void Deselect()
    {
        if (FlashlightGO.activeSelf)
        {
            FlashlightGO.GetComponent<Animation>().Play(HideAnimation);
            if (ClickSound)
            {
                AudioSource.PlayClipAtPoint(ClickSound, transform.position, 0.75f);
            }
            on = false;
            hide = true;
            playAnim = false;
        }
    }

    public void Select()
    {
        if (useAnimation)
        {
            playAnim = true;
            isPlayed = false;
        }
        else
        {
            on = !on;
            if (ClickSound)
            {
                AudioSource.PlayClipAtPoint(ClickSound, transform.position, 0.75f);
            }
        }
    }

    public void Event_FlashlightOn()
    {
        on = true;
        eventOn = true;
        if (ClickSound) { AudioSource.PlayClipAtPoint(ClickSound, transform.position, 0.75f); }
    }

    public void LoaderSetItemEnabled()
    {
        FlashlightGO.SetActive(true);
        FlashlightGO.GetComponent<Animation>().Play(IdleAnimation);
        if (useAnimation)
        {
            on = true;
            eventOn = true;
            playAnim = true;
            isPlayed = true;
        }
        else
        {
            on = !on;
        }
    }

    void Update()
    {
        if (inputManager.InputsCount() > 0)
        {
            FlashlightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), inputManager.GetInput("Flashlight"));
        }

        if (batteryPercentage < 90.0f)
        {
            canReload = true;
        }
        else
        {
            canReload = false;
        }

        if (gameManager)
        {
            Inventory inv = gameManager.inventoryScript;
            if (inv.CheckItemIDInventory(BatteryInvID))
            {
                Batteries = inv.GetItemAmount(BatteryInvID);
                MaxBatteries = inv.GetItem(BatteryInvID).MaxItemCount;
            }
            else
            {
                Batteries = 0;
            }

            if (inv.CheckItemIDInventory(FlashlightInvID))
            {
                PickedFlashlight = true;
            }
        }

        Light lite = Flashlight;

        if (PickedFlashlight)
        {
            BatteryUI.SetActive(true);

            if (ControlFreak2.CF2Input.GetKeyDown(FlashlightKey) && batteryPercentage > 0 && !FlashlightGO.GetComponent<Animation>().isPlaying)
            {
                if (!on && !(switcher.currentItem == 0))
                {
                    switcher.selectItem(0);
                }
                else
                {
                    if (useAnimation)
                    {
                        playAnim = !playAnim;
                        isPlayed = false;
                        if (eventOn)
                        {
                            on = false;
                            if (ClickSound) { AudioSource.PlayClipAtPoint(ClickSound, transform.position, 0.75f); }
                            eventOn = false;
                        }
                    }
                    else
                    {
                        on = !on;
                        if (ClickSound)
                        {
                            AudioSource.PlayClipAtPoint(ClickSound, transform.position, 0.75f);
                        }
                    }
                }
            }
        }

        if (!playAnim && hide && !(FlashlightGO.GetComponent<Animation>().isPlaying))
        {
            FlashlightGO.SetActive(false);
            hide = false;
        }

        if (playAnim && !isPlayed)
        {
            FlashlightGO.SetActive(true);
            FlashlightGO.GetComponent<Animation>().Play(DrawAnimation);
            isPlayed = true;
        }
        else if (!isPlayed)
        {
            FlashlightGO.GetComponent<Animation>().Play(HideAnimation);
            hide = true;
            isPlayed = true;
        }

        if (on)
        {
            lite.enabled = true;
            BatterySlider.value = batteryPercentage / 100;
            if(BatterySlider.value < 0.15)
                BatteryPercentSpr.color = Color.red;
            else
            {
                BatteryPercentSpr.color = Color.white;
            }
            batteryPercentage -= Time.deltaTime * (100 / batteryLifeInSec);
            lite.intensity = Mathf.Lerp(lite.intensity, batteryPercentage/100, Time.deltaTime);
        }
        else
        {
            lite.enabled = false;
        }

        Batteries = Mathf.Clamp(Batteries, 0, MaxBatteries);

        if (Batteries <= 0)
        {
            Batteries = 0;
            canPickup = true;
        }

        //Setting for a max batteries
        else if (Batteries >= MaxBatteries)
        {
            Batteries = MaxBatteries;
            canPickup = false;
        }

        batteryPercentage = Mathf.Clamp(batteryPercentage, 0, 100);
    }

    public void SendValues()
    {
        if (GetComponent<SaveHelper>())
        {
            GetComponent<SaveHelper>().SetValues(
                new List<string>() {
                    batteryPercentage.ToString()
                });
        }
    }

    public void SetSavedValues(List<string> values)
    {
        batteryPercentage = float.Parse(values[0]);
        Debug.Log("Set Load Values: " + float.Parse(values[0]));
    }
}