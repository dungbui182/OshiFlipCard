using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace GrandDreams.Core.Utilities
{
    public enum ERotateFlip
    {
        None_None, Clockwise_None, CounterClockwise_None, None_FlippedX, Clockwise_FlippedX, CounterClockwise_FlippedX, None_FlippedY, Clockwise_FlippedY, CounterClockwise_FlippedY
    }

    public static class TextureExtensions
    {
        public static Texture2D TransformTexture(this Texture2D texture, ERotateFlip rotateFlip)
        {
            if (rotateFlip == ERotateFlip.None_None)
            {
                return texture;
            }

            Color32[] original = texture.GetPixels32();

            int length = original.Length;

            Color32[] result = new Color32[length];
            int w = texture.width;
            int h = texture.height;

            int iResult, iOriginal;

            Texture2D resultTexture = null;

            switch (rotateFlip)
            {
                case ERotateFlip.Clockwise_None:

                    resultTexture = new Texture2D(h, w);

                    for (int j = 0; j < h; ++j)
                    {
                        for (int i = 0; i < w; ++i)
                        {
                            iResult = (i + 1) * h - j - 1;
                            iOriginal = length - 1 - (j * w + i);
                            result[iResult] = original[iOriginal];
                        }
                    }
                    break;

                case ERotateFlip.CounterClockwise_None:

                    resultTexture = new Texture2D(h, w);

                    for (int j = 0; j < h; ++j)
                    {
                        for (int i = 0; i < w; ++i)
                        {
                            iResult = (i + 1) * h - j - 1;
                            iOriginal = j * w + i;
                            result[iResult] = original[iOriginal];
                        }
                    }
                    break;

                case ERotateFlip.None_FlippedX:

                    resultTexture = new Texture2D(w, h);

                    for (int j = 0; j < h; ++j)
                    {
                        for (int i = 0; i < w; ++i)
                        {
                            iResult = j * w + w - 1 - i;
                            iOriginal = j * w + i;
                            result[iResult] = original[iOriginal];
                        }
                    }
                    break;
                case ERotateFlip.Clockwise_FlippedX:

                    resultTexture = new Texture2D(h, w);

                    for (int j = 0; j < h; ++j)
                    {
                        for (int i = 0; i < w; ++i)
                        {
                            iResult = (w - i) * h - j - 1;
                            iOriginal = length - 1 - (j * w + i);
                            result[iResult] = original[iOriginal];
                        }
                    }
                    break;
                case ERotateFlip.CounterClockwise_FlippedX:

                    resultTexture = new Texture2D(h, w);

                    for (int j = 0; j < h; ++j)
                    {
                        for (int i = 0; i < w; ++i)
                        {
                            iResult = (w - i) * h - j - 1;
                            iOriginal = j * w + i;
                            result[iResult] = original[iOriginal];
                        }
                    }
                    break;
                case ERotateFlip.None_FlippedY:

                    resultTexture = new Texture2D(w, h);

                    for (int j = 0; j < h; ++j)
                    {
                        for (int i = 0; i < w; ++i)
                        {
                            iResult = (h - 1 - j) * w + i;
                            iOriginal = j * w + i;
                            result[iResult] = original[iOriginal];
                        }
                    }
                    break;
                case ERotateFlip.Clockwise_FlippedY:

                    resultTexture = new Texture2D(h, w);

                    for (int j = 0; j < h; ++j)
                    {
                        for (int i = 0; i < w; ++i)
                        {
                            iResult = (i + 1) * h + j - h;
                            iOriginal = length - 1 - (j * w + i);
                            result[iResult] = original[iOriginal];
                        }
                    }
                    break;
                case ERotateFlip.CounterClockwise_FlippedY:

                    resultTexture = new Texture2D(h, w);

                    for (int j = 0; j < h; ++j)
                    {
                        for (int i = 0; i < w; ++i)
                        {
                            iResult = (i + 1) * h + j - h;
                            iOriginal = j * w + i;
                            result[iResult] = original[iOriginal];
                        }
                    }
                    break;
            }

            resultTexture.SetPixels32(result);
            resultTexture.Apply();
            return resultTexture;
        }

        public static Texture2D TransformTexture(this Color[] original, int w, int h, ERotateFlip rotateFlip)
        {
            int length = original.Length;

            Color32[] result = new Color32[length];

            int iResult, iOriginal;
            Texture2D resultTexture = null;

            switch (rotateFlip)
            {
                case ERotateFlip.None_None:
                    resultTexture = new Texture2D(w, h);
                    break;
                case ERotateFlip.Clockwise_None:
                    resultTexture = new Texture2D(h, w);
                    for (int j = 0; j < h; ++j)
                    {
                        for (int i = 0; i < w; ++i)
                        {
                            iResult = (i + 1) * h - j - 1;
                            iOriginal = length - 1 - (j * w + i);
                            result[iResult] = original[iOriginal];
                        }
                    }
                    break;

                case ERotateFlip.CounterClockwise_None:
                    resultTexture = new Texture2D(h, w);
                    for (int j = 0; j < h; ++j)
                    {
                        for (int i = 0; i < w; ++i)
                        {
                            iResult = (i + 1) * h - j - 1;
                            iOriginal = j * w + i;
                            result[iResult] = original[iOriginal];
                        }
                    }
                    break;

                case ERotateFlip.None_FlippedX:
                    resultTexture = new Texture2D(w, h);
                    for (int j = 0; j < h; ++j)
                    {
                        for (int i = 0; i < w; ++i)
                        {
                            iResult = j * w + w - 1 - i;
                            iOriginal = j * w + i;
                            result[iResult] = original[iOriginal];
                        }
                    }
                    break;
                case ERotateFlip.Clockwise_FlippedX:
                    resultTexture = new Texture2D(h, w);
                    for (int j = 0; j < h; ++j)
                    {
                        for (int i = 0; i < w; ++i)
                        {
                            iResult = (w - i) * h - j - 1;
                            iOriginal = length - 1 - (j * w + i);
                            result[iResult] = original[iOriginal];
                        }
                    }
                    break;
                case ERotateFlip.CounterClockwise_FlippedX:
                    resultTexture = new Texture2D(h, w);
                    for (int j = 0; j < h; ++j)
                    {
                        for (int i = 0; i < w; ++i)
                        {
                            iResult = (w - i) * h - j - 1;
                            iOriginal = j * w + i;
                            result[iResult] = original[iOriginal];
                        }
                    }
                    break;
                case ERotateFlip.None_FlippedY:
                    resultTexture = new Texture2D(w, h);
                    for (int j = 0; j < h; ++j)
                    {
                        for (int i = 0; i < w; ++i)
                        {
                            iResult = (h - 1 - j) * w + i;
                            iOriginal = j * w + i;
                            result[iResult] = original[iOriginal];
                        }
                    }
                    break;
                case ERotateFlip.Clockwise_FlippedY:
                    resultTexture = new Texture2D(h, w);
                    for (int j = 0; j < h; ++j)
                    {
                        for (int i = 0; i < w; ++i)
                        {
                            iResult = (i + 1) * h + j - h;
                            iOriginal = length - 1 - (j * w + i);
                            result[iResult] = original[iOriginal];
                        }
                    }
                    break;
                case ERotateFlip.CounterClockwise_FlippedY:
                    resultTexture = new Texture2D(h, w);
                    for (int j = 0; j < h; ++j)
                    {
                        for (int i = 0; i < w; ++i)
                        {
                            iResult = (i + 1) * h + j - h;
                            iOriginal = j * w + i;
                            result[iResult] = original[iOriginal];
                        }
                    }
                    break;
            }

            resultTexture.SetPixels32(result);
            resultTexture.Apply();
            return resultTexture;
        }

        public static void LoadFromFile(this Image image, string filePath, float pixelsPerUnit = 100.0f)
        {
#if UNITY_EDITOR
            filePath.CopyToClipboard();
#endif
            Texture2D spriteTexture = LoadTexture(filePath);
            Sprite newSprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0, 0), pixelsPerUnit);

            image.sprite = newSprite;
        }

        public static Sprite ToSprite(this Texture2D tex, float pixelsPerUnit = 100.0f)
        {
            Sprite newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0), pixelsPerUnit);
            return newSprite;
        }

        public static void LoadFromFile(this RawImage rawImage, string filePath)
        {
            rawImage.texture = LoadTexture(filePath);
        }

        public static Texture2D LoadTexture(string filePath)
        {
            Texture2D tex2D;
            byte[] fileData;

            if (System.IO.File.Exists(filePath))
            {
                fileData = System.IO.File.ReadAllBytes(filePath);
                tex2D = new Texture2D(2, 2);
                if (tex2D.LoadImage(fileData))
                    return tex2D;
            }
            return null;
        }

        public static void SaveToLocal(this Texture2D texture, string finalImageName, int imageQuality = 70)
        {
            string imagePath = System.IO.Path.GetDirectoryName(finalImageName);
            Debug.Log(finalImageName);

            byte[] imageData = texture.EncodeToJPG(imageQuality);
            Debug.Log(texture == null);

            FileHelpers.CreateDirectoryIfNotExist(imagePath);

            System.IO.File.WriteAllBytes(finalImageName, imageData);
        }

        public static Texture2D ToTexture(this string filePath)
        {
            Texture2D tex2D;
            byte[] fileData;

            if (System.IO.File.Exists(filePath))
            {
                fileData = System.IO.File.ReadAllBytes(filePath);
                tex2D = new Texture2D(2, 2);
                if (tex2D.LoadImage(fileData))
                    return tex2D;
            }
            return null;
        }

        public static byte[] ToTextureData(this string filePath)
        {
            byte[] fileData;

            if (System.IO.File.Exists(filePath))
            {
                fileData = System.IO.File.ReadAllBytes(filePath);
                return fileData;
            }
            return null;
        }

        public static Texture2D ToTexture2D(this RenderTexture rTex, Rect rect)
        {
            Texture2D tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
            RenderTexture.active = rTex;
            tex.ReadPixels(rect, 0, 0);
            tex.Apply();
            return tex;
        }

        public static Texture2D Crop(this Texture2D tex, RectInt rect)
        {
            Color[] croppedColors = tex.GetPixels(rect.x, rect.y, rect.width, rect.height);
            Texture2D croppedTex = new Texture2D(rect.width, rect.height);
            croppedTex.SetPixels(croppedColors);
            croppedTex.Apply();

            return croppedTex;
        }

        public static void LoadFromUrl(this Image image, string url, System.Action onSucceed = null, System.Action<string> onFailed = null)
        {
            image.StartCoroutine(LoadFromUrlRoutine(image, url, onSucceed, onFailed));
        }

        private static System.Collections.IEnumerator LoadFromUrlRoutine(Image image, string url, System.Action onSucceed = null, System.Action<string> onFailed = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                image.sprite = null;
                yield break;
            }

            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                image.sprite = null;
                image.SetAlpha(0);
                if (onFailed != null)
                {
                    onFailed(www.error);
                }
            }
            else
            {
                Texture downloadedTexture2D = ((DownloadHandlerTexture)www.downloadHandler).texture;
                image.sprite = (downloadedTexture2D as Texture2D).ToSprite();
                image.SetAlpha(1);

                if (onSucceed != null)
                {
                    onSucceed();
                }
            }

            Resources.UnloadUnusedAssets();
            www.Dispose();
        }

        public static void LoadFromUrl(this RawImage rawImage, string url, System.Action onSucceed = null, System.Action<string> onFailed = null)
        {
            rawImage.StartCoroutine(LoadFromUrlRoutine(rawImage, url, onSucceed, onFailed));
        }

        private static System.Collections.IEnumerator LoadFromUrlRoutine(RawImage rawImage, string url, System.Action onSucceed = null, System.Action<string> onFailed = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                rawImage.texture = null;
                yield break;
            }

            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                rawImage.texture = null;
                rawImage.SetAlpha(0);

                Debug.Log(www.error);
                if (onFailed != null)
                {
                    onFailed(www.error);
                }
            }
            else
            {
                Texture downloadedTexture2D = ((DownloadHandlerTexture)www.downloadHandler).texture;
                rawImage.texture = downloadedTexture2D;
                rawImage.SetAlpha(1);

                if (onSucceed != null)
                {
                    onSucceed();
                }
            }

            Resources.UnloadUnusedAssets();
            www.Dispose();
        }

        public static void DownloadTexture(this MonoBehaviour behaviour, string url, System.Action<Texture2D> onSucceed = null, System.Action<string> onFailed = null)
        {
            behaviour.StartCoroutine(DownloadTextureRoutine(url, onSucceed, onFailed));
        }

        public static System.Collections.IEnumerator DownloadTextureRoutine(string url, System.Action<Texture2D> onSucceed = null, System.Action<string> onFailed = null)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                if (onFailed != null)
                {
                    onFailed(www.error);
                }
            }
            else
            {
                Texture2D downloadedTexture2D = ((DownloadHandlerTexture)www.downloadHandler).texture;
                if (onSucceed != null)
                {
                    onSucceed(downloadedTexture2D);
                }
            }

            Resources.UnloadUnusedAssets();
            www.Dispose();
        }

        public static void DownloadTextures(this MonoBehaviour behaviour, List<string> urls, System.Action<List<Texture2D>> onSucceed = null, System.Action<string> onFailed = null)
        {
            behaviour.StartCoroutine(DownloadTexuturesRoutine(urls, onSucceed, onFailed));
        }

        public static void DownloadTextures(this MonoBehaviour behaviour, string[] urls, System.Action<List<Texture2D>> onSucceed = null, System.Action<string> onFailed = null)
        {
            behaviour.StartCoroutine(DownloadTexuturesRoutine(urls.ToList(), onSucceed, onFailed));
        }

        public static System.Collections.IEnumerator DownloadTexuturesRoutine(List<string> urls, System.Action<List<Texture2D>> onSucceed = null, System.Action<string> onFailed = null)
        {
            List<Texture2D> downloadedTextures = new List<Texture2D>();


            for (int index = 0; index < urls.Count; index++)
            {
                int tmpIndex = index;

#if UNITY_EDITOR
                Debug.Log("Downloading texture: " + urls[tmpIndex]);
#endif
                if (string.IsNullOrEmpty(urls[tmpIndex]))
                {
                    downloadedTextures.Add(null);
                    continue;
                }

                UnityWebRequest www = UnityWebRequestTexture.GetTexture(urls[tmpIndex]);
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    downloadedTextures.Add(null);
                }
                else
                {
                    Texture2D downloadedTexture2D = ((DownloadHandlerTexture)www.downloadHandler).texture;

                    downloadedTextures.Add(downloadedTexture2D);
                }

                www.Dispose();
            }

            Resources.UnloadUnusedAssets();

            if (onSucceed != null)
            {
                onSucceed(downloadedTextures);
            }
        }

        public static Texture2D DoResize(this Texture2D texture2D, int targetWidth, int targetHeight)
        {
            RenderTexture rt = new RenderTexture(targetWidth, targetHeight, 24);
            RenderTexture.active = rt;
            Graphics.Blit(texture2D, rt);
            Texture2D result = new Texture2D(targetWidth, targetHeight);
            result.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
            result.Apply();
            return result;
        }
    }
}
