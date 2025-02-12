using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Dingla")]
public class DinglaData : AIAbstractData
{
    [field : Header("Dingla specific values")]

    

    public override AIAbstractDataInstance Instance()
    {
        return new DinglaDataInstance(this);
    }
    
}

public class DinglaDataInstance : AIAbstractDataInstance
{

    public DinglaDataInstance(DinglaData data) : base(data)
    {
        
    }
}