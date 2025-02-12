using System.Collections.Generic;
using AI;
using UnityEngine;

[CreateAssetMenu(fileName = "DingAI", menuName = "Scriptable Objects/DingAIData")]
public class DingAIData : AIData
{
    public override AIDataInstance Instance() => new DingAIDataInstance(this);
}

public class DingAIDataInstance : AIDataInstance
{
    public DingAIDataInstance(DingAIData data) : base(data)
    {
        
    }
}