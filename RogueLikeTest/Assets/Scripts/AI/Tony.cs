using System;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    [RequireComponent(typeof(HitCollider))]
    public class Tony : AbstractIA
    {
        private TonyDataInstance currentTonyData;

        protected override void Init()
        {
            currentTonyData = (TonyDataInstance)currentAiData;
            
            base.Init();
        }

        public override void ChangeState(AIStates aiState)
        {
            base.ChangeState(aiState);
            
            if (aiState == AIStates.attacking)
            {
                //ajouter un son
            }
        }

        protected override void Attack()
        {
            base.Attack();
            
            Vector2 direction = (playerTransform.position - m_transform.position);
            direction.Normalize();
            m_rigidbody.velocity = direction * currentTonyData.speed;
        }

        [SerializeField] private GameObject coppaPrefab;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector3 spawnPosition = transform.position;

                    spawnPosition += new Vector3(i, 0, 0);
                    
                    Instantiate(coppaPrefab, spawnPosition, quaternion.identity);
                }

                Vector2 projectionDirection;
                projectionDirection = transform.position - other.transform.position;
                projectionDirection = projectionDirection.normalized;
                m_rigidbody.AddForce(projectionDirection * currentTonyData.punchPower);
            }
                
        }
    }
}