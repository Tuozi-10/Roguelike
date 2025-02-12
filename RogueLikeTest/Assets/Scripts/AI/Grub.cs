using System;
using Controller;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    [RequireComponent(typeof(HitCollider))]
    public class Grub : AbstractIA
    {
        enum Direction
        {
            Up = 0,
            Down = 1,
            Left = 2,
            Right = 3
        }

        [SerializeField] private Animator animator;
        
        [Header("Specific values")] 
        private BatData batData;
        private BatDataInstance batDataInstance;

        private Vector2 basePosition;
        
        private bool m_moving;
        private float m_wanderingDurationRange;
        private static readonly int Direction1 = Animator.StringToHash("direction");

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

            newMoveTarget = DeleteLessSignificantDirection(newMoveTarget);
            UpdateAnimator(newMoveTarget);
            m_transform.DOMove(newMoveTarget, 1f).OnComplete(() => m_moving = false);
        }

        protected override void Attack()
        {
            base.Attack();
            
            Vector2 direction = (playerTransform.position - m_transform.position);
            direction.Normalize();
            direction = DeleteLessSignificantDirection(direction);
            UpdateAnimator(direction);
            m_rigidbody.linearVelocity = direction * aiData.Speed;
        }

        private Vector2 DeleteLessSignificantDirection(Vector2 move)
        {
            if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
            {
                return new Vector2(move.x, 0);
            }
            else
            {
                return new Vector2(0, move.y);
            }
        }

        private void UpdateAnimator(Vector2 move)
        {
            Direction direction;

            if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
            {
                direction = move.x > 0 ? Direction.Right : Direction.Left;
            }
            else
            {
                direction = move.y > 0 ? Direction.Up : Direction.Down;
            }

            if (animator.GetInteger(Direction1) != (int)direction)
            {
                animator.SetInteger(Direction1, (int)direction);
            }
        }
    }
}