using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public abstract class EnemyData : ScriptableObject
{
    [Header("Common Values")]
    public int Health;
    public int RangeSight;
    public float Speed;
    
    
    [Header("Dead")]
    public float Spread;
    public int CountBlood;


    public EnemyDataInstance Instance() => new EnemyDataInstance(this);


}

public class EnemyDataInstance
{
    public int Health;
    public int RangeSight;
    public float Speed;
    public float Spread;
    public int CountBlood;
    

   public EnemyDataInstance(EnemyData data)
   {
       Health = data.Health;
       RangeSight = data.RangeSight;
       Speed = data.Speed;
       Spread = data.Spread;
       CountBlood = data.CountBlood;
   }

}
