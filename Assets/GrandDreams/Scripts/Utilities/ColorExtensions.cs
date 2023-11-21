using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

namespace GrandDreams.Core.Utilities
{
    public static class ColorExtensions
    {
        public static Color SetAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        public static Color SetAlpha32(this Color color, int alpha)
        {
            color.a = alpha / 255f;
            return color;
        }

        public static void SetAlpha(this Image image, float alpha)
        {
            var tmpColor = image.color;
            tmpColor.a = alpha;
            image.color = tmpColor;
        }

        public static void SetAlpha(this RawImage image, float alpha)
        {
            var tmpColor = image.color;
            tmpColor.a = alpha;
            image.color = tmpColor;
        }

        public static void SetAlpha(this Text text, float alpha)
        {
            var tmpColor = text.color;
            tmpColor.a = alpha;
            text.color = tmpColor;
        }

        public static void SetAlpha(this SpriteRenderer spriteRenderer, float alpha)
        {
            var tmpColor = spriteRenderer.color;
            tmpColor.a = alpha;
            spriteRenderer.color = tmpColor;
        }

        public static void SetAlpha(this TextMeshProUGUI text, float alpha)
        {
            var tmpColor = text.color;
            tmpColor.a = alpha;
            text.color = tmpColor;
        }

        public static void SetAlpha(this CanvasGroup canvasGroup, float alpha)
        {
            canvasGroup.alpha = alpha;
        }
    }
}