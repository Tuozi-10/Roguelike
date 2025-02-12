using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SciptableObject/BatData", fileName = "data")]
public class BatData : IaABstractData
{
    [field: Header("Range Wander"), SerializeField]
    public int RangeWander { get; private set; }

    public override IaABstractDataInstance Instance()
    {
        return new BatDataInstance(this);
    }
}

public class BatDataInstance : IaABstractDataInstance
{
    public int rangeWonder;

    public BatDataInstance(BatData data) : base(data)
    {
        rangeWonder = data.RangeWander;
    }
    
}