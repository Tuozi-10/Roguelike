using System.Collections.Generic;
using AI;
using UnityEngine;

[CreateAssetMenu(fileName = "CageAI", menuName = "Scriptable Objects/CageAIData")]
public class CageAIData : AIData
{
    [Header("Specific values")] public List<AbstractIA> spawn;
    public int spawnMax = 5;
    public int spawnMin = 1;
    
    public override AIDataInstance Instance() => new CageAIDataInstance(this);
}

public class CageAIDataInstance : AIDataInstance
{
    public List<AbstractIA> spawn;
    public int spawnMax;
    public int spawnMin;
    
    public CageAIDataInstance(CageAIData data) : base(data)
    {
        spawn = data.spawn;
        spawnMax = data.spawnMax;
        spawnMin = data.spawnMin;
    }
}