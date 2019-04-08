using UnityEngine;

public class ItemID : MonoBehaviour {

	public enum Type { NoInventoryItem, InventoryItem, WeaponItem, BackpackExpand, QuestItem }
	public enum WeaponType { Weapon, Ammo }
    public enum MessageType { None, Hint, Message, Quest, ItemName }

	public Type ItemType = Type.InventoryItem;
	public WeaponType weaponType = WeaponType.Weapon;
    public MessageType messageType = MessageType.None;
    public string message;
    public AudioClip PickupSound;
	public int Amount = 1;

	public int WeaponID;
	public int InventoryID;

    public int itemID;
    public int QuestID;
    public int QuestStep;
    public int TotalStep;
    
	public int BackpackExpand;
	public bool DestroyOnPickup;
    public bool DisableOnPickup;

    public void UseObject()
	{
        if (PickupSound)
        {
            AudioSource.PlayClipAtPoint(PickupSound, transform.position);
        }

        if (DestroyOnPickup)
        {
            Destroy(gameObject);
        }

        if (DisableOnPickup)
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().useGravity = false;
            gameObject.SetActive(false);
            
            if(transform.childCount > 0)
            {
                foreach (Transform child in transform.transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
	}

    public void DisableObject(bool Disable)
    {
        if (Disable == false)
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;

            if (transform.childCount > 0)
            {
                foreach (Transform child in transform.transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}
