using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrandDreams.Core.Components
{
    public class GDToggleGroup : MonoBehaviour, IGDToggleGroup
    {


        #region Declare Variables

        [SerializeField] private bool allowSelectMultiple = false;
        public bool AllowSelectMultiple
        {
            get
            {
                return allowSelectMultiple;
            }
            set
            {
                allowSelectMultiple = value;
            }
        }

        private List<IGDToggle> toggles;

        #endregion Declare Variables

        private void Awake()
        {
            toggles = new List<IGDToggle>();
        }

        private void OnDestroy()
        {
        }

        #region Public Function

        public void AddToggle(IGDToggle toggle)
        {
            toggles.Add(toggle);
        }

        public void BroadcastToggleState(IGDToggle toggleSender)
        {
            if(AllowSelectMultiple)
            {
                return;
            }

            for (int index = 0; index < toggles.Count; index++)
            {
                if (toggles[index] != toggleSender)
                {
                    toggles[index].IsOn = false;
                }
            }
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