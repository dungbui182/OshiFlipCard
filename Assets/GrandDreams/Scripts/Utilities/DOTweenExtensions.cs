using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDreams.Core.Utilities
{
    public static class DOTweenExtensions
    {
        public static void ShowCanvasGroup(this CanvasGroup canvasGroup, ref Sequence sequenceShow, float delayedTime = 0, float timeDuration = 0.5f, System.Action onStartup = null, System.Action onDone = null)
        {
            RectTransform rectTransform = canvasGroup.transform as RectTransform;

            rectTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            if (onStartup != null)
            {
                onStartup();
            }

            if (sequenceShow != null)
            {
                sequenceShow.Kill();
                sequenceShow = null;
            }

            sequenceShow = DOTween.Sequence();

            sequenceShow.Append(canvasGroup.DOFade(1, timeDuration));
            sequenceShow.Join(rectTransform.DOScale(1, timeDuration).SetEase(Ease.OutBack));

            sequenceShow.SetDelay(delayedTime).OnComplete(() =>
            {
                canvasGroup.blocksRaycasts = true;

                if (onDone != null)
                {
                    onDone();
                }
            });
        }

        public static void HideCanvasGroup(this CanvasGroup canvasGroup, ref Sequence sequenceShow, float delayedTime = 0, float timeDuration = 0.5f, System.Action onStartup = null, System.Action onDone = null)
        {
            RectTransform rectTransform = canvasGroup.transform as RectTransform;

            canvasGroup.blocksRaycasts = false;

            if (onStartup != null)
            {
                onStartup();
            }

            if (sequenceShow != null)
            {
                sequenceShow.Kill();
                sequenceShow = null;
            }

            sequenceShow = DOTween.Sequence();

            sequenceShow.Append(canvasGroup.DOFade(0, timeDuration));
            sequenceShow.Join(rectTransform.DOScale(0.5f, timeDuration).SetEase(Ease.InBack));

            sequenceShow.SetDelay(delayedTime).OnComplete(() =>
            {
                if (onDone != null)
                {
                    onDone();
                }
            });
        }

        public static Sequence Initialize(this Sequence sequence, bool createNewInstance = true, bool completeTween = false)
        {
            if (sequence != null)
            {
                sequence.Kill(completeTween);
                sequence = null;
            }

            if (createNewInstance)
            {
                sequence = DOTween.Sequence();
            }

            return sequence;
        }

        public static Sequence Final(this Sequence sequence, float delayedTime, System.Action OnDone)
        {
            sequence.SetDelay(delayedTime).OnComplete(() =>
            {
                if (OnDone != null)
                {
                    OnDone();
                }
            });

            return sequence;
        }

        public static Tween Initialize(this Tween tween, bool completeTween = false)
        {
            if (tween != null)
            {
                tween.Kill(completeTween);
                tween = null;
            }

            return tween;
        }

        public static Tween Final(this Tween tween, float delayedTime, System.Action OnDone)
        {
            tween.SetDelay(delayedTime).OnComplete(() =>
            {
                if (OnDone != null)
                {
                    OnDone();
                }
            });

            return tween;
        }

        public static void KillAll(this List<Tween> tweens, bool isComplete = false)
        {
            foreach (var tween in tweens)
            {
                tween.Kill(isComplete);
            }
        }

        public static void KillAll(this List<Sequence> sequences, bool isComplete = false)
        {
            foreach (var sequence in sequences)
            {
                sequence.Kill(isComplete);
            }
        }

#if NET_4_6
        public static Tween TypeText(this Text text, string content, dynamic tweenParameters = null)
        {
            if (string.IsNullOrEmpty(content))
            {
                text.text = "";
                return null;
            }

            char[] chars = content.ToCharArray();
            List<string> strings = new List<string>();

            for (int index = 0; index < chars.Length; index++)
            {
                if (chars[index] == '\\')
                {
                    if (index < chars.Length - 1 && chars[index] != ' ')
                    {
                        strings.Add("\\" + chars[index + 1]);
                        index++;
                    }
                }
                else
                {
                    strings.Add(chars[index].ToString());
                }
            }

            float timeDuration = 1;
            float delayedTime = 0;
            Ease ease = Ease.Linear;
            System.Action<string> onUpdate = null;
            System.Action onComplete = null;

            if (tweenParameters != null)
            {
                if (tweenParameters.ExistsProperty("timeDuration"))
                {
                    timeDuration = tweenParameters.timeDuration;
                }

                if (tweenParameters.ExistsProperty("delayedTime"))
                {
                    delayedTime = tweenParameters.delayedTime;
                }

                if (tweenParameters.ExistsProperty("ease"))
                {
                    ease = tweenParameters.ease;
                }

                if (tweenParameters.ExistsProperty("onUpdate"))
                {
                    onUpdate = tweenParameters.onUpdate;
                }

                if (tweenParameters.ExistsProperty("onComplete"))
                {
                    onComplete = tweenParameters.onComplete;
                }
            }

            int indexChar = 0;
            string appendingContent = "";

            Tween tween = DOTween.To(() => indexChar, i =>
            {
                if (indexChar < (int)i)
                {
                    do
                    {
                        indexChar++;
                        appendingContent += strings[indexChar];

                    } while (indexChar < (int)i);

                    text.text = appendingContent;

                    if (onUpdate != null)
                    {
                        onUpdate(appendingContent);
                    }
                }

            }, strings.Count, timeDuration).SetEase(ease)
            .OnStart(() =>
            {
                appendingContent = strings[0];
                text.text = appendingContent;
                if (onUpdate != null)
                {
                    onUpdate(appendingContent);
                }
            })
            .OnComplete(() =>
            {
                text.text = content;

                if (onComplete != null)
                {
                    onComplete();
                }
            });

            return tween;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="tweenParameters">
        /// object contains property: 
        /// - timeDuration
        /// - delayedTime
        /// - ease
        /// - onUpdate
        /// - onComplete
        /// </param>
        /// <returns></returns>
        public static Tween TypeText(this string content, dynamic tweenParameters = null)
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            char[] chars = content.ToCharArray();
            List<string> strings = new List<string>();

            for (int index = 0; index < chars.Length; index++)
            {
                if (chars[index] == '\\')
                {
                    if (index < chars.Length - 1 && chars[index] != ' ')
                    {
                        strings.Add("\\" + chars[index + 1]);
                        index++;
                    }
                }
                else
                {
                    strings.Add(chars[index].ToString());
                }
            }

            float timeDuration = 1;
            float delayedTime = 0;
            Ease ease = Ease.Linear;
            System.Action<string> onUpdate = null;
            System.Action onComplete = null;

            if (tweenParameters != null)
            {
             
            }

            int indexChar = 0;
            string appendingContent = "";

            Tween tween = DOTween.To(() => indexChar, i =>
            {
                if (indexChar < (int)i)
                {
                    do
                    {
                        indexChar++;
                        appendingContent += strings[indexChar];

                    } while (indexChar < (int)i);

                    if (onUpdate != null)
                    {
                        onUpdate(appendingContent);
                    }
                }

            }, strings.Count - 1, timeDuration).SetEase(ease)
            .OnStart(() =>
            {
                appendingContent = strings[0];
                if (onUpdate != null)
                {
                    onUpdate(appendingContent);
                }
            })
            .OnComplete(() =>
            {
                if (onComplete != null)
                {
                    onComplete();
                }
            });

            return tween;
        }
#endif

    }
}