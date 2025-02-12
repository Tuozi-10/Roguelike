using UnityEngine;

[CreateAssetMenu(fileName = "Bat", menuName = "Scriptable Objects/BatData")]
public class BatData : AIData
{
    [Header("Specific values")] public float RangeWander;
    
    public override AIDataInstance Instance() => new BatDataInstance(this);
}

public class BatDataInstance : AIDataInstance
{
    public float RangeWander;
    
    public BatDataInstance(BatData data) : base(data)
    {
        RangeWander = data.RangeWander;
    }
}