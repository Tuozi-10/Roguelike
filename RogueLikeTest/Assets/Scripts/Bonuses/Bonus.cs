using System;
using Controller;
using DefaultNamespace;
using Maps;
using TMPro;
using UnityEngine;

namespace Bonuses
{
    public class Bonus : MonoBehaviour
    {
        public enum bonusType
        {
            none,
            Cutter,
            Bezier,
            Health,
            Money
        }

        public int param;

        public int price { get; private set; }

        [SerializeField] private TMP_Text m_textPrice;
        
        public void SetPrice(int newPrice)
        {
            price = newPrice;
            RefreshTextColor();
        }
        
        [SerializeField] private bonusType m_bonus;

        public void RefreshTextColor()
        {
            if (m_textPrice is { })
            {
                m_textPrice.text = price.ToString();
                m_textPrice.color = price <= GameManager.Money ? Color.white : Color.red;
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (GameManager.Money < price && price > 0) return;
                
                GameManager.Money -= price;
                PlayerController.instance.ApplyBonus(m_bonus, param);
                MapManager.RefreshShopTexts();
                Destroy(gameObject);
            }
        }

    }
}
