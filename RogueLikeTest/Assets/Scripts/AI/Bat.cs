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
        [SerializeField] private float m_rangeWander = 2;
        private Vector2 basePosition;

        private bool m_moving;
        private float m_wanderingDurationRange;

        private BatDataInstance m_batData;
        
        protected override void Init()
        {
            base.Init();
            basePosition = transform.position;
            
            m_batData = (BatDataInstance)m_dataInstance;
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
            
            Vector2 newMoveTarget = new Vector2(Random.Range(basePosition.x - m_rangeWander, basePosition.x + m_rangeWander),
                    Random.Range(basePosition.y - m_rangeWander, basePosition.y + m_rangeWander));
            
            m_transform.DOMove(newMoveTarget, 1f).OnComplete(() => m_moving = false);
        }

        protected override void Attack()
        {
            base.Attack();
            
            Vector2 direction = (playerTransform.position - m_transform.position);
            direction.Normalize();
            m_rigidbody.linearVelocity = direction * m_speed;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(other.transform.GetComponent<DingIa>() != null)
                Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), other.collider);
        }
    }
}