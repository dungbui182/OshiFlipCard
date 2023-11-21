using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GrandDreams.Core.Utilities;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using GrandDreams.Common.Unity.Utilities;
using UnityEngine.Serialization;

namespace GrandDreams.Game.FlipCard
{
    public class TimerController : MonoBehaviour
    {
        #region Declare Variables

        public System.Action OnTimerController_TimeUp = delegate { };

        private float runningTime;
        public float RunningTime
        {
            get => runningTime;
            set
            {
                runningTime = value;

            
                textTimer.text = string.Format("{0:00}", runningTime);
            }
        }

        [SerializeField] private Text textTimer;
        [SerializeField] private Image imageTimerFill;

        private float timeDuration;

        private Sequence sequence;
        private Tween tweenTimer;

        #endregion Declare Variables

        private void Awake()
        {
            Reset();
        }

        private void OnDestroy()
        {
            if (sequence != null)
            {
                sequence.Kill();
                sequence = null;
            }

            if (tweenTimer != null)
            {
                tweenTimer.Kill();
                tweenTimer = null;
            }

            OnTimerController_TimeUp = delegate { };
        }

        #region Public Function

        public void Reset()
        {
        }

        public void Initialize(float timeDuration)
        {
            this.timeDuration = timeDuration;

            RunningTime = timeDuration;
        }

        public void Show(float delayedTime = 0, System.Action onDone = null)
        {
            if (sequence != null)
            {
                sequence.Kill();
                sequence = null;
            }

            sequence = DOTween.Sequence();
            sequence.Final(delayedTime, onDone);
        }

        public void Hide(float delayedTime = 0, System.Action onDone = null)
        {
            if (sequence != null)
            {
                sequence.Kill();
                sequence = null;
            }

            sequence = DOTween.Sequence();
            sequence.Final(delayedTime, onDone);
        }

        int currentTime = 0;
        public void Run()
        {
            if (tweenTimer != null)
            {
                tweenTimer.Kill();
                tweenTimer = null;
            }
            // xét thời gian đổ đầy fill 
            RunningTime = timeDuration;
            tweenTimer = DOTween.To(() => RunningTime, t =>
            {
                RunningTime = t;
                int intRunningTime = (int)t;
                if (currentTime != intRunningTime)
                {
                    currentTime = intRunningTime;
                    
                    float fillSizeX = 90 + ((timeDuration - (runningTime -1)) * 598 / timeDuration);
                    imageTimerFill.rectTransform.DOSizeDelta(new Vector2(fillSizeX,93.5f), 1f).SetEase(Ease.Linear);
                    
                    if (currentTime < 3)
                    {
                        if (SoundManager.SharedInstance != null)
                        {
                            SoundManager.SharedInstance.PlaySfx("Counter");
                        }
                    }
                }

            }, 0, timeDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (OnTimerController_TimeUp != null)
                {
                    OnTimerController_TimeUp();
                }
            });
        }

        public void Pause()
        {
            tweenTimer.Pause();
        }

        public void Stop()
        {
            if (tweenTimer != null)
            {
                tweenTimer.Kill();
                tweenTimer = null;
            }

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