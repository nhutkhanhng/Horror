using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealth : MonoBehaviour {

    [Header("Setup")]
    public Transform Hips;

    [Header("Character Health")]
    public int Health;
    public AudioClip HtAudio;

    private Rigidbody[] RigidbodyCache;
    private ZombieAI ai;
    private GhostAI _ai;

    [HideInInspector] public bool damageTaken;

    void Awake()
    {
        RigidbodyCache = Hips.GetComponentsInChildren<Rigidbody>();
        if (GetComponent<ZombieAI>() != null)
        {
            ai = GetComponent<ZombieAI>();
        }
        else _ai = GetComponent<GhostAI>();
    }

    void Start () {
        Ragdoll(false);

        for (int i = 0; i < RigidbodyCache.Length; i++)
        {
            Physics.IgnoreCollision(Camera.main.transform.root.GetComponent<Collider>(), RigidbodyCache[i].GetComponent<Collider>());
        }
    }

	void Update () {
        if (Health <= 0 || Health <= 0.9)
        {
            Health = 0;
            Ragdoll(true);
        }
    }

    public void ApplyDamage(int damage)
    {
        if (Health <= 0) return;
        Health -= damage;

        damageTaken = true;

        if (HtAudio) { AudioSource.PlayClipAtPoint(HtAudio, transform.position, 1.0f); }
    }

    private void Ragdoll(bool enabled)
    {
        if(ai != null)
            ai.StateMachine(!enabled); 
        else _ai.StateMachine(!enabled);
        foreach (Rigidbody rb in RigidbodyCache)
        {
            if (enabled)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.GetComponent<Collider>().isTrigger = false;
                gameObject.GetComponent<Collider>().enabled = false;
            }
            else
            {
                rb.isKinematic = true;
                rb.useGravity = false;
                rb.GetComponent<Collider>().isTrigger = true;
                gameObject.GetComponent<Collider>().enabled = true;
            }
        }
    }
}
