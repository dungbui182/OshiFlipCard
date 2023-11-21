using DG.Tweening;
using GrandDreams.Core.Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GrandDreams.Game.FlipCard
{

    public enum ECardStatus
    {
        Hidden,
        PlayingAnimation,
        Shown
    }


    public class FlipCard : MonoBehaviour, IPointerDownHandler
    {

        #region Declare Variables

        private static readonly Vector3 HIDE_SCALE_VALUE = new Vector3(0, 0, 0);

        private ECardStatus cardStatus = ECardStatus.Hidden;
        public ECardStatus CardStatus
        {
            get => cardStatus;
            set => cardStatus = value;
        }

        public System.Action<FlipCard> OnPointerDownClick = delegate { };
        [SerializeField] private Sprite[] spriteItems;
        [SerializeField] private int itemType;
        [SerializeField] private Image imageRendererIcon;
        [SerializeField] private Image ImageCardContenOishi;
        private Sequence sequenceShow;
        public int ItemType => itemType;

        #endregion Declare Variables

        #region Private Function
        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            Reset();
        }

        #endregion Private Function

        #region Public Function
        public void Reset()
        {
            imageRendererIcon.rectTransform.localScale = HIDE_SCALE_VALUE;
            imageRendererIcon.SetAlpha(0);
        }
        // khi show trạng thái cardStatus được đặt là PlayingAnimation khi chạy hết Animation trạng thái được đặt là shown
        public void Show(float delayedTime = 0, System.Action onDone = null)
        {
            cardStatus = ECardStatus.PlayingAnimation;

            sequenceShow = DOTween.Sequence();

            sequenceShow.AppendCallback(() =>
            {
                imageRendererIcon.rectTransform.localScale = HIDE_SCALE_VALUE;
                imageRendererIcon.SetAlpha(0);
            });

            float timeDuration = 0.35f;

            sequenceShow.Append(ImageCardContenOishi.rectTransform.DOScale(2, timeDuration).SetEase(Ease.InSine));
            sequenceShow.Join(ImageCardContenOishi.DOFade(0, timeDuration).SetEase(Ease.InSine));
            sequenceShow.InsertCallback(timeDuration, () => { 
                 ImageCardContenOishi.rectTransform.localScale = Vector3.one;
            });
            sequenceShow.Insert(0.15f, imageRendererIcon.rectTransform.DOScale(1, timeDuration).SetEase(Ease.OutBack));
            sequenceShow.Join(imageRendererIcon.DOFade(1, timeDuration));

            sequenceShow.SetDelay(delayedTime).OnComplete(() =>
            {
                if (onDone != null)
                {
                    onDone();
                }

                cardStatus = ECardStatus.Shown;
            });
        }
        // khi Hide trạng thái cardStatus được đặt là PlayingAnimation khi chạy hết Animation trạng thái được đặt là Hidden

        public void Hide(float delayedTime = 0, System.Action onDone = null)
        {
            sequenceShow = DOTween.Sequence();

            float timeDuration = 0.35f;

            sequenceShow.AppendCallback(() =>
            {
                cardStatus = ECardStatus.PlayingAnimation;
            });
            ImageCardContenOishi.rectTransform.localScale = new Vector3(2f, 2f, 2f);

            sequenceShow.Append(imageRendererIcon.rectTransform.DOScale(0f, timeDuration).SetEase(Ease.InBack));
            sequenceShow.Join(imageRendererIcon.DOFade(0f, timeDuration));
            sequenceShow.Insert(0.05f, ImageCardContenOishi.rectTransform.DOScale(1, timeDuration).SetEase(Ease.InSine));
            sequenceShow.Join(ImageCardContenOishi.DOFade(1, timeDuration).SetEase(Ease.InQuint));

            sequenceShow.SetDelay(delayedTime).OnComplete(() =>
            {
                if (onDone != null)
                {
                    onDone();
                }

                cardStatus = ECardStatus.Hidden;
            });
        }

        // set giá trị hình cần lấy intsItemImage được truyền từ PanelPlaying xuống và giá trị của hình itemType
        public void SetItemType(int intsItemImage, int itemType)
        {
            this.itemType = itemType;

            imageRendererIcon.sprite = spriteItems[intsItemImage];
        }

        #endregion Public Function

        #region Event
        // xử lý sự kiện Click
        public void OnPointerDown(PointerEventData eventData)
        {

            if (cardStatus == ECardStatus.PlayingAnimation || cardStatus == ECardStatus.Shown)
            {
                return;
            }

            OnPointerDownClick(this);

        }
        #endregion Event
    }
}