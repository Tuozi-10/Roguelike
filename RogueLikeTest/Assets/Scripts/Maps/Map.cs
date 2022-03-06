using System;
using System.Collections.Generic;
using AI;
using UnityEngine;

namespace Maps
{
    public class Map : MonoBehaviour
    {
        private List<AbstractIA> enemies = new List<AbstractIA>();

        private void Awake()
        {
          Init();
        }

        protected virtual void Init()
        {
            foreach (var enemy in transform.GetComponentsInChildren<AbstractIA>())
            {
                enemies.Add(enemy);
            }
        }
        
        public virtual bool mapDone
        {
            get
            {
                foreach (var ia in enemies) // check for every enemy still in the list their HP in case we have a death anim or idk
                {
                    if (ia != null && !ia.isDead) return false;
                }

                return true;
            }
            
        } 
        
    }
}
