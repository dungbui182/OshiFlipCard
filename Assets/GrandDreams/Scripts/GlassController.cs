using DG.Tweening;
using GrandDreams.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDreams.Game.SoliteKinect2023
{
    public class GlassController : MonoBehaviour
    {
        #region Declare Variables
        public static GlassController Instance { get; private set; }

        private float positionX = 0;
        public float PositionX
        {
            get
            {
                return positionX;
            }
            set
            {
                positionX = value;
                float tempPositionX = Mathf.Lerp(rtGlass.anchoredPosition.x, positionX, Time.deltaTime * lerpSpeed);
                rtGlass.SetAnchoredPosX(tempPositionX);
            }
        }

        [SerializeField] private RectTransform rtGlass;
        [SerializeField] private Collider2D glassCollider;
       

        private RectTransform rectTransform;

        private Sequence sequenceShow;
        private float lerpSpeed;

        private float originPosY;

        #endregion Declare Variables

        private void Awake()
        {
            Instance = this;

            rectTransform = transform as RectTransform;

            originPosY = rtGlass.anchoredPosition.y;

            //lerpSpeed = ConfigData.Data.LerpSpeed;
    }

        private void Start()
        {
            Reset();
        }

        private void OnDestroy()
        {
            sequenceShow = sequenceShow.Initialize(false);
        }

        private void FixedUpdate()
        {
            rtGlass.SetAnchoredPosY(originPosY);
        }

        #region Public Function

        public void Reset()
        {
            rectTransform.anchoredPosition = new Vector2(0, -400);
        }

        public void DisableCollider()
        {
            glassCollider.enabled = false;
        }

        public void ShowFromBottom(float timeDuration = 1, float delayedTime = 0, System.Action onDone = null)
        {
            rectTransform.anchoredPosition = new Vector2(0, -400);
            PositionX = 0;

            sequenceShow = sequenceShow.Initialize();
            sequenceShow.Append(rectTransform.DOAnchorPos(Vector2.zero, timeDuration).SetEase(Ease.OutQuad));
            sequenceShow.SetDelay(delayedTime).OnComplete(() =>
            {
                if(onDone != null)
                {
                    onDone();
                }
            });
        }

        public void HideToBottom(float timeDuration = 1, float delayedTime = 0, System.Action onDone = null)
        {
            sequenceShow = sequenceShow.Initialize();
            sequenceShow.Append(rectTransform.DOAnchorPos(new Vector2(0, -400), timeDuration).SetEase(Ease.InQuad));
            sequenceShow.Join(rtGlass.DOAnchorPos(Vector2.zero, timeDuration).SetEase(Ease.InQuad));
            sequenceShow.SetDelay(delayedTime).OnComplete(() =>
            {
                if (onDone != null)
                {
                    onDone();
                }
            });
        }

        public void ShowFromLeft(float timeDuration = 1, float delayedTime = 0, System.Action onDone = null)
        {
            rectTransform.anchoredPosition = Vector2.zero;
            PositionX = -1300;

            sequenceShow = sequenceShow.Initialize();
            sequenceShow.Append(rtGlass.DOAnchorPosX(0, timeDuration).SetEase(Ease.OutQuad));
            sequenceShow.SetDelay(delayedTime).OnComplete(() =>
            {
                if (onDone != null)
                {
                    onDone();
                }
            });
        }

        public void HideToRight(float timeDuration = 1, float delayedTime = 0, System.Action onDone = null)
        {
            rectTransform.anchoredPosition = Vector2.zero;

            sequenceShow = sequenceShow.Initialize();
            sequenceShow.Append(rtGlass.DOAnchorPosX(1300, timeDuration).SetEase(Ease.InQuad));
            sequenceShow.SetDelay(delayedTime).OnComplete(() =>
            {
                if (onDone != null)
                {
                    onDone();
                }
            });
        }

        public void HideToAbove(float timeDuration = 1, float delayedTime = 0, System.Action onDone = null)
        {
            rectTransform.anchoredPosition = Vector2.zero;

            sequenceShow = sequenceShow.Initialize();
            sequenceShow.Append(rtGlass.DOScale(2, timeDuration).SetEase(Ease.InQuad));
            sequenceShow.Join(rtGlass.GetComponent<CanvasGroup>().DOFade(0f, timeDuration).SetEase(Ease.InQuad));
            sequenceShow.SetDelay(delayedTime).OnComplete(() =>
            {
                if (onDone != null)
                {
                    onDone();
                }
            });
        }



        #endregion Public Function

        #region Private Function



        #endregion Private Function

        #region Event



        #endregion Event

        #region Editor



        #endregion Editor



    }
}