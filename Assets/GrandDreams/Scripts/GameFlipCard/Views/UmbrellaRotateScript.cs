using UnityEngine;
using DG.Tweening;
using GrandDreams.Core.Utilities;

namespace GrandDreams.Game.FlipCard.Views
{
    public class UmbrellaRotateScript : MonoBehaviour
    {
        [SerializeField] private Vector2 delayTime;
        [SerializeField] private Vector3 rotateFrom;
        [SerializeField] private Vector3 rotateTo;
        [SerializeField] private float timeRotate;

        private Tween tweenRotate;
        private RectTransform rectUmbrella;

        // Update is called once per frame
        public void StartRotate()
        {
            rectUmbrella = this.GetComponent<RectTransform>();
            rectUmbrella.localRotation = Quaternion.Euler(rotateFrom);
            // RotateMode.Fast di chuyển 
            tweenRotate = tweenRotate.Initialize();
            tweenRotate = rectUmbrella.DOLocalRotate(rotateTo, timeRotate, RotateMode.Fast).SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo).SetDelay(Random.Range(delayTime.x, delayTime.y));
        }

        public void StopRotate()
        {
            tweenRotate = tweenRotate.Initialize();
        }

        private void OnDestroy()
        {
            if(tweenRotate != null)
            {
                tweenRotate.Initialize(false);
            }
        }
    }
}