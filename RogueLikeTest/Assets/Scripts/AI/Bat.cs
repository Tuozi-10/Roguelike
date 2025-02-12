using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    [RequireComponent(typeof(HitCollider))]
    public class Bat : AbstractIA
    {
        private BatDataInstance currentBatData;
        
        [Header("Specific values"), Space]
        private Vector2 basePosition;

        private bool m_moving;
        private float m_wanderingDurationRange;

        protected override void Init()
        {
            currentBatData = (BatDataInstance)currentAiData;
            
            base.Init();
            basePosition = transform.position;
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
            
            Vector2 newMoveTarget = new Vector2(Random.Range(basePosition.x - currentBatData.rangeWander, basePosition.x + currentBatData.rangeWander),
                    Random.Range(basePosition.y - currentBatData.rangeWander, basePosition.y + currentBatData.rangeWander));
            
            m_transform.DOMove(newMoveTarget, 1f).OnComplete(() => m_moving = false);
        }

        protected override void Attack()
        {
            base.Attack();
            
            Vector2 direction = (playerTransform.position - m_transform.position);
            direction.Normalize();
            m_rigidbody.velocity = direction * currentBatData.speed;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(other.transform.GetComponent<DingIa>() != null)
                Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), other.collider);
        }
    }
}