using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDreams.Core
{
    public class BlackImage : MonoBehaviour
    {

        #region Declare Variables

        public static BlackImage Instance { get; private set; }

        private Image imageBlack;

        private Sequence sequenceShow;

        #endregion Declare Variables

        private void Awake()
        {
            Instance = this;

            imageBlack = GetComponent<Image>();
            imageBlack.raycastTarget = true;
        }

        private void OnDestroy()
        {
            Instance = null;

            if (sequenceShow != null)
            {
                sequenceShow.Kill();
                sequenceShow = null;
            }
        }

        #region Public Function

        public void Show(float delayedTime = 0f, System.Action onStart = null, System.Action onDone = null)
        {
            if (sequenceShow != null)
            {
                sequenceShow.Kill();
                sequenceShow = null;
            }

            sequenceShow = DOTween.Sequence();

            sequenceShow.AppendCallback(() =>
            {
                imageBlack.raycastTarget = true;
                if(onStart != null)
                {
                    onStart();
                }
            });

            sequenceShow.Append(imageBlack.DOFade(1, 0.25f));
            sequenceShow.SetDelay(delayedTime).OnComplete(() =>
            {
                if (onDone != null)
                {
                    onDone();
                }
            });
        }

        public void Hide(float delayedTime = 0f, System.Action onStart = null, System.Action onDone = null)
        {
            if (sequenceShow != null)
            {
                sequenceShow.Kill();
                sequenceShow = null;
            }

            sequenceShow = DOTween.Sequence();

            sequenceShow.AppendCallback(() =>
            {
                if (onStart != null)
                {
                    onStart();
                }
            });

            sequenceShow.Append(imageBlack.DOFade(0, 0.25f));

            sequenceShow.SetDelay(delayedTime).OnComplete(() =>
            {
                imageBlack.raycastTarget = false;
                if (onDone != null)
                {
                    onDone();
                }
            });
        }

        #endregion Public Function
    }
}