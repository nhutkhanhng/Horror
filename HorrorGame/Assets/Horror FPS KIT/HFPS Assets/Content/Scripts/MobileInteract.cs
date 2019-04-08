using System;
using System.Runtime.Serialization.Formatters;
using System.Text;
using UnityEngine;

public enum State
{
    CanInteract,
    CantInteract,
    Idle,
}
public class MobileInteract : MonoBehaviour
{
    public float MaxDistance = 2;
    
    private GameObject Player;
    private bool isAllow;
    private bool isAdded;
    private bool isRemoved;
    private Vector3 screenPoint;
    private Vector3 interactPoint;
    private float onScreenDistance;
    private BoxCollider TriggerCollider;
    
    public bool enableGizmos;
    public State InteractState;
   

    void Start()
    {
        Player = HFPS_GameManager.Instance.Player;
        interactPoint = new Vector3(0.5f, 0.5f, 0);
        InteractState = State.Idle;
    }


    public void Update()
    {
        if (InteractState == State.CanInteract)
        {
            screenPoint = Camera.main.WorldToViewportPoint(transform.position);
            onScreenDistance = Vector2.Distance(screenPoint, interactPoint);

            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 &&
                            screenPoint.y > 0 &&
                            screenPoint.y < 1;
            
            //Debug.LogError(gameObject.name + "-OnScreen" + onScreen);

            if (onScreen)
            {
                /*Debug.LogError(gameObject.name + "-ScreenPoint: " + screenPoint);
                Debug.LogError(gameObject.name + "-AimPoint: " + interactPoint);

                Debug.LogError(gameObject.name + "-Distance: " + onScreenDistance);*/

                if (!isAdded)
                {
                    MobileInteractManager.Instance.AddInteractObject(gameObject, Math.Abs(onScreenDistance),transform);
                    isAdded = true;
                    isRemoved = false;
                }
                else
                {
                    MobileInteractManager.Instance.UpdateDistance(gameObject, Math.Abs(onScreenDistance));
                }
            }
            else
            {
                if (isAdded && !isRemoved)
                {
                    isAdded = false;
                    isRemoved = true;
                    MobileInteractManager.Instance.RemoveInteracObject(gameObject);
                }
            }  
        }
        else if(InteractState ==  State.CantInteract)
        {
            if (isAdded && !isRemoved)
            {
                isAdded = false;
                isRemoved = true;
                MobileInteractManager.Instance.RemoveInteracObject(gameObject);
                InteractState = State.Idle;
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            InteractState = State.CanInteract;
        }
    }
    
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            InteractState = State.CantInteract;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!enableGizmos) return;
        Gizmos.color = Color.green;
        Player = GameObject.FindWithTag("Player");
        
        var heading = Player.transform.position - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;

        float curDistance = Vector3.Distance(Player.transform.position, transform.position);
        if (curDistance <= MaxDistance)
            Gizmos.color = Color.red;
        
        Gizmos.DrawRay(transform.position, direction * MaxDistance);
    }

    public void AllowInteract()
    {
        isAllow = true;
    }

    public void UnAllowInteract()
    {
        isAllow = false;
    }
}