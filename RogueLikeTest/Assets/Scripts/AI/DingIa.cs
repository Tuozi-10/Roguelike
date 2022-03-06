using System.Collections;
using Controller;
using UnityEngine;

namespace AI
{
    public class DingIa : AbstractIA
    {
        private Animator m_animator;
        protected override void Init()
        {
            base.Init();
            m_animator = GetComponent<Animator>();
        }
        
        private bool IsAttacking;
        
        protected override void Attack()
        {
            base.Attack();

            if (IsAttacking)
                return;
            IsAttacking = true;

            StartCoroutine(DoAttack());
        }

        private IEnumerator DoAttack()
        {
            m_animator.Play("dinga",0,0);
            Vector2 direction = (playerTransform.position - m_transform.position);
            direction.Normalize();
            
            m_rigidbody.velocity = direction * m_speed;

            yield return new WaitForSeconds(0.32f);
            m_rigidbody.velocity = Vector2.zero;
            
            yield return new WaitForSeconds(0.15f);
            CameraController.instance.ShakeCamera(0.35f, 0.15f);
            
            yield return new WaitForSeconds(0.5f);

            IsAttacking = false;
        }
    }
}
