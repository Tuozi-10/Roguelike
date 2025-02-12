using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class CageIA : AbstractIA
    {
        

        private CageData cage;
        private CageDataInstance cageInstance;
        
        
        
        
        protected override void Init()
        {
            base.Init();
           
            cage = (CageData)enemy;
            cageInstance = cage.Instance();
        }

        public void DoCageShoot()
        {
            for (int i = 0; i < Random.Range(cageInstance.SpawnMin, cageInstance.SpawnMax + 1); i++)
            {
                var go = Instantiate(cageInstance.Spawn[Random.Range(0, cageInstance.Spawn.Count)], transform.parent, true);
                go.transform.position = transform.position + new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f,0.75f),0);
                go.GetComponent<AbstractIA>().ChangeState(AIStates.attacking);
            }
        }

       
    }
}