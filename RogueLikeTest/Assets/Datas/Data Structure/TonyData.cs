using UnityEngine;

[CreateAssetMenu(menuName = "Data/Tony")]
public class TonyData : AIAbstractData
{
    [field : Header("Tony specific values")]
    [field:SerializeField] public int punchPower { get; private set; }

    public override AIAbstractDataInstance Instance()
    {
        return new TonyDataInstance(this);
    }
    
}

public class TonyDataInstance : AIAbstractDataInstance
{
    public int punchPower;

    public TonyDataInstance(TonyData data) : base(data)
    {
        punchPower = data.punchPower;
    }
}