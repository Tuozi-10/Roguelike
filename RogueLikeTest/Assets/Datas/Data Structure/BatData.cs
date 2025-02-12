using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Bat")]
public class BatData : AIAbstractData
{
    [field : Header("Bat specific values")]
    [field:SerializeField] public float rangeWander { get; private set; }

    public override AIAbstractDataInstance Instance()
    {
        return new BatDataInstance(this);
    }
    
}

public class BatDataInstance : AIAbstractDataInstance
{
    public float rangeWander;

    public BatDataInstance(BatData data) : base(data)
    {
        rangeWander = data.rangeWander;
    }
}