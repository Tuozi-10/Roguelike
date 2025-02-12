using UnityEngine;

namespace AI
{
    [CreateAssetMenu(menuName = "SO/bat")]
    public class BatData : AbstractData
    {
        [field: Header("speed "), Space, SerializeField]
        public int speed { get; private set; }

        public override AbstractDataInstance Instance()
        {
            return new BatDataInstance(this);
        }
    }

    public class BatDataInstance : AbstractDataInstance
    {
        public int speed;
        public BatDataInstance(BatData data) : base(data)
        {
            speed = data.speed;
        }
    }
}