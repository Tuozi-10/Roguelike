using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utilities
{
    public class BloodBathManager : MonoBehaviour
    {
        public static BloodBathManager instance { get; private set; }

        [SerializeField] private List<GameObject> m_bloodPrefabs;
        private Queue<GameObject> m_pool = new Queue<GameObject>();
        private Queue<GameObject> m_poolBloodPoof = new Queue<GameObject>();
        [SerializeField] private Transform parentEffects;
        [SerializeField] private GameObject m_bloodPoof;

        private List<GameObject> m_currentlyDisplayed = new List<GameObject>();
        
        private void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            instance = this;
        }

        public void CleanMap()
        {
            foreach (var go in m_currentlyDisplayed)
            {
                go.transform.DOKill();
                go.transform.localScale = Vector3.zero;
                AddToPool(go);
            }
            
            m_currentlyDisplayed.Clear();
        }
        
        public void RequestBlood(Vector2 pos, int count, float spread)
        {
            if (count > 0) RequestBlood(pos, count - 1, spread);

            GetFromPool(out var go);
            go.transform.position = pos + new Vector2(Random.Range(-spread, spread), Random.Range(-spread, spread));
            go.transform.DOScale(0, 0.5f).SetDelay(10f).OnComplete(() =>
            {
                m_currentlyDisplayed.Remove(go);
                AddToPool(go);
            });
            
            m_currentlyDisplayed.Add(go);
        }

        private void GetFromPool(out GameObject go)
        {
            go = m_pool.Count > 0
                ? m_pool.Dequeue()
                : Instantiate(m_bloodPrefabs[Random.Range(0, m_bloodPrefabs.Count)], parentEffects);

            go.transform.localScale = new Vector3(Random.Range(0, 100) >= 50 ? 1 : -1, 1, 1);
            go.SetActive(true);
        }

        private void AddToPool(GameObject go)
        {
            go.transform.DOKill();
            go.SetActive(false);
            m_pool.Enqueue(go);
        }

        public void RequestBloodPoof(Vector2 pos)
        {
            StartCoroutine(DoBloodPoof(pos));
        }
        
        private void GetFromPoolBloodPoof(out GameObject go)
        {
            go = m_poolBloodPoof.Count > 0 ? m_poolBloodPoof.Dequeue() : Instantiate(m_bloodPoof, parentEffects);
            go.SetActive(true);
        }

        private IEnumerator DoBloodPoof(Vector2 pos)
        {
            GetFromPoolBloodPoof(out var go);
            go.transform.position = pos;

            yield return new WaitForSeconds(0.2f);

            go.SetActive(false);
            m_poolBloodPoof.Enqueue(go);
        }
    }
}