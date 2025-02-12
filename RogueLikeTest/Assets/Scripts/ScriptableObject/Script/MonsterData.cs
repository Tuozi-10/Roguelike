using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;


[CreateAssetMenu(menuName = "SciptableObject/Monster", fileName = "data")]
public class MonsterData : IaABstractData
{
    [field: Header("Range Wander"), SerializeField]
    public int RangeWander { get; private set; }
    
    [field: Header("Spawn Max"), SerializeField]
    public int SpawnMax { get; private set; }
    
    [field: Header("Spawn Min"), SerializeField]
    public int SpawnMin { get; private set; }
    
    [field: Header("Spawn"), SerializeField]
    public List<AbstractIA> Spawn { get; private set; }
    
    [field: Header("Spawn"), SerializeField]
    public float TimeForSpawn { get; private set; }
    
    
    
    public override IaABstractDataInstance Instance()
    {
        return new MonsterDataInstance(this);
    }
}

public class MonsterDataInstance : IaABstractDataInstance
{
    public int rangeWonder;
    public int spawnMax;
    public int spawnMin;
    public List<AbstractIA> spawn;
    public float timeForSpawn;
    
    public MonsterDataInstance(MonsterData data) : base(data)
    {
        rangeWonder = data.RangeWander;
        spawn = data.Spawn;
        spawnMax = data.SpawnMax;
        spawnMin = data.SpawnMin;
        timeForSpawn = data.TimeForSpawn;
    }
}
