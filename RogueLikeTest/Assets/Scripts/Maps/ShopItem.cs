using System;
using System.Collections.Generic;
using Bonuses;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Maps
{
    public class ShopItem : MonoBehaviour
    {
        [SerializeField] private List<Bonus> m_bonusesPossibles;
        [SerializeField] private int m_minPrice = 1;
        [SerializeField] private int m_maxPrice = 30;

        private Bonus m_bonus;
        
        private void Awake()
        {
            m_bonus = Instantiate(m_bonusesPossibles[Random.Range(0, m_bonusesPossibles.Count)], transform, true);
            m_bonus.transform.position = transform.position;
            m_bonus.SetPrice(Random.Range(m_minPrice, m_maxPrice+1));
            MapManager.shopItems.Add(m_bonus);
        }
    }
}