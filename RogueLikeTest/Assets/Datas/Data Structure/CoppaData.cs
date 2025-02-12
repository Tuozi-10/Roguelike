using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Coppa")]
public class CoppaData : AIAbstractData
{
    [field : Header("Coppa specific values")]
    
    

    public override AIAbstractDataInstance Instance()
    {
        return new CoppaDataInstance(this);
    }
    
}

public class CoppaDataInstance : AIAbstractDataInstance
{

    public CoppaDataInstance(CoppaData data) : base(data)
    {
        
    }
}