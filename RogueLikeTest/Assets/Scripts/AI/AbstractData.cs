using UnityEngine;

namespace AI
{
    public abstract class AbstractData : ScriptableObject
    {
        [field: Header("hp "), Space, SerializeField]
        public int hp { get; private set; }
        [field: Header("armor "), Space, SerializeField]
        public int armor { get; private set; }

        public abstract AbstractDataInstance Instance();
    }

    public class AbstractDataInstance
    {
        public int hp;
        public int armor;

        public AbstractDataInstance(AbstractData data)
        {
            hp = data.hp;
            armor = data.armor;
        }
    }
    
}