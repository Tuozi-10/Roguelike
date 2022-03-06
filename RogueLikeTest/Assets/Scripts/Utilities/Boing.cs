using System;
using DG.Tweening;
using UnityEngine;

namespace Utilities
{
    public class Boing : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D other)
        {
            transform.DOScale(0.95f, 0.125f).OnComplete(()=>transform.DOScale(1, 0.175f).SetEase(Ease.OutBack, 2.5f));
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}
