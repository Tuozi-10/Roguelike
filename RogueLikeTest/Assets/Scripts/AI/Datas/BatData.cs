using AI;
using UnityEngine;

[CreateAssetMenu(fileName = "data" ,menuName = "ScriptableObjects/Bat")]
public class BatData : IAData
{
    [Header("Specific values"), SerializeField] public float m_rangeWander;

    public BatDataInstance Instance()
    {
        return new BatDataInstance(this);
    }
}

public class BatDataInstance : IADataInstance
{
    public float rangeWander;
    public BatDataInstance(BatData data) : base(data)
    {
        rangeWander = data.m_rangeWander;
    }
}