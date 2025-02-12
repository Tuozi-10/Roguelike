using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

[CreateAssetMenu(menuName = "SciptableObject/Cage", fileName = "data")]
public class CageData : IaABstractData
{
    [field: Header("Spawn Max"), SerializeField]
    public int SpawnMax { get; private set; }
    
    [field: Header("Spawn Min"), SerializeField]
    public int SpawnMin { get; private set; }
    
    [field: Header("Spawn"), SerializeField]
    public List<AbstractIA> Spawn { get; private set; }

    public override IaABstractDataInstance Instance()
    {
        return new CageDataInstance(this);
    }
}

public class CageDataInstance : IaABstractDataInstance
{
    public int spawnMax;
    public int spawnMin;
    public List<AbstractIA> spawn;

    public CageDataInstance(CageData data) : base(data)
    {
        spawn = data.Spawn;
        spawnMax = data.SpawnMax;
        spawnMin = data.SpawnMin;
    }
}