using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class CageIA : AbstractIA
    {
        protected CageAIData cageAIData { get; set; }
        protected CageAIDataInstance cageAIDataInstance { get; set; }

        protected override void Init()
        {
            base.Init();

            cageAIData = (CageAIData)aiData;
            cageAIDataInstance = (CageAIDataInstance)cageAIData.Instance();
        }
        
        public void DoCageShoot()
        {
            for (int i = 0; i < Random.Range(cageAIDataInstance.spawnMin, cageAIDataInstance.spawnMax + 1); i++)
            {
                var go = Instantiate(cageAIDataInstance.spawn[Random.Range(0, cageAIDataInstance.spawn.Count)], transform.parent, true);
                go.transform.position = transform.position + new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f,0.75f),0);
                go.GetComponent<AbstractIA>().ChangeState(AIStates.attacking);
            }
        }
    }
}