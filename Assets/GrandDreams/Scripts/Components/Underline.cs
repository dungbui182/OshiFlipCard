using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDreams.Core.Components
{
    [RequireComponent(typeof(Text))]
    [RequireComponent(typeof(ContentSizeFitter))]
    public class Underline : MonoBehaviour
    {

        #region Declare Variables

        [SerializeField] private float lineThickness = 3, lineDistance;

        private RectTransform rectTransform;
        private Text text;

        private ContentSizeFitter contentSizeFitter;

        #endregion Declare Variables

        private void Awake()
        {
            rectTransform = transform as RectTransform;

            rectTransform.anchorMin = rectTransform.anchorMax = rectTransform.pivot = new Vector2(0.5f, 0.5f);

            text = GetComponent<Text>();

            contentSizeFitter = GetComponent<ContentSizeFitter>();
            if(contentSizeFitter == null)
            {
                contentSizeFitter = gameObject.AddComponent<ContentSizeFitter>();
            }

            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            GameObject gameObjectUnderline = new GameObject("Underline");
            RectTransform rectTransformUnderline = gameObjectUnderline.AddComponent<RectTransform>();
            Image image = gameObjectUnderline.AddComponent<Image>();
            image.color = text.color;
            rectTransformUnderline.SetParent(rectTransform);

            rectTransformUnderline.localScale = Vector3.one;

            rectTransformUnderline.anchorMin = new Vector2(0, 0);
            rectTransformUnderline.anchorMax = new Vector2(1, 0);
            rectTransformUnderline.pivot = new Vector2(0.5f, 0);
            var rect = rectTransform.rect;

            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

            rectTransformUnderline.sizeDelta = new Vector2(rect.width, lineThickness);
            rectTransformUnderline.anchoredPosition3D = new Vector3(rect.x + rect.width / 2f, -lineDistance - lineThickness, 0);
        }

        private void OnValidate()
        {
            contentSizeFitter = GetComponent<ContentSizeFitter>();
            if (contentSizeFitter != null)
            {
                contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
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