using System;
using DG.Tweening;
using UnityEngine;

namespace Controller
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform m_player;
        [SerializeField] private Transform m_target;
        [SerializeField] private float durationMove = 0.35f;

        [SerializeField] private Transform m_shaker;
        
        private Transform m_transform;
        
        public static CameraController instance;

        private void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }
                
            instance = this;

            m_transform = transform;
        }

        public void SetCamAtPos(Vector2 position)
        {
            m_transform.DOKill();
            m_transform.position = new Vector3(position.x, position.y, -10);
        }

        public void ShakeCamera(float intensity = 0.75f, float duration = 0.2f)
        {
            m_shaker.DOShakePosition(duration, intensity, 50, 270);
        }
        
        public void Update()
        {
            Vector3 target = (m_target.position + m_player.position*3)/4f;
            target.z = -10;
            m_transform.DOMove(target, durationMove);
        }
    }
}