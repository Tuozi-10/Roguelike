using UnityEngine;

[CreateAssetMenu(fileName = "Poisonous", menuName = "Scriptable Objects/Poisonous")]
public class PoisonousData : BatData
{
    [Header("specific")]
    public int PoisonDamage;
    public float PoisonDuration;
    public float PoisonDelay;

    public new PoisonousDataInstance Instance() => new PoisonousDataInstance(this);
}

public class PoisonousDataInstance : BatDataInstance
{
    public int PoisonDamage;
    public float PoisonDuration;
    public float PoisonDelay;
    public PoisonousDataInstance(PoisonousData data) : base(data)
    {
        PoisonDamage = data.PoisonDamage;
        PoisonDuration = data.PoisonDuration;
        PoisonDelay = data.PoisonDelay;
    }
}
