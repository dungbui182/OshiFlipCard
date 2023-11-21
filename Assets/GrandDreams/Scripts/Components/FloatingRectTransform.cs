using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrandDreams.Core.Components
{
    public class FloatingRectTransform : MonoBehaviour
    {
        #region Declare Variables

        public bool IsFloating { get; set; }

        [SerializeField] private Vector2 rangeFloating;
        [SerializeField] private float speed = 1, aceleration = 0.01f;

        private float currentSpeedX, currentSpeedY, targetPosX, targetPosY, acelerationX, acelerationY;

        private RectTransform rectTransform;

        private Vector2 originPosition, currentPosition;

        #endregion Declare Variables

        private void Awake()
        {
            rectTransform = transform as RectTransform;

            originPosition = rectTransform.anchoredPosition;
            currentPosition = rectTransform.anchoredPosition;

            targetPosX = Random.Range(-rangeFloating.x, rangeFloating.x) + originPosition.x;
            acelerationX = targetPosX > currentPosition.x ? aceleration : -aceleration;
            targetPosY = Random.Range(-rangeFloating.y, rangeFloating.y) + originPosition.y;
            acelerationY = targetPosY > currentPosition.y ? aceleration : -aceleration;

            IsFloating = true;
        }

        private void OnDestroy()
        {

        }

        private void Update()
        {
            if (IsFloating)
            {
                currentSpeedX = acelerationX >= 0 ? (currentSpeedX >= speed ? speed : currentSpeedX + acelerationX) : (currentSpeedX < -speed ? -speed : currentSpeedX + acelerationX);
                currentSpeedY = acelerationY >= 0 ? (currentSpeedY >= speed ? speed : currentSpeedY + acelerationY) : (currentSpeedY < -speed ? -speed : currentSpeedY + acelerationY);

                if ((Mathf.Abs(currentSpeedX) > Mathf.Abs(currentPosition.x > targetPosX ? currentPosition.x - targetPosX : targetPosX - currentPosition.x)) || currentPosition.x < originPosition.x - rangeFloating.x || currentPosition.x > originPosition.x + rangeFloating.x)
                {
                    targetPosX = Random.Range(-rangeFloating.x, rangeFloating.x) + originPosition.x;
                    acelerationX = targetPosX >= currentPosition.x ? aceleration : -aceleration;
                }

                if ((Mathf.Abs(currentSpeedY) > Mathf.Abs(currentPosition.y > targetPosY ? currentPosition.y - targetPosY : targetPosY - currentPosition.y)) || currentPosition.y < originPosition.y - rangeFloating.y || currentPosition.y > originPosition.y + rangeFloating.y)
                {
                    targetPosY = Random.Range(-rangeFloating.y, rangeFloating.y) + originPosition.y;
                    acelerationY = targetPosY >= currentPosition.y ? aceleration : -aceleration;
                }

                currentPosition.x += currentSpeedX;
                currentPosition.y += currentSpeedY;

                rectTransform.anchoredPosition = currentPosition;
            }
        }

        #region Public Function

        public void Reset()
        {
            currentSpeedX = 0;
            currentSpeedY = 0;
            currentPosition = originPosition;
            targetPosX = Random.Range(-rangeFloating.x, rangeFloating.x) + originPosition.x;
            acelerationX = targetPosX > currentPosition.x ? aceleration : -aceleration;
            targetPosY = Random.Range(-rangeFloating.y, rangeFloating.y) + originPosition.y;
            acelerationY = targetPosY > currentPosition.y ? aceleration : -aceleration;
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