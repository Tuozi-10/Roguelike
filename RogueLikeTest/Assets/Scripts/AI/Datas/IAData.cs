using UnityEngine;

[CreateAssetMenu(fileName = "data" ,menuName = "ScriptableObjects/IA")]
public abstract class IAData : ScriptableObject
{
    [Header("Common values")] public int m_hp;
    public int m_rangeSight;
    public float m_speed;

    public IADataInstance Instance()
    {
        return new IADataInstance(this);
    }
}

public class IADataInstance
{
    public int hp;
    public int rangeSight;
    public float speed;
    public IADataInstance(IAData data)
    {
        hp = data.m_hp;
        rangeSight = data.m_rangeSight;
        speed = data.m_speed;
    }
}