using DG.Tweening;
using GrandDreams.Core;
using System;
using GrandDreams.Core.Utilities;
using GrandDreams.Game.FlipCard.Views;
using UnityEngine;
using UnityEngine.UI;
using GrandDreams.Common.Unity.Utilities;

namespace GrandDreams.Game.FlipCard
{
    public class PanelLogin : BasePanel
    {
        public System.Action OnPanelStart_RequestNextStep = delegate { };

        #region Declare Variables

        [SerializeField]
        private RectTransform rtButtonStart,
            rtLogoOishi,
            rTSloGan,
            rTTagLineBox,
            rTTagLineInBox,
            rTTagLineDownBox,
            rTManagerUmbrella,
            rTTextPairingImage;

        [SerializeField]
        private Vector2 originPosLogoOishi,
            originPosSloGan,
            originPosTagLineBox,
            originPosTagLineInBox,
            originPosTagLineDownBox,
            originPosManagerUmbrella,
            originPosTextPairingImage;

        [SerializeField] private Button btnStart;
        [SerializeField] private UmbrellaRotateScript[] umbrellaRotateScripts;

        private Vector2 originPosButtonStart;

        private float timeShow = 0.5f;
        private float durationOfTween = 0.4f;

        private Sequence sequenceStart;
        #endregion Declare Variables

        #region Protected Function
        protected override void Awake()
        {

            originPosLogoOishi = rtLogoOishi.anchoredPosition;
            originPosSloGan = rTSloGan.anchoredPosition;
            originPosTagLineBox = rTTagLineBox.anchoredPosition;
            originPosTagLineInBox = rTTagLineInBox.anchoredPosition;
            originPosTagLineDownBox = rTTagLineDownBox.anchoredPosition;
            originPosManagerUmbrella = rTManagerUmbrella.anchoredPosition;
            originPosTextPairingImage = rTTextPairingImage.anchoredPosition;

            rtButtonStart = btnStart.GetComponent<RectTransform>();
            originPosButtonStart = rtButtonStart.anchoredPosition;

            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            btnStart.onClick.AddListener(OnButtonPanelLoginClick);
            sequenceStart = DOTween.Sequence();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();

            btnStart.onClick.RemoveListener(OnButtonPanelLoginClick);
            OnPanelStart_RequestNextStep = delegate { };
            sequenceStart = sequenceStart.Initialize(false);
        }

        #endregion Protected Function

        #region Public Function

        public override void Reset()
        {
            base.Reset();

            rtButtonStart.localScale = Vector3.zero;
            rtLogoOishi.localScale = Vector3.zero;
            rTSloGan.localScale = Vector3.zero;
            rTTagLineBox.localScale = Vector3.zero;
            rTTagLineInBox.localScale = Vector3.zero;
            rTTagLineDownBox.localScale = Vector3.zero;
            rTManagerUmbrella.localScale = Vector3.zero;
            rTTextPairingImage.localScale = Vector3.zero;

            rtButtonStart.anchoredPosition = originPosButtonStart.AddY(-40);
            rtLogoOishi.anchoredPosition = originPosLogoOishi.AddY(40);
            rTSloGan.anchoredPosition = originPosSloGan.AddY(40);
            rTTagLineBox.anchoredPosition = originPosTagLineBox.AddY(40);
            rTTagLineInBox.anchoredPosition = originPosTagLineInBox.AddY(40);
            rTTagLineDownBox.anchoredPosition = originPosTagLineDownBox.AddY(40);
            rTManagerUmbrella.anchoredPosition = originPosManagerUmbrella.AddY(40);
            rTTextPairingImage.anchoredPosition = originPosTextPairingImage.AddY(40);

        }

