using GrandDreams.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDreams.Core.Components
{
    public class PoolingScrollRect : MonoBehaviour
    {

        #region Declare Variables

        public System.Action<List<PoolingScrollItem>> OnPoolingScrollRect_Initialized = delegate { };

        private float scrollRectHeight = 0;
        public float ScrollRectHeight
        {
            get
            {
                return scrollRectHeight;
            }
        }

        private float contentRectHeight = 0;
        public float ContentRectHeight
        {
            get
            {
                return contentRectHeight;
            }
        }

        private float itemHeight = 0;
        public float ItemHeight
        {
            get
            {
                return itemHeight;
            }
        }

        private int totalItem = 0;
        public int TotalItem            //Number of scrollerItem generated
        {
            get
            {
                return totalItem;
            }
        }

        protected int totalLine = 0;
        public int TotalLine            //Number of line of data
        {
            get
            {
                return totalLine;
            }
            set
            {
                this.totalLine = value;
                rectTransformContent.SetSizeDeltaY(totalLine * itemHeight + (totalLine - 1) * itemSpacing + paddingTop + paddingBot);
                ForceRefreshList();

                for (int index = 0; index < scrollItems.Count; index++)
                {
                    scrollItems[index].TotalLine = totalLine;
                }
            }
        }

        [SerializeField] private RectTransform rectTransformPrefabItem;
        [SerializeField] private RectTransform rectTransformContent;
        [SerializeField] private int paddingTop, paddingBot;
        [SerializeField] private float itemSpacing = 0;

        protected RectTransform rectTransform;

        protected ScrollRect scrollRect;

        protected List<PoolingScrollItem> scrollItems;

        protected float oldPosY = 0;

        protected float currentPosY = 0;
        protected float fullItemHeight = 0;
        protected float fullRectHeight = 0;

        #endregion Declare Variables

        private void Awake()
        {
            rectTransform = transform as RectTransform;

            scrollRect = GetComponent<ScrollRect>();

            scrollItems = new List<PoolingScrollItem>();

            scrollRect.onValueChanged.AddListener(HandleScrollRect_ValueChanged);
        }

        private void OnDestroy()
        {
            OnPoolingScrollRect_Initialized = delegate { };
        }

        #region Public Function

        private IEnumerator ieInitialize;
        public void InitializeItems(int totalLine)
        {
            if (ieInitialize != null)
            {
                StopCoroutine(ieInitialize);
                ieInitialize = null;
            }

            ieInitialize = InitializeItemsRoutine(totalLine);
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(ieInitialize);
            }
        }

        public void ResetScrollbar(System.Action onDone = null)
        {
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(ResetScrollbarRoutine(onDone));
            }
        }

        public void GetCurrentItemPosition(out int itemIndex, out int stepIndex)
        {
            currentPosY = rectTransformContent.anchoredPosition.y;

            stepIndex = (int)(currentPosY / (fullRectHeight));
            float remainderPos = currentPosY % (fullRectHeight);
            itemIndex = (int)(remainderPos / fullItemHeight);
        }

        #endregion Public Function

        #region Private Function

        private IEnumerator ResetScrollbarRoutine(System.Action onDone = null)
        {
            rectTransformContent.SetAnchoredPosY(0);
            yield return new WaitForEndOfFrame();

            if (onDone != null)
            {
                onDone();
            }
        }

        private IEnumerator InitializeItemsRoutine(int totalLine)
        {
            rectTransformContent.Clear();
            scrollItems = new List<PoolingScrollItem>();

            scrollRectHeight = rectTransform.rect.height;
            itemHeight = rectTransformPrefabItem.rect.height;

            TotalLine = totalLine;
            contentRectHeight = totalLine == 0 ? 0 : (totalLine * itemHeight + (totalLine -1) * itemSpacing + paddingTop + paddingBot);

            var remainder = scrollRectHeight % (itemHeight + itemSpacing);
            var tmpTotalItem = (int)((scrollRectHeight - paddingBot - paddingTop + itemSpacing) / (itemHeight + itemSpacing));
            if(tmpTotalItem == 0)
            {
                totalItem = 0;
            }
            else
            {
                totalItem = contentRectHeight <= scrollRectHeight ? tmpTotalItem : (remainder == 0 ? tmpTotalItem + 2 : tmpTotalItem + 3);
                totalItem = totalItem > totalLine ? totalLine : totalItem;
            }

            fullItemHeight = itemHeight + itemSpacing;
            fullRectHeight = fullItemHeight * totalItem;

            for (int index = 0; index < totalItem; index++)
            {
                InstantiateItem(index);
            }

            yield return new WaitForEndOfFrame();

            TotalLine = totalLine;

            if (OnPoolingScrollRect_Initialized != null)
            {
                OnPoolingScrollRect_Initialized(scrollItems);
            }
        }

        protected virtual void InstantiateItem(int itemIndex)
        {
            RectTransform rt = Instantiate<RectTransform>(rectTransformPrefabItem);
            rt.SetParent(rectTransformContent);
            rt.localScale = Vector3.one;
            rt.name = string.Format("Item ({0})", itemIndex);
            rt.anchoredPosition3D = Vector3.zero;

            PoolingScrollItem scrollItem = rt.GetComponent<PoolingScrollItem>();

            scrollItem.Initialize(itemIndex, totalItem, itemSpacing, paddingTop);

            scrollItems.Add(scrollItem);
        }

        protected virtual void ForceRefreshList()
        {
            currentPosY = rectTransformContent.anchoredPosition.y;

            int currentStep = (int)(currentPosY / (fullRectHeight));
            float remainderPos = currentPosY % (fullRectHeight);
            int currentItemStep = (int)(remainderPos / fullItemHeight);

            for (int index = 0; index < scrollItems.Count; index++)
            {
                if (currentItemStep > index)
                {
                    if (index == 0)
                    {
                        scrollItems[scrollItems.Count - 1].StepIndex = currentStep;
                    }
                    if (scrollItems[index].StepIndex <= currentStep && (currentStep + 1) * totalItem + index < totalLine)
                    {
                        scrollItems[index].StepIndex = currentStep + 1;
                    }
                }
            }
        }

        #endregion Private Function

        #region Event

        protected virtual void HandleScrollRect_ValueChanged(Vector2 value)
        {
            for (int index = 0; index < scrollItems.Count; index++)
            {
                scrollItems[index].HandleOnParent_PositionUpdated(rectTransformContent.anchoredPosition);
            }
        }

        #endregion Event

        #region Editor



        #endregion Editor

    }
}