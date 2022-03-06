using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectiles
{
    public class ProjectileManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_prefabPoof;
        [SerializeField] private Transform m_parent;
        private Queue<GameObject> m_pool= new Queue<GameObject>();

        public static ProjectileManager instance;
        public static List<ProjectileBase> projectilesList = new List<ProjectileBase>();
        
        
        private void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }
            
            instance = this;
        }
        
        public void RequestPoof(Vector2 pos)
        {
            StartCoroutine(DoPoof(pos));
        }
        
        private void GetFromPoolPoof(out GameObject go)
        {
            go = m_pool.Count > 0 ? m_pool.Dequeue() : Instantiate(m_prefabPoof, m_parent);
            go.SetActive(true);
        }

        private IEnumerator DoPoof(Vector2 pos)
        {
            GetFromPoolPoof(out var go);
            go.transform.position = pos;

            yield return new WaitForSeconds(0.16f);

            go.SetActive(false);
            m_pool.Enqueue(go);
        }

        public void CleanMap()
        {
            foreach (var projectile in projectilesList)
            {
                projectile.ImpactBehaviour();
            }
        }
        
    }
}