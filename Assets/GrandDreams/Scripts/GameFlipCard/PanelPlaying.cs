using DG.Tweening;
using GrandDreams.Common.Unity;
using GrandDreams.Common.Unity.Utilities;
using GrandDreams.Core;
using GrandDreams.Core.Utilities;
using GrandDreams.Game.FlipCard.Views;
using System;
using UnityEngine;

namespace GrandDreams.Game.FlipCard
{
    public class PanelPlaying : BasePanel
    {
        #region Declare Variables

        public System.Action<bool> ResultPanelPlaying = delegate { };

        [SerializeField]
        private RectTransform
            rTTagLineBox,
            rTTagLineInBox,
            rTTagLineDownBox,
            rTManagerBar,
            rTManagerUmbrella,
            rTManagerFlipCard,
            rTTimeLine;

        [SerializeField]
        private Vector2
            originPosTagLineBox,
            originPosTagLineInBox,
            originPosTagLineDownBox,
            originPosManagerBar,
            originPosManagerUmbrella,
            originPosManagerFlipCard,
            originPosTimeLine;

        [SerializeField] private UmbrellaRotateScript[] umbrellaRotateScripts;
        [SerializeField] private FlipCard[] flipCards;
        [SerializeField] private TimerController timerController;
        private FlipCard flipFirst;
        private FlipCard flipSecond;
        private int[] intsItemImage = { 0, 1, 2, 3, 4, 5 };

        private int point = 0, timeDuration = 30;
        private int tempIndexLoop;
        private bool isWin = true;
        private float timeShow = 0.05f;
        private float durationOfTween = 0.4f;

        private bool isLockPanelCard;
        private Sequence sequencePlaying;


        #endregion Declare Variables

        #region Protected Function
        protected override void Awake()
        {
            base.Awake();

            originPosTagLineBox = rTTagLineBox.anchoredPosition;
            originPosTagLineInBox = rTTagLineInBox.anchoredPosition;
            originPosTagLineDownBox = rTTagLineDownBox.anchoredPosition;
            originPosManagerUmbrella = rTManagerUmbrella.anchoredPosition;
            originPosManagerBar = rTManagerBar.anchoredPosition;
            originPosManagerFlipCard = rTManagerFlipCard.anchoredPosition;
            originPosTimeLine = rTTimeLine.anchoredPosition;

        }

        protected override void Start()
        {
            base.Start();

            timerController.OnTimerController_TimeUp += HandlenOnTimerController_TimeUp;
            timeDuration = ConfigData.Data.TimeDuration;
            sequencePlaying = DOTween.Sequence();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            timerController.OnTimerController_TimeUp -= HandlenOnTimerController_TimeUp;
            flipCards[tempIndexLoop].OnPointerDownClick -= HandleFlipCardOnPointerDownClick;
            sequencePlaying = sequencePlaying.Initialize(false);
        }
        #endregion Protected Function

        #region Public Function

        public override void Reset()
        {
            base.Reset();

            rTTagLineBox.localScale = Vector3.zero;
            rTTagLineInBox.localScale = Vector3.zero;
            rTTagLineDownBox.localScale = Vector3.zero;
            rTManagerUmbrella.localScale = Vector3.zero;
            rTManagerBar.localScale = Vector3.zero;
            rTManagerFlipCard.localScale = Vector3.zero;
            rTTimeLine.localScale = Vector3.zero;


            rTTagLineBox.anchoredPosition = originPosTagLineBox.AddY(40);
            rTTagLineInBox.anchoredPosition = originPosTagLineInBox.AddY(40);
            rTTagLineDownBox.anchoredPosition = originPosTagLineDownBox.AddY(40);
            rTManagerUmbrella.anchoredPosition = originPosManagerUmbrella.AddY(40);
            rTManagerBar.anchoredPosition = originPosManagerBar.AddY(40);
            rTManagerFlipCard.anchoredPosition = originPosManagerFlipCard.AddY(40);
            rTTimeLine.anchoredPosition = originPosTimeLine.AddY(40);
        }

