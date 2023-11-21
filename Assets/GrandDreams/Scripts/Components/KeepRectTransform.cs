using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrandDreams.Core.Components
{
    public class KeepRectTransform : MonoBehaviour
    {
        #region Declare Variables

        [SerializeField] private bool keepPosition, keepRotation, keepScale;

        private RectTransform rectTransform;

        private Vector3 originPosition, originRotation, originScale;

        #endregion Declare Variables

        private void Awake()
        {
            rectTransform = transform as RectTransform;

            originPosition = rectTransform.position;
            originRotation = rectTransform.eulerAngles;
            originScale = rectTransform.localScale;
        }

        private void LateUpdate()
        {
            if (keepPosition)
            {
                rectTransform.position = originPosition;
            }

            if (keepRotation)
            {
                rectTransform.eulerAngles = originRotation;
            }

            if (keepScale)
            {
                rectTransform.localScale = originScale;
            }
        }

        #region Public Function



        #endregion Public Function

        #region Private Function



        #endregion Private Function

        #region Event



        #endregion Event

        #region Editor



        #endregion Editor
    }
}