using DG.Tweening;
using GrandDreams.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrandDreams.Core
{
    public class RandomRotate : MonoBehaviour
    {

        #region Declare Variables

        private const float RATE_TIME_DURATION = 3f / 360f;

        [SerializeField] private float speed = 1;
        [SerializeField] private float minAngle, maxAngle;

        private RectTransform rectTransform;
        private Tween tweenRotate;

        #endregion Declare Variables

        private void Awake()
        {
            rectTransform = transform as RectTransform;
        }

        private void OnDestroy()
        {
            tweenRotate = tweenRotate.Initialize();
        }

        private void Start()
        {
            Rotate();
        }

        #region Public Function



        #endregion Public Function

        #region Private Function

        private void Rotate()
        {
            tweenRotate = tweenRotate.Initialize();
            float angle = Random.Range(minAngle, maxAngle);
            float timeDuration = Mathf.Abs(angle) * RATE_TIME_DURATION * Random.Range(0.8f, 1.2f) / speed;
            tweenRotate = rectTransform.DOLocalRotate(new Vector3(0, 0, rectTransform.localEulerAngles.z + angle), timeDuration, RotateMode.FastBeyond360).SetEase(Ease.InQuad);

            tweenRotate.OnComplete(() =>
            {
                Rotate();
            });
        }

        #endregion Private Function

        #region Event



        #endregion Event

        #region Editor



        #endregion Editor



    }
}