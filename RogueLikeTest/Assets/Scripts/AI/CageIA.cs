using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class CageIA : AbstractIA
    {
        private CageDataInstance m_cageDataInstance;

        protected override void Init()
        {
            base.Init();
            m_cageDataInstance = (CageDataInstance)m_iaABstractDataInstance;
        }
        
        public void DoCageShoot()
        {
            for (int i = 0; i < Random.Range(m_cageDataInstance.spawnMin, m_cageDataInstance.spawnMin + 1); i++)
            {
                var go = Instantiate(m_cageDataInstance.spawn[Random.Range(0, m_cageDataInstance.spawn.Count)], transform.parent, true);
                go.transform.position = transform.position + new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f,0.75f),0);
                go.GetComponent<AbstractIA>().ChangeState(AIStates.attacking);
            }
        }
        
    }
}