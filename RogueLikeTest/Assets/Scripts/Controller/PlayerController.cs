using System;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using Maps;
using Projectiles;
using Ui;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Utilities;
using static Bonuses.Bonus;

namespace Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Targetting m_target;
        [SerializeField] private float m_speed = 0.1f;
        [SerializeField] private float m_dashSpeed = 35f;
        [SerializeField] private float m_durationDash = 0.35f;
        [SerializeField] private GameObject m_projectile;
        [SerializeField] private GameObject m_projectileCutter;
        [SerializeField] private TrailRenderer[] m_trails;
        [SerializeField] private GameObject m_parentSkinDaguer;
        [SerializeField] private GameObject m_parentSkinBezier;
        public int PoisonDamage;
        public float PoisonDuration;
        public float LastPoisonTime;
        public float PoisonDelay;
        private float poisonStart = -100000;

        private float m_timerDash = 0f;

        public static PlayerController instance;

        private Animator m_animator;
        private SpriteRenderer m_spriteRenderer;

        [SerializeField] private SpriteRenderer m_spriteRendererHead;
        [SerializeField] private List<Sprite> m_heads;

        [SerializeField] private int m_lifePoints = 3;
        private int m_currentLife;
        private Rigidbody2D m_rigidbody;
        private bonusType m_currentBonus = bonusType.none;

        [SerializeField] private float m_durationImu = 0.25f;
        
        private float m_currentImu;

        public void ApplyBonus(bonusType newBonus, int param = 0)
        {
            switch (newBonus)
            {
                case bonusType.Health:
                    m_currentLife += param;
                    LifeManager.Instance.UpdateLife(m_currentLife);
                    return;
                case bonusType.Money:
                    GameManager.Money += param;
                    return;
               
                default:
                    m_currentBonus = newBonus;

                    switch (newBonus)
                    {
                        case bonusType.none:
                            m_parentSkinBezier.SetActive(false);
                            m_parentSkinDaguer.SetActive(false);
                            break;
                        case bonusType.Cutter:
                            m_parentSkinBezier.SetActive(false);
                            m_parentSkinDaguer.SetActive(true);
                            break;
                        case bonusType.Bezier:
                            m_parentSkinBezier.SetActive(true);
                            m_parentSkinDaguer.SetActive(false);
                            break;
                    }

                    break;
            }
        }

        private void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            instance = this;
            m_animator = GetComponent<Animator>();
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            m_rigidbody = GetComponent<Rigidbody2D>();
        }

        public void ResetVelocity()
        {
            m_rigidbody.linearVelocity = Vector2.zero;
        }
        
        private void Start()
        {
            ReInit();
        }

        public void ReInit()
        {
            ApplyBonus(bonusType.none);
            transform.position = Vector3.zero;
            m_currentLife = m_lifePoints;
            LifeManager.Instance.UpdateLife(m_currentLife);
            poisonStart = -1000;

        }

        private Queue<GameObject> m_pool = new Queue<GameObject>();
        private Queue<GameObject> m_poolCutter = new Queue<GameObject>();

        private void Update()
        {
            if (GameManager.InMenu)
                return;
            HitPoison(PoisonDamage);
            m_currentImu -= Time.deltaTime;
            
            ManageMove();
            ManageHeadSprite();
            if (Input.GetMouseButtonDown(0)) Shoot();
        }

        private void ManageMove()
        {
            var speed = m_timerDash <= 0 ? m_speed : m_dashSpeed;
            bool moved = false;

            int nbInputs = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.A) ? 1 : 0) +
                           (Input.GetKey(KeyCode.S) ? 1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0);
            if (nbInputs > 1) speed *= 0.75f;

            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.up * speed * Time.deltaTime);
                m_animator.Play("PlayerWalkUp");
                moved = true;
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
                m_spriteRenderer.flipX = true;
                if (!moved) m_animator.Play("PlayerWalkSide");
                moved = true;
            }

            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime);
                if (!moved) m_animator.Play("PlayerWalkBottom");
                moved = true;
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                if (!moved) m_animator.Play("PlayerWalkSide");
                m_spriteRenderer.flipX = false;
                moved = true;
            }

            if (!moved)
                m_animator.Play("PlayerIdle");

            if (Input.GetKeyDown(KeyCode.Space) && m_timerDash < -0.5f)
            {
                m_timerDash = m_durationDash;
            }

            m_timerDash -= Time.deltaTime;

            foreach (var trailRenderer in m_trails)
            {
                trailRenderer.emitting = m_timerDash > 0;
            }
        }

        private void ManageHeadSprite()
        {
            var position = transform.position;
            var positionTarget = m_target.transform.position;

            GetAngle(positionTarget, position, out float angle);
            angle += 180;

            if (angle < 330 && angle > 240) m_spriteRendererHead.sprite = m_heads[0];
            else if (angle > 330 || angle < 30) m_spriteRendererHead.sprite = m_heads[1];
            else if (angle > 30 && angle < 140) m_spriteRendererHead.sprite = m_heads[2];
            else m_spriteRendererHead.sprite = m_heads[3];
        }

        private void Shoot(bool checkBezierRecursive = true)
        {
            Debug.Log("Shoot");
            if (checkBezierRecursive && m_currentBonus == bonusType.Bezier) // double shoot for bezier
                Shoot(false);

            var go = m_currentBonus == bonusType.Cutter ? GetFromPoolCutter() : GetFromPool();
            go.transform.position = transform.position + Vector3.up / 2f;
            go.transform.SetParent(null);

            var position = go.transform.position;
            var positionTarget = m_target.transform.position;

            go.GetComponent<ProjectileBase>().Initialize(Vector3.Normalize(positionTarget - position),
                m_currentBonus == bonusType.Bezier, checkBezierRecursive);

            GetAngle(positionTarget, position, out float angle);
            go.transform.rotation = Quaternion.Euler(0, 0, angle);
            
            AudioManager.instance.PlaySound(AudioManager.sounds.shoot, 0.125f);
        }

        private void GetAngle(Vector2 from, Vector2 to, out float angle)
        {
            float angleRad = Mathf.Atan2(to.y - from.y, to.x - from.x);
            angle = (180 / Mathf.PI) * angleRad;
        }

        public void Hit(int damages)
        {
            if (m_currentImu > 0 || m_timerDash > 0) return;
            m_currentImu = m_durationImu;
            
            AudioManager.instance.PlaySound(AudioManager.sounds.hit);
            CameraController.instance.ShakeCamera();

            m_currentLife -= damages;
            m_spriteRenderer.DOColor(Color.red, 0.15f).OnComplete(() => m_spriteRenderer.DOColor(Color.white, 0.15f));
            m_spriteRendererHead.DOColor(Color.red, 0.15f).OnComplete(() => m_spriteRendererHead.DOColor(Color.white, 0.15f));

            m_spriteRenderer.DOFade(0.25f, 0.15f).OnComplete(()=> m_spriteRenderer.DOFade(1, 0.15f));
            m_spriteRendererHead.DOFade(0.25f, 0.15f).OnComplete(()=> m_spriteRendererHead.DOFade(1, 0.15f));
            
            LifeManager.Instance.UpdateLife(m_currentLife);
        }

        public void StartPoison(int damage, float duration,float lastPoisonTime, float delay)
        {
            poisonStart = Time.time;
            PoisonDamage = damage; 
            PoisonDuration = duration;
            LastPoisonTime = lastPoisonTime;
            PoisonDelay = delay;
        }
        public void HitPoison(int damages)
        {
            
            if (Time.time - LastPoisonTime > PoisonDelay && Time.time - poisonStart < PoisonDuration )
            {
            
                AudioManager.instance.PlaySound(AudioManager.sounds.hit);
                CameraController.instance.ShakeCamera();

                m_currentLife -= damages;
                m_spriteRenderer.DOColor(Color.red, 0.15f).OnComplete(() => m_spriteRenderer.DOColor(Color.white, 0.15f));
                m_spriteRendererHead.DOColor(Color.red, 0.15f).OnComplete(() => m_spriteRendererHead.DOColor(Color.white, 0.15f));

                m_spriteRenderer.DOFade(0.25f, 0.15f).OnComplete(()=> m_spriteRenderer.DOFade(1, 0.15f));
                m_spriteRendererHead.DOFade(0.25f, 0.15f).OnComplete(()=> m_spriteRendererHead.DOFade(1, 0.15f));
        
                LifeManager.Instance.UpdateLife(m_currentLife);
                LastPoisonTime = Time.time;
                
            }
            
            
        }

        public GameObject GetFromPool()
        {
            if (m_pool.Count == 0)
                return Instantiate(m_projectile);

            var proj = m_pool.Dequeue();
            proj.transform.position = transform.position + Vector3.up / 2f;
            proj.SetActive(true);
            return proj;
        }
        
        public void AddToPool(GameObject go)
        {
            go.transform.DOKill();
            go.SetActive(false);

            if (m_pool.Contains(go)) return;

            m_pool.Enqueue(go);
        }

        public GameObject GetFromPoolCutter()
        {
            if (m_poolCutter.Count == 0)
                return Instantiate(m_projectileCutter);

            var proj = m_poolCutter.Dequeue();
            proj.transform.position = transform.position + Vector3.up / 2f;
            proj.SetActive(true);
            return proj;
        }

        public void AddToPoolCutter(GameObject go)
        {
            go.transform.DOKill();
            go.SetActive(false);

            if (m_poolCutter.Contains(go)) return;

            m_poolCutter.Enqueue(go);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Door"))
            {
                MapManager.instance.DisplayNextMap(other.GetComponent<Door>().door);
            }
        }
    }
}