using GrandDreams.Core.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDreams.Core.Components
{
    public class AutoCompleteItem : MonoBehaviour
    {
        #region Declare Variables

        public bool Interactable
        {
            set
            {
                btn.interactable = value;
            }
        }

        public System.Action<AutoCompleteItem, int> OnItem_Clicked;

        [SerializeField] private Text textContent;
        [SerializeField] private Button btn;

        private PoolingScrollItem scrollItem;

        #endregion Declare Variables

        private void Awake()
        {
            btn.GetComponent<Button>();
            btn.onClick.AddListener(HandleButton_Clicked);

            scrollItem = GetComponent<PoolingScrollItem>();
        }

        private void Start()
        {
            Reset();
        }

        private void OnDestroy()
        {
            btn.onClick.RemoveListener(HandleButton_Clicked);
        }

        #region Public Function

        public void Reset()
        {
            Interactable = true;
        }

        public void SetContent(string content)
        {
            textContent.text = content;
            Interactable = true;
        }

        #endregion Public Function

        #region Private Function



        #endregion Private Function

        #region Event

        private void HandleButton_Clicked()
        {
            Interactable = false;

            if (OnItem_Clicked != null)
            {
                OnItem_Clicked(this, scrollItem.Line);
            }
        }

        #endregion Event

        #region Editor



        #endregion Editor
    }
}