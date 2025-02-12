using System.Collections;
using Controller;
using DG.Tweening;
using UnityEngine;

namespace Projectiles
{
    public class ProjectileCutter : ProjectileBase
    {
        public override void Initialize(Vector2 direction)
        {
            base.Initialize(direction);
            StartCoroutine(DoCutterBehavior());
        }

        private IEnumerator DoCutterBehavior()
        {
            yield return new WaitForSeconds(0.175f);
            transform.DORotate(new Vector3(0, 0, transform.localEulerAngles.z + 180 + Random.Range(-2,+2)),0.05f);

            while (true)
            {
                yield return null;
                m_rb.linearVelocity = -transform.right * m_speed;
            }
        }
        
        public override void ImpactBehaviour()
        {
            PlayerController.instance.AddToPoolCutter(gameObject);
        }
    }
}