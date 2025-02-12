using System.Collections.Generic;
using AI;
using UnityEngine;

[CreateAssetMenu(fileName = "Cage", menuName = "Scriptable Objects/Cage")]
public class CageData : EnemyData
{
    [Header("Specific")]
    public int SpawnMin;
    public int SpawnMax;
    public List<AbstractIA> Spawn;
    public new CageDataInstance Instance() => new CageDataInstance(this);

    
}

public class CageDataInstance : EnemyDataInstance
{
    public int SpawnMin;
    public int SpawnMax;
    public List<AbstractIA> Spawn;
    
    public CageDataInstance(CageData data) : base(data)
    {
        Spawn = data.Spawn;
        SpawnMin = data.SpawnMin;
        SpawnMax = data.SpawnMax;
    }
}
