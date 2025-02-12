using System;
using System.Collections;
using Bonuses;
using Controller;
using DG.Tweening;
using Maps;
using UnityEngine;
using Utilities;
using static AI.AbstractIA.AIStates;
using Random = UnityEngine.Random;

namespace AI
{
    /// <summary>
    /// the abstract AI class is here to manage all the things our AIs will have in common: hp, behavior states, and so on
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class AbstractIA : MonoBehaviour
    {
        [Header("Common values"), Space]
        [SerializeField] private int m_hp = 1;
        [SerializeField] private int m_rangeSight     = 10;
        [SerializeField] protected float m_speed = 10;
        [SerializeField] private SpriteRenderer m_body;

        [SerializeField] private AbstractData m_data; 
        protected AbstractDataInstance m_dataInstance;
        
        #region effects applied ON ia
        
        [Header("Effects"), Space]
        [SerializeField] private float m_stuntValue   = 0;
        
        [SerializeField] private int m_poisonStrength = 0;
        [SerializeField] private int m_poisonDuration = 0;

        #endregion

        [Header("Death"), Space]
        [SerializeField] private float m_spread = 0.5f;
        [SerializeField] private int m_countBlood = 5;
        
        public enum AIStates
        {
            wandering, 
            attacking, 
            dead
        }

        protected AIStates m_currentAiState = wandering;

        protected PlayerController player;
        protected Transform playerTransform;
        protected Transform m_transform;
        protected Rigidbody2D m_rigidbody;

        /// <summary>
        /// always put the assignment from instances in start, and in awake the singleton setters to avoid trying to get them before assigning them
        /// </summary>
        private void Start()
        {
            m_dataInstance = m_data.Instance();
            
            Init();
        }

        private bool init;
        
        protected virtual void Init()
        {
            if (init) return;
            init = true;
            
            player = PlayerController.instance;
            playerTransform = player.transform;
            m_transform = transform;
            m_rigidbody = GetComponent<Rigidbody2D>();
        }
        
        public void Update()
        {
            BehaviourIA();
        }

        #region state Machine

        public virtual void ChangeState(AIStates aiState)
        {
            if (!init) Init();
            
            if (m_currentAiState == dead)
                return; // no walking dead
            
            m_currentAiState = aiState;
            // do things for when it changes here
        }
        
        protected virtual void BehaviourIA()
        {
            switch (m_currentAiState)
            {
                case attacking : Attack();
                    break;
                case wandering: Wander();
                    break;
                case dead: Die();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void Wander()
        {
            if (Vector2.Distance(playerTransform.position, m_transform.position) < m_rangeSight)
                ChangeState(attacking);
        }

        protected virtual void Attack()
        {
            
        }

        protected void Die()
        {
            BloodBathManager.instance.RequestBlood(m_transform.position, m_countBlood, m_spread);
            // display fx/anim death here
            Destroy(gameObject);
            MapManager.instance.CheckMapCompleted();

            AudioManager.instance.PlaySound(AudioManager.sounds.goreWet, 0.25f);
            
            var random = Random.Range(0, 100);

            if(random < 4) BonusManager.instance.RequestBonusAtPosition(Bonuses.Bonus.bonusType.Health, transform.position, 1);
            else if(random < 5) BonusManager.instance.RequestBonusAtPosition(Bonuses.Bonus.bonusType.Cutter, transform.position);
            else if(random < 6) BonusManager.instance.RequestBonusAtPosition(Bonuses.Bonus.bonusType.Bezier, transform.position);
            else if(random < 11) BonusManager.instance.RequestBonusAtPosition(Bonuses.Bonus.bonusType.Money, transform.position, 10);
        }

        #endregion
        
        #region public methods

        public void LooseHp(int count)
        {
            m_hp -= count;
            
            BloodBathManager.instance.RequestBloodPoof(m_transform.position);
            
            if(m_hp <= 0)
                ChangeState(dead);
            else
            {
                if(m_currentAiState == wandering) // in case u noob are camping snip
                    ChangeState(attacking);
                
                m_body.DOColor(Color.red, 0.05f).OnComplete(() => m_body.DOColor(Color.white, 0.05f));
                m_body.DOFade(0.25f, 0.05f).OnComplete(()=> m_body.DOFade(1, 0.05f));
            }
            
        }

        private void OnDestroy()
        {
            m_body.DOKill();
            m_transform.DOKill();
        }

        #endregion

        #region properties

        public bool isDead => m_currentAiState == dead;
        
        #endregion
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, m_rangeSight);
        }

        public override string ToString()
        {
            return $"{name} {m_hp}";
        }
    }
}
