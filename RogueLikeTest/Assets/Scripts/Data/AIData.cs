using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/EnemyData")]
public abstract class AIData : ScriptableObject
{
    [Header("Common values")] public int Hp;
    public int RangeSight;
    public float Speed;

    [Header("Death")] public float Spread;
    public int CountBlood;

    public abstract AIDataInstance Instance();
}

public class AIDataInstance
{
    public int Hp;

    public AIDataInstance(AIData data)
    {
        Hp = data.Hp;
    }
}