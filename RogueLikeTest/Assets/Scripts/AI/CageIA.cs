using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class CageIA : AbstractIA
    {
        [SerializeField] private List<AbstractIA> m_spawn;
        
        [SerializeField] private int m_spawnMax = 5;
        [SerializeField] private int m_spawnMin = 1;
        
        public void DoCageShoot()
        {
            for (int i = 0; i < Random.Range(m_spawnMin, m_spawnMax + 1); i++)
            {
                var go = Instantiate(m_spawn[Random.Range(0, m_spawn.Count)], transform.parent, true);
                go.transform.position = transform.position + new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f,0.75f),0);
                go.GetComponent<AbstractIA>().ChangeState(AIStates.attacking);
            }
        }
        
    }
}