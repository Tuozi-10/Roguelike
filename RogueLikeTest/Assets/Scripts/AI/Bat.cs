using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    [RequireComponent(typeof(HitCollider))]
    public class Bat : AbstractIA
    {
        [Header("Specific values")] 
        private BatData batData;
        private BatDataInstance batDataInstance;

        private Vector2 basePosition;

        private bool m_moving;
        private float m_wanderingDurationRange;

        protected override void Init()
        {
            base.Init();
            basePosition = transform.position;

            batData = (BatData)aiData;
            batDataInstance = (BatDataInstance)batData.Instance();
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
            
            Vector2 newMoveTarget = new Vector2(Random.Range(basePosition.x - batDataInstance.RangeWander, basePosition.x + batDataInstance.RangeWander),
                    Random.Range(basePosition.y - batData.RangeWander, basePosition.y + batData.RangeWander));
            
            m_transform.DOMove(newMoveTarget, 1f).OnComplete(() => m_moving = false);
        }

        protected override void Attack()
        {
            base.Attack();
            
            Vector2 direction = (playerTransform.position - m_transform.position);
            direction.Normalize();
            m_rigidbody.linearVelocity = direction * aiData.Speed;
        }
    }
}