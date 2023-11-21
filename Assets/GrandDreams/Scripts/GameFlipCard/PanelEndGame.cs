using DG.Tweening;
using GrandDreams.Common.Unity.Utilities;
using GrandDreams.Core;
using GrandDreams.Core.Utilities;
using GrandDreams.Game.FlipCard.Views;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDreams.Game.FlipCard
{
    public class PanelEndGame : BasePanel
    {
        #region Declare Variables

        public System.Action OnPanelEndGame_RequestRestart = delegate { };

        [SerializeField] private UmbrellaRotateScript[] umbrellaRotateScripts;
        [SerializeField] private ParticleSystem psFlares;
        [SerializeField]
        private RectTransform
           rtLogoOishi,
           rTSloGan,
           rTTagLineInBoxWin,
           rTTagLineBoxWin,
           rTTagLineInBoxLose,
           rTTagLineBoxLose,
           rTManagerUmbrella;

        [SerializeField]
        private Vector2 originPosLogoOishi,
           originPosSloGan,
           originPosTagLineBoxWin,
           originPosTagLineInBoxWin,
            originPosTagLineBoxLose,
           originPosTagLineInBoxLose,
           originPosManagerUmbrella;

        [SerializeField] private TextMeshProUGUI textEndGame;
        [SerializeField] private Button btnRestart;

        private Sequence sequencePlay;
        private float timeShow = 0f;
        private float durationOfTween = 0.4f;
        private bool isWin = false;

        private bool allowToRestart = false;

        #endregion Declare Variables

        #region Protected Function
        // giá trị của OriginPos được đặt = giá trị trên MainScen 
        protected override void Awake()
        {
            originPosLogoOishi = rtLogoOishi.anchoredPosition;
            originPosSloGan = rTSloGan.anchoredPosition;
            originPosTagLineBoxWin = rTTagLineBoxWin.anchoredPosition;
            originPosTagLineInBoxWin = rTTagLineInBoxWin.anchoredPosition;
            originPosTagLineBoxLose = rTTagLineBoxLose.anchoredPosition;
            originPosTagLineInBoxLose = rTTagLineInBoxLose.anchoredPosition;
            originPosManagerUmbrella = rTManagerUmbrella.anchoredPosition;

            btnRestart.onClick.AddListener(HandleButtonRestart_Clicked);

            base.Awake();
        }
        // khi Start bắt đầu chạy hàm Reset trong basePanel cũng được gọi 
        protected override void Start()
        {
            base.Start();
            sequencePlay = DOTween.Sequence();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();

            btnRestart.onClick.RemoveListener(HandleButtonRestart_Clicked);

            OnPanelEndGame_RequestRestart = delegate { };
        }

        #endregion Protected Function

        #region Public Function

        public override void Reset()
        {
            base.Reset();

            allowToRestart = false;

            rtLogoOishi.localScale = Vector3.zero;
            rTSloGan.localScale = Vector3.zero;
            rTTagLineBoxWin.localScale = Vector3.zero;
            rTTagLineInBoxWin.localScale = Vector3.zero;
            rTTagLineBoxLose.localScale = Vector3.zero;
            rTTagLineInBoxLose.localScale = Vector3.zero;
            rTManagerUmbrella.localScale = Vector3.zero;



            rtLogoOishi.anchoredPosition = originPosLogoOishi.AddY(40);
            rTSloGan.anchoredPosition = originPosSloGan.AddY(40);
            rTTagLineBoxWin.anchoredPosition = originPosTagLineBoxWin.AddY(40);
            rTTagLineInBoxWin.anchoredPosition = originPosTagLineInBoxWin.AddY(20);
            rTTagLineBoxLose.anchoredPosition = originPosTagLineBoxLose.AddY(40);
            rTTagLineInBoxLose.anchoredPosition = originPosTagLineInBoxLose.AddY(40);
            rTManagerUmbrella.anchoredPosition = originPosManagerUmbrella.AddY(40);

            textEndGame.SetAlpha(0);
        }
        // trạng thái win được truyền từ GameControler xuống và hàm SequencePlay được gọi để thay thế cho SequenceShow tránh bị chồng đè 
        public override void Show(object[] parameters = null, float delayedTime = 0, Action onAwake = null, Action onStart = null, Action onDone = null)
        {
            isWin = (bool)parameters[0];

            foreach (var script in umbrellaRotateScripts)
            {
                script.StartRotate();
            }

            durationOfTween = 0.4f;

            System.Action overrideDone = () =>
            {
                sequencePlay = sequencePlay.Initialize();


                sequencePlay.Append(rtLogoOishi.DOScale(Vector2.one, durationOfTween).SetEase(Ease.OutBack));
                sequencePlay.Join(rtLogoOishi.DOAnchorPos(originPosLogoOishi, durationOfTween).SetEase(Ease.OutBack));

                sequencePlay.Insert(timeShow + 0.25f, rTSloGan.DOScale(Vector2.one, durationOfTween).SetEase(Ease.OutBack));
                sequencePlay.Join(rTSloGan.DOAnchorPos(originPosSloGan, durationOfTween).SetEase(Ease.OutBack));

                if (isWin)
                {

                    psFlares.Play();
                    sequencePlay.Insert(timeShow + 0.5f,
                        rTTagLineBoxWin.DOScale(0.8f, durationOfTween).SetEase(Ease.OutBack));
                    sequencePlay.Join(rTTagLineBoxWin.DOAnchorPos(originPosTagLineBoxWin, durationOfTween).SetEase(Ease.OutBack));

                    sequencePlay.Insert(timeShow + 0.75f,
                        rTTagLineInBoxWin.DOScale(0.8f, durationOfTween).SetEase(Ease.OutBack));

                    if (SoundManager.SharedInstance != null)
                    {
                        SoundManager.SharedInstance.PlaySfx("EndGameWin");
                    }
                }
                else
                {

                    sequencePlay.Insert(timeShow + 0.5f,
                        rTTagLineBoxLose.DOScale(0.8f, durationOfTween).SetEase(Ease.OutBack));
                    sequencePlay.Join(rTTagLineBoxLose.DOAnchorPos(originPosTagLineBoxLose, durationOfTween).SetEase(Ease.OutBack));

                    sequencePlay.Insert(timeShow + 0.75f,
                        rTTagLineInBoxLose.DOScale(0.8f, durationOfTween).SetEase(Ease.OutBack));
                }

                sequencePlay.Insert(timeShow + 1.25f,
                    rTManagerUmbrella.DOScale(0.75f, durationOfTween).SetEase(Ease.OutBack));
                sequencePlay.Join(rTManagerUmbrella.DOAnchorPos(originPosManagerUmbrella, durationOfTween)
                    .SetEase(Ease.OutBack));

                sequencePlay.Insert(timeShow + 5,
                   textEndGame.DOFade(1, durationOfTween).SetEase(Ease.OutBack));
                sequencePlay.AppendCallback(() =>
                {

                    allowToRestart = true;

                    if (onDone != null)
                    {
                        onDone();

                    }
                });
                if (SoundManager.SharedInstance != null)
                {
                    SoundManager.SharedInstance.PlaySfx("EndGameFail");
                }
            };

            base.Show(parameters, delayedTime, onAwake, onStart, overrideDone);
        }

        public override void Hide(object[] parameters = null, float delayedTime = 0, Action onAwake = null, Action onStart = null, Action onDone = null)
        {
            System.Action overrideAwake = () =>
            {
                sequenceShow = sequenceShow.Initialize();

                foreach (var script in umbrellaRotateScripts)
                {
                    script.StopRotate();
                }
                psFlares.Stop();
                psFlares.gameObject.SetActive(false);
                durationOfTween = 0.3f;
                timeShow = 0f;

                sequenceShow.Append(rTManagerUmbrella.DOScale(Vector2.zero, durationOfTween).SetEase(Ease.InBack));
                sequenceShow.Join(rTManagerUmbrella.DOAnchorPos(originPosManagerUmbrella.AddY(40), durationOfTween)
                    .SetEase(Ease.InBack));

                if (isWin)
                {
                    sequenceShow.Insert(timeShow + 0.15f,
                        rTTagLineInBoxWin.DOScale(Vector2.zero, durationOfTween).SetEase(Ease.InBack));
                    sequenceShow.Join(rTTagLineInBoxWin.DOAnchorPos(originPosTagLineInBoxWin.AddY(-40), durationOfTween)
                        .SetEase(Ease.InBack));

                    sequenceShow.Insert(timeShow + 0.3f,
                        rTTagLineBoxWin.DOScale(Vector2.zero, durationOfTween).SetEase(Ease.InBack));

                }
                else
                {
                    sequenceShow.Insert(timeShow + 0.15f,
                        rTTagLineInBoxLose.DOScale(Vector2.zero, durationOfTween).SetEase(Ease.InBack));
                    sequenceShow.Join(rTTagLineInBoxLose.DOAnchorPos(originPosTagLineInBoxLose.AddY(-40), durationOfTween)
                        .SetEase(Ease.InBack));

                    sequenceShow.Insert(timeShow + 0.3f,
                        rTTagLineBoxLose.DOScale(Vector2.zero, durationOfTween).SetEase(Ease.InBack));

                }

                sequenceShow.Insert(timeShow + 0.45f,
                    rTSloGan.DOScale(Vector2.zero, durationOfTween).SetEase(Ease.InBack));
                sequenceShow.Join(
                    rTSloGan.DOAnchorPos(originPosSloGan.AddY(-40), durationOfTween).SetEase(Ease.InBack));

                sequenceShow.Insert(timeShow + 0.6f,
                    rtLogoOishi.DOScale(Vector2.zero, durationOfTween).SetEase(Ease.InBack));
                sequenceShow.Join(rtLogoOishi.DOAnchorPos(originPosLogoOishi.AddY(-40), durationOfTween)
                    .SetEase(Ease.InBack)).OnComplete(() =>
                    {
                        if (onAwake != null)
                        {
                            onAwake();
                        }
                    });
            };
            base.Hide(parameters, delayedTime, overrideAwake, onStart, onDone);
        }

        #endregion Public Function

        #region Event

        private void HandleButtonRestart_Clicked()
        {

            if (!allowToRestart)
            {
                return;
            }
            if (SoundManager.SharedInstance != null)
            {
                SoundManager.SharedInstance.PlaySfx("Click");
            }

            if (OnPanelEndGame_RequestRestart != null)
            {
                OnPanelEndGame_RequestRestart();
                allowToRestart = false;
            }
        }

        #endregion Event
    }
}