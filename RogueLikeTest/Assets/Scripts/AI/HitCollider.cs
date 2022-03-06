using System;
using Controller;
using UnityEngine;

namespace AI
{
    public class HitCollider : MonoBehaviour
    {
        [SerializeField] private int damages = 1;

        [SerializeField] private GameObject ColliderHit;

        public void EnableCollider() => ColliderHit.SetActive(true);
        public void DisableCollider() => ColliderHit.SetActive(false);
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
                other.GetComponent<PlayerController>().Hit(damages);
        }
    }
}
