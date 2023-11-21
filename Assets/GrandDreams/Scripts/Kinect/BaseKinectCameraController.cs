using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDreams.Core.Kinect
{
    public class BaseKinectCameraController : MonoBehaviour
    {
        #region Declare Variables

        public RawImage RawImageColorTex
        {
            get
            {
                return rawImageColorTex;
            }
        }

        public RawImage RawImageForeground
        {
            get
            {
                return rawImageForeground;
            }
        }

        [SerializeField] protected RawImage rawImageColorTex, rawImageForeground;

        protected BackgroundRemovalManager backManager;
        protected KinectManager kinectManager;

        #endregion Declare Variables

        protected virtual IEnumerator Start()
        {
            yield return new WaitUntil(() => KinectManager.Instance != null);
            kinectManager = KinectManager.Instance;

            if (rawImageForeground != null)
            {
                yield return new WaitUntil(() => BackgroundRemovalManager.Instance != null);
                backManager = BackgroundRemovalManager.Instance;
            }
        }

        protected virtual void Update()
        {
            if (rawImageColorTex && rawImageColorTex.texture == null)
            {
                if (kinectManager)
                {
                    rawImageColorTex.texture = kinectManager.GetUsersClrTex2D();
                    rawImageColorTex.rectTransform.localScale = kinectManager.GetColorImageScale();
                    rawImageColorTex.color = Color.white;
                }
            }

            if (rawImageForeground && rawImageForeground.texture == null)
            {
                if (kinectManager && backManager && backManager.enabled)
                {
                    rawImageForeground.texture = backManager.GetForegroundTex();
                    rawImageForeground.rectTransform.localScale = kinectManager.GetColorImageScale();
                    rawImageForeground.color = Color.white;
                }
            }
        }

        #region Public Function

        public virtual void HideColorTex()
        {
            if (rawImageColorTex && rawImageColorTex.texture != null)
            {
                rawImageColorTex.texture = null;
                rawImageColorTex.color = Color.clear;
            }
        }

        public virtual void HideForeground()
        {
            if (rawImageForeground && rawImageForeground.texture != null)
            {
                rawImageForeground.texture = null;
                rawImageForeground.color = Color.clear;
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