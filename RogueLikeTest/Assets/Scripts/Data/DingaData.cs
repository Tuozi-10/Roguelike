using UnityEngine;

[CreateAssetMenu(fileName = "Dinga", menuName = "Scriptable Objects/Dinga")]
public class DingaData : EnemyData
{
    public new DingaDataInstance Instance() => new DingaDataInstance(this);
}

public class DingaDataInstance : EnemyDataInstance
{
    public DingaDataInstance(EnemyData data) : base(data)
    {
        
    }
}
