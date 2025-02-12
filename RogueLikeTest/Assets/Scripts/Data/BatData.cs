using UnityEngine;

[CreateAssetMenu(fileName = "Bat", menuName = "Scriptable Objects/Bat")]
public class BatData : EnemyData
{
    [Header("Specific")]
    public float RangeWander;
    
    public new BatDataInstance Instance() => new BatDataInstance(this);
}

public class BatDataInstance : EnemyDataInstance
{
    public float RangeWander;

    public BatDataInstance(BatData data) : base(data)
    {
        RangeWander = RangeWander;
    }
}
