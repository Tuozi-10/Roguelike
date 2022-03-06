using System.Collections.Generic;
using DG.Tweening;
using Menus;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class LifeManager : MonoBehaviour
    {
        [SerializeField] private Image lifeIcon;
        [SerializeField] private List<Image> currentIcons;

        public static LifeManager Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            Instance = this;
        }

        private int currentLife;

        public void UpdateLife(int newLife)
        {
            if (newLife <= 0)
            {
                MenuManager.instance.DoLoose();
                newLife = 0;
            }
            
            while(currentIcons.Count < newLife)
                currentIcons.Add(Instantiate(lifeIcon, this.transform));
            
            if (newLife > currentLife)
            {
                for (var i = currentLife; i < newLife; i++)
                {
                    DoSpawnEffect(currentIcons[i]);
                }
            }
            else if (newLife < currentLife)
            {
                for (var i = newLife; i < currentLife; i++) 
                {
                    DoUnSpawnEffect(currentIcons[i]);
                }
            }

            currentLife = newLife;
        }

        private void DoSpawnEffect(Image icon)
        {
            icon.DOKill();
            icon.rectTransform.DOKill();

            icon.DOFade(1, 0.35f);
            icon.GetComponent<RectTransform>().DOScale(1, 0.35f).SetEase(Ease.OutBack);
        }
        
        private void DoUnSpawnEffect(Image icon)
        {
            icon.DOKill();
            icon.rectTransform.DOKill();

            icon.DOFade(0, 0.35f);
            icon.GetComponent<RectTransform>().DOScale(0, 0.3f).SetEase(Ease.InBack);
        }
        
    }
}