        public override void Show(object[] parameters = null, float delayedTime = 0, Action onAwake = null,
            Action onStart = null, Action onDone = null)
        {
            System.Action overrideOnDone = () =>
                {
                    sequencePlaying = sequencePlaying.Initialize();
                    timerController.Initialize(timeDuration);


                    // trộn số và truyền vị trí và giá trị số xuống FlipCard (gán giá trị 0,1... xuống tương ứng với từng lá bài) thông qua SetItemType
                    intsItemImage.Shuffle();

                    for (int i = 0; i < flipCards.Length; i++)
                    {
                        {
                            tempIndexLoop = i;
                            int itemType = 0;
                            if (intsItemImage[tempIndexLoop] == 0 || intsItemImage[tempIndexLoop] == 1)
                            {
                                itemType = 0;
                            }
                            else if (intsItemImage[tempIndexLoop] == 2 || intsItemImage[tempIndexLoop] == 3)
                            {
                                itemType = 1;
                            }
                            else if (intsItemImage[tempIndexLoop] == 4 || intsItemImage[tempIndexLoop] == 5)
                            {
                                itemType = 2;
                            }

                            flipCards[tempIndexLoop].SetItemType(intsItemImage[tempIndexLoop], itemType);
                            flipCards[tempIndexLoop].OnPointerDownClick += HandleFlipCardOnPointerDownClick;
                        }
                    }
                    // tạo vòng foreach để RotateUmbrella
                    foreach (var script in umbrellaRotateScripts)
                    {
                        script.StartRotate();
                    }
                    durationOfTween = 0.4f;

                    sequencePlaying.Append(rTTagLineBox.DOScale(Vector2.one, durationOfTween).SetEase(Ease.OutBack));
                    sequencePlaying.Join(rTTagLineBox.DOAnchorPos(originPosTagLineBox, durationOfTween).SetEase(Ease.OutBack));

                    sequencePlaying.Insert(timeShow + 0.25f, rTTagLineInBox.DOScale(Vector2.one, durationOfTween).SetEase(Ease.OutBack));

                    sequencePlaying.Insert(timeShow + 0.5f,
                        rTTagLineDownBox.DOScale(Vector2.one, durationOfTween).SetEase(Ease.OutBack));
                    sequencePlaying.Join(rTTagLineDownBox.DOAnchorPos(originPosTagLineDownBox, durationOfTween).SetEase(Ease.OutBack));

                    sequencePlaying.Insert(timeShow + 0.75f,
                      rTManagerFlipCard.DOScale(Vector2.one, durationOfTween).SetEase(Ease.OutBack));
                    sequencePlaying.Join(rTManagerFlipCard.DOAnchorPos(originPosManagerFlipCard, durationOfTween).SetEase(Ease.OutBack));

                    sequencePlaying.Insert(timeShow + 1f,
                        rTManagerUmbrella.DOScale(0.8f, durationOfTween).SetEase(Ease.OutBack));
                    sequencePlaying.Join(rTManagerUmbrella.DOAnchorPos(originPosManagerUmbrella, durationOfTween).SetEase(Ease.OutBack));

                    sequencePlaying.Insert(timeShow + 1.25f,
                        rTManagerBar.DOScale(Vector2.one, durationOfTween).SetEase(Ease.OutBack));
                    sequencePlaying.Join(rTManagerBar.DOAnchorPos(originPosManagerBar, durationOfTween).SetEase(Ease.OutBack));

                    sequencePlaying.Insert(timeShow + 1.5f,
                        rTTimeLine.DOScale(Vector2.one, durationOfTween).SetEase(Ease.OutBack));
                    sequencePlaying.Join(rTTimeLine.DOAnchorPos(originPosTimeLine, durationOfTween)
                        .SetEase(Ease.OutBack)).OnComplete(() =>
                        {
                            timerController.Run();

                        });

                };
            base.Show(parameters, delayedTime, onAwake, onStart, overrideOnDone);

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

                sequenceShow.Append(rTTimeLine.DOScale(Vector2.zero, durationOfTween).SetEase(Ease.InBack));
                sequenceShow.Join(rTTimeLine.DOAnchorPos(originPosTimeLine.AddY(40), durationOfTween)
                    .SetEase(Ease.InBack));

                sequenceShow.Insert(timeShow + 0.15f,
                    rTManagerFlipCard.DOScale(Vector2.zero, durationOfTween).SetEase(Ease.InBack));
                sequenceShow.Join(rTManagerFlipCard.DOAnchorPos(originPosManagerFlipCard.AddY(-40), durationOfTween)
                    .SetEase(Ease.InBack));

                sequenceShow.Insert(timeShow + 0.3f,
                    rTManagerUmbrella.DOScale(Vector2.zero, durationOfTween).SetEase(Ease.InBack));
                sequenceShow.Join(rTManagerUmbrella.DOAnchorPos(originPosManagerUmbrella.AddY(-40), durationOfTween)
                    .SetEase(Ease.InBack));

                sequenceShow.Insert(timeShow + 0.45f,
                    rTManagerBar.DOScale(Vector2.zero, durationOfTween).SetEase(Ease.InBack));
                sequenceShow.Join(rTManagerBar.DOAnchorPos(originPosManagerBar.AddY(-40), durationOfTween)
                    .SetEase(Ease.InBack));

                sequenceShow.Insert(timeShow + 0.6f,
                    rTTagLineDownBox.DOScale(Vector2.zero, durationOfTween).SetEase(Ease.InBack));
                sequenceShow.Join(rTTagLineDownBox.DOAnchorPos(originPosTagLineDownBox.AddY(-40), durationOfTween)
                    .SetEase(Ease.InBack));

                sequenceShow.Insert(timeShow + 0.75f,
                    rTTagLineInBox.DOScale(Vector2.zero, durationOfTween).SetEase(Ease.InBack));

                sequenceShow.Insert(timeShow + 0.9f,
                    rTTagLineBox.DOScale(Vector2.zero, durationOfTween).SetEase(Ease.InBack));
                sequenceShow.Join(
                    rTTagLineBox.DOAnchorPos(originPosTagLineBox.AddY(-40), durationOfTween).SetEase(Ease.InBack));


                if (onAwake != null)
                {
                    onAwake();
                }
            };
            base.Hide(parameters, delayedTime, overrideAwake, onStart, onDone);
        }

