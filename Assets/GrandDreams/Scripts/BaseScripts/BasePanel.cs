using DG.Tweening;
using GrandDreams.Core.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrandDreams.Core
{
    public enum ETranfromType
    {
        None, Absolute, Relative
    }

    public class BasePanel : MonoBehaviour, IBasePanel
    {

        #region Declare Variables

        public virtual bool IsPanelChanged

        {
            get; set;
        }

        public virtual bool IsShowing
        {
            get;
            set;
        }

        public virtual bool Interactable
        {
            get
            {
                return canvasGroup.blocksRaycasts;
            }
            set
            {
                canvasGroup.blocksRaycasts = value;
            }
        }

        public virtual Action<Action> ShowConfirmMessage
        {
            get;
            set;
        }

        public ETranfromType StartScaleType = ETranfromType.None;
        public Vector3 StartScaleValue = Vector3.one;
        public ETranfromType EndScaleType = ETranfromType.None;
        public Vector3 EndScaleValue = Vector3.one;
        public ETranfromType StartPositionType = ETranfromType.None;
        public Vector2 StartPositionValue = Vector2.zero;
        public ETranfromType EndPositionType = ETranfromType.None;
        public Vector2 EndPositionValue = Vector2.zero;

        public float StartAlpha = 0;
        public float EndAlpha = 0;

        public float AnimationDurationStart = 0.35f, AnimationDurationEnd = 0.35f;
        public Ease EaseShowScaleType = Ease.OutBack, EaseHideScaleType = Ease.InBack, EaseShowPositionType = Ease.OutBack, EaseHidePositionType = Ease.InBack;

        protected RectTransform rectTransform, rectTransformRoot;
        protected CanvasGroup canvasGroup;

        protected Vector2 originPosition, startPosition, endPosition;
        protected Vector3 originScale, startScale, endScale;

        protected Sequence sequenceShow = null;

        protected object[] dataInitialized = null;

        #endregion Declare Variables

        protected virtual void Awake()
        {
            rectTransform = transform as RectTransform;
            rectTransformRoot = transform.Find("Root") as RectTransform;

            canvasGroup = GetComponent<CanvasGroup>();
            if(canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = StartAlpha;
            canvasGroup.blocksRaycasts = false;

            originPosition = rectTransformRoot.anchoredPosition;
            originScale = rectTransformRoot.localScale;

            startScale = StartScaleType == ETranfromType.None ? Vector3.one : (StartScaleType == ETranfromType.Absolute ? StartScaleValue : new Vector3(originScale.x * StartScaleValue.x, originScale.y * StartScaleValue.y, originScale.z * StartScaleValue.z));
            endScale = EndScaleType == ETranfromType.None ? startScale : (EndScaleType == ETranfromType.Absolute ? EndScaleValue : new Vector3(originScale.x * EndScaleValue.x, originScale.y * EndScaleValue.y, originScale.z * EndScaleValue.z));

            startPosition = StartPositionType == ETranfromType.None ? Vector2.zero : (StartPositionType == ETranfromType.Absolute ? StartPositionValue : rectTransformRoot.anchoredPosition + StartPositionValue);
            endPosition = EndPositionType == ETranfromType.None ? startPosition : (EndPositionType == ETranfromType.Absolute ? EndPositionValue : rectTransformRoot.anchoredPosition + EndPositionValue);
        }

        protected virtual void Start()
        {
            StartCoroutine(StartRoutine());

            Reset();
        }

        protected virtual IEnumerator StartRoutine()
        {
            yield return new WaitForEndOfFrame();
        }

        protected virtual void OnDestroy()
        {
            sequenceShow = sequenceShow.Initialize(false);

        }

        #region Public Function

        public virtual void Initialize(object[] parameters)
        {
            this.dataInitialized = parameters;
        }

        public virtual void Reset()
        {
            rectTransformRoot.anchoredPosition = startPosition;
            rectTransformRoot.localScale = startScale;
            canvasGroup.alpha = StartAlpha;
            canvasGroup.blocksRaycasts = false;
        }

        public virtual void Show(object[] parameters = null, float delayedTime = 0, System.Action onAwake = null, System.Action onStart = null, System.Action onDone = null)
        {
            if (sequenceShow != null)
            {
                sequenceShow.Kill();
                sequenceShow = null;
            }

            Reset();

            Initialize(dataInitialized);

            sequenceShow = DOTween.Sequence();

            if(onAwake != null)
            {
                onAwake();
            }

            sequenceShow.AppendCallback(() =>
            {
                if(onStart != null)
                {
                    onStart();
                }
            });

            sequenceShow.Append(rectTransformRoot.DOScale(originScale, AnimationDurationStart).SetEase(EaseShowScaleType));
            sequenceShow.Join(rectTransformRoot.DOAnchorPos(originPosition, AnimationDurationStart).SetEase(EaseShowPositionType));
            sequenceShow.Join(canvasGroup.DOFade(1, AnimationDurationStart));

            sequenceShow.SetDelay(delayedTime).OnComplete(() =>
            {
                IsShowing = true;
                canvasGroup.blocksRaycasts = true;
                if (onDone != null)
                {
                    onDone();
                }
            });
        }

        public virtual void Hide(object[] parameters = null, float delayedTime = 0, System.Action onAwake = null, System.Action onStart = null, System.Action onDone = null)
        {
            if (IsPanelChanged)
            {
                if (ShowConfirmMessage != null)
                {
                    ShowConfirmMessage(() =>
                    {
                        if (sequenceShow != null)
                        {
                            sequenceShow.Kill();
                            sequenceShow = null;
                        }

                        IsShowing = false;
                        canvasGroup.blocksRaycasts = false;
                        sequenceShow = DOTween.Sequence();

                        if (onAwake != null)
                        {
                            onAwake();
                        }

                        sequenceShow.AppendCallback(() =>
                        {
                            if (onStart != null)
                            {
                                onStart();
                            }
                        });

                        sequenceShow.Append(rectTransformRoot.DOScale(endScale, AnimationDurationEnd).SetEase(EaseHideScaleType));
                        sequenceShow.Join(rectTransformRoot.DOAnchorPos(endPosition, AnimationDurationEnd).SetEase(EaseHidePositionType));
                        sequenceShow.Join(canvasGroup.DOFade(EndAlpha, AnimationDurationEnd));

                        sequenceShow.SetDelay(delayedTime).OnComplete(() =>
                        {
                            if (onDone != null)
                            {
                                onDone();
                            }
                        });
                    });
                }
            }
            else
            {
                if (sequenceShow != null)
                {
                    sequenceShow.Kill();
                    sequenceShow = null;
                }

                sequenceShow = DOTween.Sequence();

                if (onAwake != null)
                {
                    onAwake();
                }

                sequenceShow.AppendCallback(() =>
                {
                    if (onStart != null)
                    {
                        onStart();
                    }
                });

                IsShowing = false;
                canvasGroup.blocksRaycasts = false;

                sequenceShow.Append(rectTransformRoot.DOScale(endScale, AnimationDurationEnd).SetEase(EaseHideScaleType));
                sequenceShow.Join(rectTransformRoot.DOAnchorPos(endPosition, AnimationDurationEnd).SetEase(EaseHidePositionType));
                sequenceShow.Join(canvasGroup.DOFade(EndAlpha, AnimationDurationEnd));

                sequenceShow.SetDelay(delayedTime).OnComplete(() =>
                {
                    if (onDone != null)
                    {
                        onDone();
                    }
                });
            }
        }

        public virtual void InputKeyDown(string keyString)
        {
        }

        public virtual void InputKeyUp(string keyString)
        {
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