using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    [RequireComponent(typeof(HitCollider))]
    public class Bat : AbstractIA
    {
        [Header("Specific values"), Space] 
        
        private Vector2 basePosition;
        

        private bool m_moving;
        private float m_wanderingDurationRange;
        private BatData bat;
        private BatDataInstance batInstance;
        
        protected override void Init()
        {
            base.Init();
            basePosition = transform.position;
            bat = (BatData)enemy;
            batInstance = (BatDataInstance)bat.Instance();
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
            
            Vector2 newMoveTarget = new Vector2(Random.Range(basePosition.x - batInstance.RangeWander, basePosition.x + batInstance.RangeWander),
                    Random.Range(basePosition.y - batInstance.RangeWander, basePosition.y + batInstance.RangeWander));
            
            m_transform.DOMove(newMoveTarget, 1f).OnComplete(() => m_moving = false);
        }

  

        protected override void Attack()
        {
            base.Attack();
            
            Vector2 direction = (playerTransform.position - m_transform.position);
            direction.Normalize();
            m_rigidbody.linearVelocity = direction * batInstance.Speed;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(other.transform.GetComponent<DingIa>() != null)
                Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), other.collider);
        }
    }
}