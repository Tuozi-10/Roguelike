using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "Data")]
public abstract class AIAbstractData : ScriptableObject
{
    [field : Header("Common values")]
    [field:SerializeField] public int hp { get; private set; }
    [field:SerializeField] public int rangeSight { get; private set; }
    [field:SerializeField] public float speed { get; private set; }

    public abstract AIAbstractDataInstance Instance();

}

public class AIAbstractDataInstance
{
    public int hp;
    public int rangeSight;
    public float speed;

    public AIAbstractDataInstance(AIAbstractData data)
    {
        hp = data.hp;
        rangeSight = data.rangeSight;
        speed = data.speed;
        
    }
}