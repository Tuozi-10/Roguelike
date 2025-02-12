using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class GaïaIa : AbstractIA
    {
        public GaïaIaInstance m_GaiaData;
        
        protected override void Init()
        {
            base.Init();
            
            m_GaiaData = (GaïaIaInstance)m_dataInstance;
            Debug.Log(m_GaiaData.spawn);
        }

        public void DoGaiaCaca()
        {
            for (int i = 0; i < Random.Range(m_GaiaData.spawnMin, m_GaiaData.spawnMax + 1); i++)
            {
                var go = Instantiate(m_GaiaData.spawn, transform.parent);
                go.transform.position = transform.position + new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f,0.75f),0);
                go.GetComponent<AbstractIA>().ChangeState(AIStates.attacking);
            }
        }
    }

}
