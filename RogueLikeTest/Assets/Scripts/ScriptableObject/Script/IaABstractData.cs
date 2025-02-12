using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SciptableObject/AbstractIAData", fileName = "data")]

public abstract class IaABstractData : ScriptableObject
{
    [field: Header("Life"), SerializeField]
    public int Hp { get; private set; }

    [field: Header("Range"), SerializeField]
    public int RangeSight{ get; private set; }
    
    [field: Header("Speed"),SerializeField]
    public int Speed { get; private set; }
    
    [field: Header("Spread"),SerializeField]
    public int Spread { get; private set; }
    
    [field: Header("Count Blood"),SerializeField]
    public int CountBlood { get; private set; }

    public virtual IaABstractDataInstance Instance()
    {
        return new IaABstractDataInstance(this);
    }
}

public class IaABstractDataInstance
{
    public int hp;
    public int rangeSight;
    public int speed;
    public int spread;
    public int countBlood;

    public IaABstractDataInstance(IaABstractData data)
    {
        hp = data.Hp;
        rangeSight = data.RangeSight;
        speed = data.Speed;
        spread = data.Spread;
        countBlood = data.CountBlood;
    }
}