        public override void Show(object[] parameters = null, float delayedTime = 0, Action onAwake = null,
            Action onStart = null, Action onDone = null)
        {
            System.Action overrideDone = () =>
            {
                sequenceStart = sequenceStart.Initialize();
                rTTagLineInBox.localScale = Vector3.zero;

                foreach (var script in umbrellaRotateScripts)
                {
                    script.StartRotate();
                }

                durationOfTween = 0.4f;

                sequenceStart.Append(rtLogoOishi.DOScale(Vector2.one, durationOfTween).SetEase(Ease.OutBack));
                sequenceStart.Join(rtLogoOishi.DOAnchorPos(originPosLogoOishi, durationOfTween).SetEase(Ease.OutBack));

                sequenceStart.Insert(timeShow + 0.25f, rTSloGan.DOScale(Vector2.one, durationOfTween).SetEase(Ease.OutBack));
                sequenceStart.Join(rTSloGan.DOAnchorPos(originPosSloGan, durationOfTween).SetEase(Ease.OutBack));

                sequenceStart.Insert(timeShow + 0.5f,
                    rTTagLineBox.DOScale(Vector2.one, durationOfTween).SetEase(Ease.OutBack));
                sequenceStart.Join(rTTagLineBox.DOAnchorPos(originPosTagLineBox, durationOfTween).SetEase(Ease.OutBack));
                sequenceStart.Insert(timeShow + 0.75f,
                    rTTagLineInBox.DOScale(Vector2.one, durationOfTween).SetEase(Ease.OutBack));

                sequenceStart.Insert(timeShow + 1f,
                    rTTagLineDownBox.DOScale(Vector2.one, durationOfTween).SetEase(Ease.OutBack));
                sequenceStart.Join(rTTagLineDownBox.DOAnchorPos(originPosTagLineDownBox, durationOfTween).SetEase(Ease.OutBack));

                sequenceStart.Insert(timeShow + 1.25f,
                    rTManagerUmbrella.DOScale(0.8f, durationOfTween * 2f).SetEase(Ease.OutBack));
                sequenceStart.Join(rTManagerUmbrella.DOAnchorPos(originPosManagerUmbrella, durationOfTween * 2f)
                    .SetEase(Ease.OutBack));

                sequenceStart.Insert(timeShow + 1.5f,
                    rTTextPairingImage.DOScale(Vector2.one, durationOfTween).SetEase(Ease.OutBack));
                sequenceStart.Join(rTTextPairingImage.DOAnchorPos(originPosTextPairingImage, durationOfTween)
                    .SetEase(Ease.OutBack));

                sequenceStart.Insert(timeShow + 1.75f, rtButtonStart.DOScale(Vector2.one, durationOfTween).SetEase(Ease.OutBack));
                sequenceStart.Join(rtButtonStart.DOAnchorPos(originPosButtonStart, durationOfTween).SetEase(Ease.OutBack));

            };
            base.Show(parameters, delayedTime, onAwake, onStart, overrideDone);


        }

        public override void Hide(object[] parameters = null, float delayedTime = 0, Action onAwake = null,
            Action onStart = null, Action onDone = null)
        {
            System.Action overrideAwake = () =>
            {
                foreach (var script in umbrellaRotateScripts)
                {
                    script.StopRotate();
                }

                durationOfTween = 0.3f;
                timeShow = 0f;

                sequenceShow.Append(rtButtonStart.GetComponent<CanvasGroup>().DOFade(0, durationOfTween).SetEase(Ease.InBack));
                sequenceShow.Join(rtButtonStart.DOAnchorPos(originPosButtonStart.AddY(40), durationOfTween)
                    .SetEase(Ease.InBack));

                sequenceShow.Insert(timeShow + 0.15f,
                    rTTextPairingImage.DOScale(Vector2.zero, durationOfTween).SetEase(Ease.InBack));
                sequenceShow.Join(rTTextPairingImage.DOAnchorPos(originPosTextPairingImage.AddY(-40), durationOfTween)
                    .SetEase(Ease.InBack));

                sequenceShow.Insert(timeShow + 0.3f,
                    rTManagerUmbrella.DOScale(Vector2.zero, durationOfTween * 1.5f).SetEase(Ease.InBack));
                sequenceShow.Join(rTManagerUmbrella.DOAnchorPos(originPosManagerUmbrella.AddY(-40), durationOfTween * 1.5f)
                    .SetEase(Ease.InBack));

                sequenceShow.Insert(timeShow + 0.45f,
                    rTTagLineDownBox.DOScale(Vector2.zero, durationOfTween).SetEase(Ease.InBack));
                sequenceShow.Join(rTTagLineDownBox.DOAnchorPos(originPosTagLineDownBox.AddY(-40), durationOfTween)
                    .SetEase(Ease.InBack));

                sequenceShow.Insert(timeShow + 0.6f,
                    rTTagLineInBox.DOScale(Vector2.zero, durationOfTween).SetEase(Ease.InBack));

                sequenceShow.Insert(timeShow + 0.75f,
                    rTTagLineBox.DOScale(Vector2.zero, durationOfTween).SetEase(Ease.InBack));
                sequenceShow.Join(rTTagLineInBox.DOAnchorPos(originPosTagLineInBox.AddY(-40), durationOfTween)
                    .SetEase(Ease.InBack));

                sequenceShow.Insert(timeShow + 0.9f,
                    rTSloGan.DOScale(Vector2.zero, durationOfTween).SetEase(Ease.InBack));
                sequenceShow.Join(
                    rTSloGan.DOAnchorPos(originPosSloGan.AddY(-40), durationOfTween).SetEase(Ease.InBack));

                sequenceShow.Insert(timeShow + 1.05f,
                    rtLogoOishi.DOScale(Vector2.zero, durationOfTween).SetEase(Ease.InBack));
                sequenceShow.Join(rtLogoOishi.DOAnchorPos(originPosLogoOishi.AddY(-40), durationOfTween)
                    .SetEase(Ease.InBack));

                if (onAwake != null)
                {
                    onAwake();
                }
            };

            base.Hide(parameters, delayedTime, overrideAwake, onStart, onDone);
        }
        #endregion Public Function

        #region Event
        private void OnButtonPanelLoginClick()
        {
            if (SoundManager.SharedInstance != null)
            {
                SoundManager.SharedInstance.PlaySfx("Click");
            }

            if (OnPanelStart_RequestNextStep != null)
            {

                OnPanelStart_RequestNextStep();
            }
        }
        #endregion Event
    }
}