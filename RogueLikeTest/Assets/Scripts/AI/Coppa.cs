using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    [RequireComponent(typeof(HitCollider))]
    public class Coppa : AbstractIA
    {
        private CoppaDataInstance currentCoppaData;

        protected override void Init()
        {
            currentCoppaData = (CoppaDataInstance)currentAiData;
            
            base.Init();
        }

        protected override void Attack()
        {
            base.Attack();
            
            Vector2 direction = (playerTransform.position - m_transform.position);
            direction.Normalize();
            m_rigidbody.velocity = direction * currentCoppaData.speed;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.transform.CompareTag("Player"))
            {
                Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), other.collider);
            }
                
        }
    }
}