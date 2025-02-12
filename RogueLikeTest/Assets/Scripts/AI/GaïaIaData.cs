using AI;
using UnityEngine;

namespace AI
{
    [CreateAssetMenu(fileName = "GaïaIaData", menuName = "Scriptable Objects/GaïaIaData")]

    public class GaïaIaData : AbstractData
    {
        [field: Header("objectSpawned "), SerializeField]
        public GameObject m_spawn { get; set; }
        [field: Header("max "), SerializeField]
        public int m_spawnMax { get; set; }
        [field: Header("min "), SerializeField]
        public int m_spawnMin { get; set; }

        public override AbstractDataInstance Instance()
        {
            return new GaïaIaInstance(this);
        }
    }

    public class GaïaIaInstance : AbstractDataInstance
    {
        public GameObject spawn;
        public int spawnMax;
        public int spawnMin;
        public GaïaIaInstance(GaïaIaData data) : base(data)
        {
            spawn = data.m_spawn;
            spawnMax = data.m_spawnMax;
            spawnMin = data.m_spawnMin;
        }
    }
}

