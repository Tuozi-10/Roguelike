using AI;
using Controller;
using DG.Tweening;
using UnityEngine;

namespace Projectiles
{
    public class ProjectileBase : MonoBehaviour
    {
        [SerializeField] protected float m_speed = 10f;
        [SerializeField] private int damage = 1;

        [SerializeField] private float m_amplitudeY = 1f;
        [SerializeField] private float m_durationBezier = .15f;
        [SerializeField] private Transform m_childTransform;
        
        protected Rigidbody2D m_rb;

        public void Initialize(Vector2 direction, bool fakeBezier, bool odd )
        {
            ProjectileManager.projectilesList.Add(this);
            
            Initialize(direction);
            
            m_childTransform.DOKill();
            m_childTransform.localPosition = Vector3.zero;
            
            if (fakeBezier) DoBezier(odd ? m_amplitudeY : -m_amplitudeY);
        }

        private void DoBezier(float amplitude)
        {
            m_childTransform.DOLocalMove(new Vector3(0, amplitude, 0), m_durationBezier).SetEase(Ease.InOutSine).OnComplete(()=> DoBezier(-amplitude));
        }
        
        public virtual void Initialize(Vector2 direction)
        {
            if(m_rb == null) m_rb = GetComponentInChildren<Rigidbody2D>();
            m_rb.velocity = direction * m_speed;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<AbstractIA>().LooseHp(damage);
            }
            
            ProjectileManager.instance.RequestPoof(transform.position);
            ImpactBehaviour();
        }

        /// <summary>
        /// let's add a overridable impact behaviour to let other bullets add idk, poison, stunt, bounding etc
        /// </summary>
        public virtual void ImpactBehaviour()
        {
            PlayerController.instance.AddToPool(gameObject);
        }
    }
}