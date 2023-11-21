using DG.Tweening;
using GrandDreams.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDreams.Core.Components
{
    [RequireComponent(typeof(InputField))]
    public class AutoCompleteField : MonoBehaviour
    {
        #region Declare Variables

        private const float ITEM_BUTTON_HEIGHT = 95;
        private const float ITEM_BUTTON_SPACING = 10;
        private const float ITEM_BUTTON_PADDING_VALUE = 22;
        private const float MAX_HEIGHT_PANEL_SEARCHING = 515;

        public List<string> Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;

                filteredData = data.Clone();
                TotalLine = filteredData.Count();
            }
        }

        private int totalLine = 0;
        private int TotalLine
        {
            get
            {
                return totalLine;
            }
            set
            {
                totalLine = value;
                allowToShowPanelAutoComplete = totalLine > 0;
                if (allowToShowPanelAutoComplete)
                {
                    SetHeightPanelAutoComplete(totalLine);
                }
                else
                {
                    HidePanelAutoComplete(true);
                }
            }
        }

        [SerializeField] private PoolingScrollRect poolingScrollRect;
        [SerializeField] private RectTransform rtPanelAutoComplete;
        [SerializeField] private Button btnClosePanelAutoComplete;

        private InputField inputField;
        private Image imgButtonClosePanelAutoComplete;

        private List<PoolingScrollItem> scrollerItems;
        private List<AutoCompleteItem> items;

        private bool oldFocusState = false, allowToShowPanelAutoComplete = true;

        List<string> data, filteredData;

        #endregion Declare Variables

        private void Awake()
        {
            inputField = GetComponent<InputField>();
            if (btnClosePanelAutoComplete != null)
            {
                imgButtonClosePanelAutoComplete = btnClosePanelAutoComplete.GetComponent<Image>();
                btnClosePanelAutoComplete.onClick.AddListener(HandleButtonBGPanelAutoComplete_Clicked);
            }

            inputField.onValueChanged.AddListener(HandleInputField_ValueChanged);

            poolingScrollRect.OnPoolingScrollRect_Initialized += HandlePoolingScrollRect_Initialized;
        }

        private void OnDestroy()
        {
            if (btnClosePanelAutoComplete != null)
            {
                btnClosePanelAutoComplete.onClick.RemoveListener(HandleButtonBGPanelAutoComplete_Clicked);
            }

            inputField.onValueChanged.RemoveListener(HandleInputField_ValueChanged);

            poolingScrollRect.OnPoolingScrollRect_Initialized -= HandlePoolingScrollRect_Initialized;
        }

        private void Update()
        {
            if (inputField.isFocused && !oldFocusState && allowToShowPanelAutoComplete)
            {
                oldFocusState = true;

                if (btnClosePanelAutoComplete != null)
                {
                    imgButtonClosePanelAutoComplete.raycastTarget = true;
                }
                rtPanelAutoComplete.gameObject.SetActive(true);
                TotalLine = filteredData.Count;
            }
        }

        #region Public Function


        #endregion Public Function

        #region Private Function

        private void ResetPanelAutoComplete()
        {
            if (items == null)
            {
                return;
            }

            for (int index = 0; index < items.Count; index++)
            {
                items[index].Reset();
                items[index].SetContent(filteredData[index]);
            }

            poolingScrollRect.ResetScrollbar();
        }

        private void HidePanelAutoComplete(bool clearAllItem = false)
        {
            if (clearAllItem)
            {
                items = new List<AutoCompleteItem>();
            }

            oldFocusState = false;

            imgButtonClosePanelAutoComplete.raycastTarget = false;
            ResetPanelAutoComplete();
            rtPanelAutoComplete.gameObject.SetActive(false);
        }

        #endregion Private Function

        #region Event

        private void HandleButtonBGPanelAutoComplete_Clicked()
        {
            HidePanelAutoComplete();
        }


        private Tween tweenDelayToExecuteSearch;
        private void HandleInputField_ValueChanged(string text)
        {
            tweenDelayToExecuteSearch = tweenDelayToExecuteSearch.Initialize();

            tweenDelayToExecuteSearch = DOVirtual.DelayedCall(0.4f, () =>
            {
                if (text == "")
                {
                    filteredData = data.Clone();
                }
                else
                {
                    filteredData = data.Where(x => x.ToUpper().Contains(text.ToUpper())).ToList();
                }

                TotalLine = filteredData.Count;
            });
        }

        private void SetHeightPanelAutoComplete(int totalLine)
        {
            float height = totalLine * ITEM_BUTTON_HEIGHT + (totalLine - 1) * ITEM_BUTTON_SPACING + ITEM_BUTTON_PADDING_VALUE;
            height = height > MAX_HEIGHT_PANEL_SEARCHING ? MAX_HEIGHT_PANEL_SEARCHING : height;
            rtPanelAutoComplete.SetSizeDeltaY(height);
            poolingScrollRect.InitializeItems(totalLine);
        }

        private void HandlePoolingScrollRect_Initialized(List<PoolingScrollItem> scrollerItems)
        {
            this.scrollerItems = scrollerItems;

            items = new List<AutoCompleteItem>();

            for (int index = 0; index < scrollerItems.Count; index++)
            {
                int tmpIndex = index;
                this.scrollerItems[tmpIndex].OnPoolingScrollItem_StepIndexUpdated += HandlePoolingScrollItem_StepIndexUpdated;

                AutoCompleteItem item = this.scrollerItems[tmpIndex].GetComponent<AutoCompleteItem>();
                item.SetContent(filteredData[tmpIndex]);
                item.OnItem_Clicked += HandleItem_Clicked;

                items.Add(item);
            }
        }

        private void HandlePoolingScrollItem_StepIndexUpdated(int itemIndex, int line)
        {
            if (allowToShowPanelAutoComplete)
            {
                items[itemIndex].SetContent(filteredData[line]);
            }
        }

        private void HandleItem_Clicked(AutoCompleteItem item, int line)
        {
            inputField.text = filteredData[line];

            HidePanelAutoComplete();
        }

        #endregion Event

        #region Editor



        #endregion Editor



    }
}