using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GrandDreams.Core.Utilities
{
    public static class TransformExtensions
    {

        public static void SetAnchoredPosX(this RectTransform rectTransform, float posX)
        {
            Vector2 anchoredPos = rectTransform.anchoredPosition;
            anchoredPos.x = posX;
            rectTransform.anchoredPosition = anchoredPos;
        }

        public static void SetAnchoredPosY(this RectTransform rectTransform, float posY)
        {
            Vector2 anchoredPos = rectTransform.anchoredPosition;
            anchoredPos.y = posY;
            rectTransform.anchoredPosition = anchoredPos;
        }

        public static void SetAnchoredPosZ(this RectTransform rectTransform, float posZ)
        {
            Vector3 anchoredPos = rectTransform.anchoredPosition3D;
            anchoredPos.z = posZ;
            rectTransform.anchoredPosition3D = anchoredPos;
        }

        public static void AddAnchoredPosX(this RectTransform rectTransform, float posX)
        {
            Vector2 anchoredPos = rectTransform.anchoredPosition;
            anchoredPos.x = anchoredPos.x + posX;
            rectTransform.anchoredPosition = anchoredPos;
        }

        public static void AddAnchoredPosY(this RectTransform rectTransform, float posY)
        {
            Vector2 anchoredPos = rectTransform.anchoredPosition;
            anchoredPos.y = anchoredPos.y + posY;
            rectTransform.anchoredPosition = anchoredPos;
        }

        public static void SetSizeDeltaX(this RectTransform rectTransform, float x)
        {
            Vector2 sizeDelta = rectTransform.sizeDelta;
            sizeDelta.x = x;
            rectTransform.sizeDelta = sizeDelta;
        }

        public static void SetSizeDeltaY(this RectTransform rectTransform, float y)
        {
            Vector2 sizeDelta = rectTransform.sizeDelta;
            sizeDelta.y = y;
            rectTransform.sizeDelta = sizeDelta;
        }

        public static void MultiplyScaleX(this Transform transform, float multiplyValue)
        {
            Vector3 scaleValue = transform.localScale;
            scaleValue.x = scaleValue.x * multiplyValue;
            transform.localScale = scaleValue;
        }

        public static void SetScaleX(this Transform transform, float scaleValueX)
        {
            Vector3 scaleValue = transform.localScale;
            scaleValue.x = scaleValueX;
            transform.localScale = scaleValue;
        }

        public static void MultiplyScaleY(this Transform transform, float multiplyValue)
        {
            Vector3 scaleValue = transform.localScale;
            scaleValue.y = scaleValue.y * multiplyValue;
            transform.localScale = scaleValue;
        }

        public static void SetScaleY(this Transform transform, float scaleValueY)
        {
            Vector3 scaleValue = transform.localScale;
            scaleValue.y = scaleValueY;
            transform.localScale = scaleValue;
        }

        public static void MultiplyScaleZ(this Transform transform, float multiplyValue)
        {
            Vector3 scaleValue = transform.localScale;
            scaleValue.z = scaleValue.z * multiplyValue;
            transform.localScale = scaleValue;
        }

        public static void SetScaleZ(this Transform transform, float scaleValueZ)
        {
            Vector3 scaleValue = transform.localScale;
            scaleValue.z = scaleValueZ;
            transform.localScale = scaleValue;
        }

        public static void SetLeft(this RectTransform rectTransform, float left)
        {
            rectTransform.offsetMin = new Vector2(left, rectTransform.offsetMin.y);
        }

        public static void SetRight(this RectTransform rectTransform, float right)
        {
            rectTransform.offsetMax = new Vector2(-right, rectTransform.offsetMax.y);
        }

        public static void SetTop(this RectTransform rectTransform, float top)
        {
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -top);
        }

        public static void SetBottom(this RectTransform rectTransform, float bottom)
        {
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, bottom);
        }

        public static void SetOffset(this RectTransform rectTransform, float left, float top, float right, float bottom)
        {
            rectTransform.offsetMin = new Vector2(left, bottom);
            rectTransform.offsetMax = new Vector2(-right, -top);
        }

        public static Vector2 GetAnchoredPositionFromRect(this Rect rect)
        {
            return new Vector2(rect.x, rect.y);
        }

        public static void Clear(this Transform transform, params Transform[] exceptObjects)
        {
            foreach (Transform child in transform)
            {
                if (exceptObjects != null && exceptObjects.Contains(child))
                {
                    continue;
                }
                GameObject.Destroy(child.gameObject);
            }
        }

        public static void ClearButLast(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                if (transform.childCount == 1)
                {
                    break;
                }
                GameObject.Destroy(child.gameObject);
            }
        }

        public static void ClearButFirst(this Transform transform)
        {
            for (int i = transform.childCount - 1; i >= 1 ; i--)
            {
                GameObject.Destroy(transform.GetChild(i));
            }
        }

        public static void ClearImmeadiate(this Transform transform, params Transform[] exceptObjects)
        {
            foreach (Transform child in transform)
            {
                if (exceptObjects != null && exceptObjects.Contains(child))
                {
                    continue;
                }
                GameObject.DestroyImmediate(child.gameObject);
            }
        }

        public static void RemoveIfExist(this Transform transform, Transform removingTransform)
        {
            if (removingTransform.IsChildOf(transform))
            {
                GameObject.Destroy(removingTransform.gameObject);
            }
        }

        public static void CopyTransform(this Transform transform, Transform copyingTransform)
        {
            transform.localPosition = copyingTransform.localPosition;
            transform.localRotation = copyingTransform.localRotation;
            transform.localScale = copyingTransform.localScale;
        }
    }
}
