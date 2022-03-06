using System.Collections;
using UnityEngine;

namespace Maps
{
    public class MapShop : Map
    {
        public override bool mapDone => true;

        
        protected override void Init()
        {
            StartCoroutine(DoCheckMap());
        }

        IEnumerator DoCheckMap()
        {
            yield return new WaitForSeconds(1.0f);
            
            MapManager.instance.CheckMapCompleted();
        }
    }
}