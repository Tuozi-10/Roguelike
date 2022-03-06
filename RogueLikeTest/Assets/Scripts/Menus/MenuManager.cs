using System;
using Controller;
using DefaultNamespace;
using DG.Tweening;
using Maps;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_buttonPlay;
        [SerializeField] private CanvasGroup m_transition;
        [SerializeField] private TMP_Text m_text;
        [SerializeField] private TMP_Text m_textMoney;
        [SerializeField] private int m_startMoney = 10;

        public static MenuManager instance;
        
        private void Awake()
        {
            GameManager.InMenu = true;

            if (instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            instance = this;
            GameManager.Money = m_startMoney;
        }

        public void ClickPlay()
        {
            AudioManager.instance.PlaySound(AudioManager.sounds.play, 0.5f);
            PlayerController.instance.ResetVelocity();
            
            m_buttonPlay.SetActive(false);
            GameManager.InMenu = false;
            m_text.DOFade(0, 0.2f);
            MapManager.instance.GenerateProceduralMap();
            MapManager.instance.DisplayNextMap(MapManager.doors.bottom);
            PlayerController.instance.ReInit();
        }

        public void DoWin()
        {
            if (GameManager.InMenu)
                return;
            
            GameManager.Money = m_startMoney;
            GameManager.InMenu = true;
            m_transition.DOFade(1, 0.25f).OnComplete(() =>
            {
                m_text.text = "You Won ... ?";
                m_text.DOFade(1, 0.25f).OnComplete(()=> m_buttonPlay.SetActive(true));
            });
        }

        public void DoLoose()
        {
            if (GameManager.InMenu)
                return;
            
            GameManager.Money = m_startMoney;
            GameManager.InMenu = true;
            m_transition.DOFade(1, 0.25f).OnComplete(() =>
            {
                m_text.text = "You lost ...";
                m_text.DOFade(1, 0.25f).OnComplete(()=> m_buttonPlay.SetActive(true));
            });
        }

        public void UpdateMoney(int money)
        {
            m_textMoney.text = $"x{money}";
        }
        
    }
}