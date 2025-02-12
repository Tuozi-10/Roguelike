using System;
using Controller;
using UnityEngine;

public class PoisonCollider : MonoBehaviour
{

    public int PoisonDamage;
    public float PoisonDuration;
    public float LastPoisonTime;
    public float PoisonDelay;
    [SerializeField] private GameObject ColliderHit;

    public void EnableCollider() => ColliderHit.SetActive(true);
    public void DisableCollider() => ColliderHit.SetActive(false);
        
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.CompareTag("Player"))
                other.GetComponent<PlayerController>().StartPoison(PoisonDamage, PoisonDuration, LastPoisonTime, PoisonDelay);

        }
            
    }

    private void Update()
    {
        
        
    }
    
}
