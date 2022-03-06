using System;
using System.Collections.Generic;
using Maps;
using UnityEngine;

namespace Bonuses
{
    public class BonusManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_bonusCutter;
        [SerializeField] private GameObject m_bonusBezier;
        [SerializeField] private GameObject m_bonusHealth;
        [SerializeField] private GameObject m_bonusMoney;

        public static BonusManager instance;
        
        private void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            instance = this;
        }

        public void RequestBonusAtPosition(Bonus.bonusType bonus, Vector3 position, int param = 0)
        {
            GameObject go = null;
            
            switch (bonus)
            {
                case Bonus.bonusType.none: break;
                case Bonus.bonusType.Cutter: go = Instantiate(m_bonusCutter); break;
                case Bonus.bonusType.Bezier: go = Instantiate(m_bonusBezier); break;
                case Bonus.bonusType.Health: go = Instantiate(m_bonusHealth); break;
                case Bonus.bonusType.Money: go = Instantiate(m_bonusMoney); break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bonus), bonus, null);
            }

            if (go is { })
            {
                go.transform.parent = MapManager.instance.transformCurrentMap;
                go.transform.position = position;
                go.GetComponent<Bonus>().param = param;
            }
        }
    }
}