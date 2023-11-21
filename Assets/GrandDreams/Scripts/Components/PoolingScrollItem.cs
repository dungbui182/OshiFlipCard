using DG.Tweening;
using GrandDreams.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrandDreams.Core.Components
{
    public class PoolingScrollItem : MonoBehaviour
    {

        #region Declare Variables

        private int stepIndex = 0;
        public int StepIndex
        {
            get
            {
                return stepIndex;
            }
            set
            {
                int idLine = itemIndex + totalItem * value;
                if (idLine >= TotalLine)
                {
                    return;
                }

                stepIndex = value;
                rectTransform.SetAnchoredPosY(-paddingTop - (itemHeight + itemSpacing) * idLine);
                if (OnPoolingScrollItem_StepIndexUpdated != null)
                {
                    OnPoolingScrollItem_StepIndexUpdated(itemIndex, idLine);
                }
            }
        }

        public int Line
        {
            get
            {
                return itemIndex + totalItem * stepIndex;
            }
        }

        public int TotalLine { get; set; }

        public System.Action<int, int> OnPoolingScrollItem_StepIndexUpdated = delegate { };

        protected RectTransform rectTransform;

        protected int totalLine;
        protected int itemIndex;
        protected int totalItem;
        protected int paddingTop;
        protected float itemSpacing;
        protected float itemHeight = 0;
        protected float totalItemHeight;

        #endregion Declare Variables

        protected virtual void Awake()
        {
        }

        protected virtual void OnDestroy()
        {
            OnPoolingScrollItem_StepIndexUpdated = delegate { };
        }


        #region Public Function

        public virtual void Initialize(int itemIndex, int totalItem, float itemSpacing, int paddingTop)
        {
            rectTransform = transform as RectTransform;
            rectTransform.anchoredPosition = Vector2.zero;

            itemHeight = rectTransform.rect.height;

            this.itemIndex = itemIndex;
            this.totalItem = totalItem;
            this.itemSpacing = itemSpacing;
            this.paddingTop = paddingTop;

            StepIndex = 0;
            totalItemHeight = totalItem * (itemHeight + itemSpacing);
        }

        public void HandleOnParent_PositionUpdated(Vector2 anchoredPos)
        {
            if (totalItemHeight == 0)
            {
                return;
            }

            var y = anchoredPos.y;
            var tmpValue = (y - (itemIndex + 1) * (itemHeight + itemSpacing)) / totalItemHeight;
            StepIndex = tmpValue < 0 ? 0 : (int)(tmpValue + 1);
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