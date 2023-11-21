using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class Status : MonoBehaviour
{
    #region Declare Variables

    public static Status SharedInstance { get; set; }

    [SerializeField] private Text textStatus;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private HorizontalLayoutGroup horizontal;

    private Tween tweenShow = null;

    #endregion Declare Variables

    private void Awake()
    {
        SharedInstance = this;
        canvasGroup = GetComponent<CanvasGroup>();
        horizontal = GetComponent<HorizontalLayoutGroup>();
        rectTransform = transform as RectTransform;
    }

    #region Public Function

    public void Show(string content, float delayedTime = 3, Action onDone = null)
    {
        if (tweenShow != null)
        {
            tweenShow.Kill();
            tweenShow = null;
        }

        textStatus.text = content;
        canvasGroup.alpha = 1;

        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

        tweenShow = canvasGroup.DOFade(0, 1).SetDelay(delayedTime).OnComplete(() =>
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
