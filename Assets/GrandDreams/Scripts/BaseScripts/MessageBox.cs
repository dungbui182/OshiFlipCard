using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDreams.Core
{
    public class MessageBox : MonoBehaviour
    {

        #region Declare Variables

        public static MessageBox Instance { get; private set; }

        public bool IsShowing { get; set; }

        [SerializeField] private GameObject gameObjectButtonLeft, gameObjectButtonRight;
        [SerializeField] private RectTransform rectTransformMessageBox;
        [SerializeField] private Text textTitle, textMessage, textButtonLeft, textButtonRight;
        [SerializeField] private Button buttonLeft, buttonRight;

        private CanvasGroup canvasGroup;

        private System.Action onButtonLeft_Clicked, onButtonRight_Clicked;
        private System.Action<string> onButtonInputData_Clicked;
        private System.Action<string, string> onButtonInputData2_Clicked;

        private int currentPriority = 0;

        private Sequence sequenceShow = null;

        #endregion Declare Variables

        private void Awake()
        {
            Instance = this;

            canvasGroup = GetComponent<CanvasGroup>();

            buttonLeft.onClick.AddListener(HandleButtonLeft_Clicked);
            buttonRight.onClick.AddListener(HandleButtonRight_Clicked);
        }

        private void OnDestroy()
        {
            if (sequenceShow != null)
            {
                sequenceShow.Kill();
                sequenceShow = null;
            }
        }

        #region Public Function

        public void ForceClose()
        {
            if (currentPriority == 1)
            {
                HandleButtonLeft_Clicked();
            }
            else if (currentPriority > 1)
            {
                HandleButtonRight_Clicked();
            }
        }

        public void ShowMessage(string message, System.Action onButtonOK_Clicked = null)
        {
            textTitle.gameObject.SetActive(true);

            gameObjectButtonLeft.SetActive(false);
            gameObjectButtonRight.SetActive(true);

            textTitle.text = "THÔNG BÁO";
            textMessage.text = message;

            textButtonRight.text = "ĐỒNG Ý";

            onButtonRight_Clicked = onButtonOK_Clicked;

            Show(1);
        }

        public void ShowMessage(string title, string message, System.Action onButtonOK_Clicked = null)
        {
            textTitle.gameObject.SetActive(true);

            gameObjectButtonLeft.SetActive(false);
            gameObjectButtonRight.SetActive(true);

            textTitle.text = title;
            textMessage.text = message;

            textButtonRight.text = "ĐỒNG Ý";

            onButtonRight_Clicked = onButtonOK_Clicked;

            Show(1);
        }

        public void ShowYesNoBox(string title, string message, System.Action onButtonYes_Clicked = null, System.Action onButtonNo_Clicked = null)
        {
            textTitle.gameObject.SetActive(true);

            gameObjectButtonLeft.SetActive(true);
            gameObjectButtonRight.SetActive(true);

            textTitle.text = title;
            textMessage.text = message;

            onButtonLeft_Clicked = onButtonNo_Clicked;
            onButtonRight_Clicked = onButtonYes_Clicked;
            textButtonLeft.text = "KHÔNG";
            textButtonRight.text = "CÓ";

            Show(2);
        }

        public void ShowCustom2ButtonsBox(string title, string message, string textButtonLeft, string textButtonRight, System.Action onButtonLeft_Clicked = null, System.Action onButtonRight_Clicked = null)
        {
            textTitle.gameObject.SetActive(true);

            gameObjectButtonLeft.SetActive(true);
            gameObjectButtonRight.SetActive(true);

            textTitle.text = title;
            textMessage.text = message;

            this.onButtonLeft_Clicked = onButtonLeft_Clicked;
            this.onButtonRight_Clicked = onButtonRight_Clicked;
            this.textButtonLeft.text = textButtonLeft;
            this.textButtonRight.text = textButtonRight;

            Show(2);
        }

        public void ShowOKCancelBox(string title, string message, System.Action onButtonOK_Clicked = null, System.Action onButtonCancel_Clicked = null)
        {
            textTitle.gameObject.SetActive(true);

            gameObjectButtonLeft.SetActive(true);
            gameObjectButtonRight.SetActive(true);

            textTitle.text = title;
            textMessage.text = message;

            onButtonLeft_Clicked = onButtonCancel_Clicked;
            onButtonRight_Clicked = onButtonOK_Clicked;

            textButtonLeft.text = "ĐÓNG";
            textButtonRight.text = "ĐỒNG Ý";

            Show(2);
        }

        public void ShowInputDataBox(string title, string message, System.Action<string> onButtonOK_Clicked = null, System.Action onButtonCancel_Clicked = null)
        {
            textTitle.gameObject.SetActive(true);

            gameObjectButtonLeft.SetActive(true);
            gameObjectButtonRight.SetActive(true);

            textTitle.text = title;
            textMessage.text = message;

            onButtonLeft_Clicked = onButtonCancel_Clicked;
            onButtonInputData_Clicked = onButtonOK_Clicked;

            textButtonLeft.text = "ĐÓNG";
            textButtonRight.text = "ĐỒNG Ý";

            Show(3);
        }

        public void ShowInputDataBox2(string title, string message, string message2, System.Action<string, string> onButtonOK_Clicked = null, System.Action onButtonCancel_Clicked = null)
        {
            textTitle.gameObject.SetActive(true);

            gameObjectButtonLeft.SetActive(true);
            gameObjectButtonRight.SetActive(true);

            textTitle.text = title;
            textMessage.text = message;

            onButtonLeft_Clicked = onButtonCancel_Clicked;
            onButtonInputData2_Clicked = onButtonOK_Clicked;

            textButtonLeft.text = "ĐÓNG";
            textButtonRight.text = "ĐỒNG Ý";

            Show(4);
        }

        public void Hide(System.Action onDone = null, bool resetAllAction = false)
        {
            if (sequenceShow != null)
            {
                sequenceShow.Kill();
                sequenceShow = null;
            }

            if (resetAllAction)
            {
                onButtonLeft_Clicked = delegate { };
                onButtonRight_Clicked = delegate { };
            }

            currentPriority = 0;
            canvasGroup.blocksRaycasts = false;
            IsShowing = false;

            sequenceShow = DOTween.Sequence();
            sequenceShow.Append(canvasGroup.DOFade(0, 0.35f));
            sequenceShow.Join(rectTransformMessageBox.DOScale(0, 0.35f).SetEase(Ease.InBack));
            sequenceShow.OnComplete(() =>
            {
                if (onDone != null)
                {
                    onDone();
                }
            });
        }

        #endregion Public Function

        #region Private Function

        private void Show(int priority)
        {
            if (currentPriority > priority)
            {
                return;
            }

            if (sequenceShow != null)
            {
                sequenceShow.Kill();
                sequenceShow = null;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransformMessageBox);
            Canvas.ForceUpdateCanvases();

            rectTransformMessageBox.localScale = Vector3.zero;
            canvasGroup.blocksRaycasts = false;

            sequenceShow = DOTween.Sequence();
            sequenceShow.Append(canvasGroup.DOFade(1, 0.35f));
            sequenceShow.Join(rectTransformMessageBox.DOScale(1, 0.35f).SetEase(Ease.OutBack));

            sequenceShow.OnComplete(() =>
            {
                IsShowing = true;
                canvasGroup.blocksRaycasts = true;
                currentPriority = priority;
            });
        }

        #endregion Private Function

        #region Event

        private void HandleButtonLeft_Clicked()
        {
            Hide(() =>
            {
                if (onButtonLeft_Clicked != null)
                {
                    onButtonLeft_Clicked();
                }
            });
        }

        private void HandleButtonRight_Clicked()
        {
            Hide(() =>
            {
                if (onButtonRight_Clicked != null)
                {
                    onButtonRight_Clicked();
                }
            });
        }

        #endregion Event

        #region Editor



        #endregion Editor
    }
}