        #endregion Public Function

        #region Private Function

        // xử lý các lá bài khi được chạm
        private void HandleFlipCardOnPointerDownClick(FlipCard card)
        {
            // Điều kiện lá bài được Click chưa nếu rồi thì sang bước tiếp theo

            if (isLockPanelCard)
            {
                return;
            }

            if (SoundManager.SharedInstance != null)
            {
                SoundManager.SharedInstance.PlaySfx("Click");
            }

            System.Action triggerOpen2Cards = () =>
            {
                // xét điều kiện thắng game là cả 2 lần nhấn điều phải khác rỗng và ItemType giữa 2 lần nhấn giống nhau nếu giống nhau thì iswin = true gửi qua bước tiếp theo
                if (flipFirst != null && flipSecond != null)
                {
                 
                    if (flipFirst.ItemType != flipSecond.ItemType)
                    {
                        flipFirst.Hide(0.5f);
                        flipSecond.Hide(0.5f, () =>
                        {
                            flipSecond = null;
                            flipFirst = null;
                            isLockPanelCard = false;
                        });
                    }
                    else
                    {
                        point++;
                        if (point == flipCards.Length / 2)
                        {
                            timerController.Stop();
                            ResultPanelPlaying(isWin);
                        }

                        flipSecond = null;
                        flipFirst = null;
                        isLockPanelCard = false;
                    }
                }
            };

            // xét điều kiện để so sánh lần nhấn 1 và 2 khi lần nhấn 1 bằng rỗng thì sét flipFist = card đó còn nếu lần nhấn thứ nhất khác rỗng thì xét lá bài đó bằng flipSecond
            if (flipFirst == null)
            {
                flipFirst = card;
                flipFirst.Show();
            }
            else
            {
                flipSecond = card;
                flipSecond.Show(0, triggerOpen2Cards);

                isLockPanelCard = true;
            }
        }

        // xử ly khi hết thời gian => xét trạng thái bằng EndGame và bắn trạng thái iswin = false sang bước tiếp theo
        private void HandlenOnTimerController_TimeUp()
        {
            ResultPanelPlaying(!isWin);
        }

        #endregion Private Function
    }
}