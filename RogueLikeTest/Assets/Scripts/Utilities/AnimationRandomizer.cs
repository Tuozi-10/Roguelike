using UnityEngine;

namespace Utilities
{
    
    /// <summary>
    /// Add a random on the first animation first frame to avoid having all environment anims playing same anim in same time ( so Nintendo 64 behavior )
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class AnimationRandomizer : MonoBehaviour
    {

        [SerializeField] private float m_randomSpeedRange = 0.05f;
    
        void Start()
        {
            var animator = GetComponent<Animator>();
            var currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
            var currentAnimName = currentClipInfo[0].clip.name;
        
            animator.Play(currentAnimName, 0, Random.Range(0,1f));
            animator.speed = Random.Range(1 - m_randomSpeedRange, 1 + m_randomSpeedRange);
        }
    }
}
