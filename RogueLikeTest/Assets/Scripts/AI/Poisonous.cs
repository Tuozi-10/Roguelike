using AI;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(PoisonCollider))]
public class Poisonous : AbstractIA
{
    private Vector2 basePosition;
    

    private bool m_moving;
    private float m_wanderingDurationRange;
    private PoisonousData poisonous;
    private PoisonousDataInstance poisonousInstance;
    [SerializeField]private  PoisonCollider poisonCollider;
    
    protected override void Init()
    {
        base.Init();
        basePosition = transform.position;
        poisonous = (PoisonousData)enemy;
        
        poisonousInstance = (PoisonousDataInstance)poisonous.Instance();
        poisonCollider.PoisonDamage = poisonousInstance.PoisonDamage;
        poisonCollider.PoisonDuration = poisonousInstance.PoisonDuration;
        poisonCollider.PoisonDelay = poisonousInstance.PoisonDelay;
    }

    public override void ChangeState(AIStates aiState)
    {
        base.ChangeState(aiState);

        if (aiState == AIStates.wandering)
            basePosition = transform.position;
    }

    protected override void Wander()
    {
        // leave the base wander to let it go in attack state if player near
        base.Wander();

        if(m_moving)
            return;
        
        m_moving = true;
        
        Vector2 newMoveTarget = new Vector2(Random.Range(basePosition.x - poisonousInstance.RangeWander, basePosition.x + poisonousInstance.RangeWander),
                Random.Range(basePosition.y - poisonousInstance.RangeWander, basePosition.y + poisonousInstance.RangeWander));
        
        m_transform.DOMove(newMoveTarget, 1f).OnComplete(() => m_moving = false);
    }



    protected override void Attack()
    {
        base.Attack();
        
        Vector2 direction = (playerTransform.position - m_transform.position);
        direction.Normalize();
        m_rigidbody.linearVelocity = direction * poisonousInstance.Speed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.GetComponent<DingIa>() != null)
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), other.collider);
    }
}


   

