using DG.Tweening;
using GrandDreams.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDreams.Core.Kinect
{
    public enum ECursorType
    {
        None, InteractionBox, OverlayScreenPos
    }

    public enum EInteractionType
    {
        None, Rectangle, Ellipse
    }

    [System.Serializable]
    public class InteractivePoint
    {
        public int[] GameStates;
        public string Name;
        public EInteractionType Type;
        public Vector2 Position;
        public Vector2 Range;
    }

    [System.Serializable]
    public class KinectCursor
    {
        public int[] GameStates;
        public RectTransform RectTransform;
        public ECursorType CursorType;
        public KinectInterop.JointType JointType;
        public bool UseProgressCircle;
        public Image ImageProgress;

        public KinectCursor(KinectCursor kc)
        {
            this.GameStates = kc.GameStates;
            this.RectTransform = kc.RectTransform;
            this.CursorType = kc.CursorType;
            this.JointType = kc.JointType;
            this.UseProgressCircle = kc.UseProgressCircle;
            this.ImageProgress = kc.ImageProgress;
        }
    }

    public class BaseKinectCursorController : MonoBehaviour
    {
        #region Declare Variables

        public System.Action<KinectCursor, int, string> OnKinectCursor_Interacted = delegate { };

        [SerializeField] protected int playerIndex;
        [SerializeField] protected Vector2 screenSize;
        [SerializeField] protected KinectCursor[] cursors;
        [SerializeField] protected InteractivePoint[] interactivePoints;
        [SerializeField] protected float smoothFactor = 10;

        protected RectTransform rectTransform;

        protected KinectCursor[] savedCursors;

        protected Vector3[] cursorPositions;
        protected Vector3[] cursorScreenPositions;
        protected Vector3[] oldCursorScreenPositions;
        protected bool[] isCursorIboxValids;

        protected Vector3 refCursorIboxLeftBotBack;
        protected Vector3 refCursorIboxRightTopFront;

        protected int length, lengthOfInteractivePoint;

        protected KinectManager kinectManager;
        protected long playerUserID = 0;

        protected int currentGameState;

        protected Dictionary<int, Tween> dictionaryTweenProgresses = new Dictionary<int, Tween>();

        #endregion Declare Variables

        protected virtual void Awake()
        {
            rectTransform = transform as RectTransform;

            savedCursors = cursors.Select(x => new KinectCursor(x)).ToArray();

            length = cursors.Length;
            lengthOfInteractivePoint = interactivePoints.Length;

            if (screenSize == Vector2.zero)
            {
                screenSize = rectTransform.sizeDelta;
            }

            cursorPositions = new Vector3[length];
            cursorScreenPositions = new Vector3[length];
            oldCursorScreenPositions = new Vector3[length];

            isCursorIboxValids = new bool[length];
        }

        protected virtual IEnumerator Start()
        {
            Reset();
            yield return new WaitUntil(() => KinectManager.Instance != null);
            kinectManager = KinectManager.Instance;
        }

        protected virtual void OnDestroy()
        {
            OnKinectCursor_Interacted = delegate { };
        }

        protected virtual void Update()
        {
            if (kinectManager && kinectManager.IsInitialized())
            {
                playerUserID = kinectManager.GetUserIdByIndex(playerIndex);

                if (playerUserID != 0)
                {
                     for (int index = 0; index < length; index++)
                    {
                        if (cursors[index].CursorType == ECursorType.None || (!cursors[index].GameStates.Contains(-1) && !cursors[index].GameStates.Contains(currentGameState)))
                        {
                            continue;
                        }

                        int tmpIndex = index;

                        switch (cursors[tmpIndex].JointType)
                        {
                            case KinectInterop.JointType.HandLeft:
                            case KinectInterop.JointType.WristLeft:
                            case KinectInterop.JointType.ElbowLeft:
                            case KinectInterop.JointType.ShoulderLeft:
                                isCursorIboxValids[tmpIndex] = kinectManager.GetLeftHandInteractionBox(playerUserID, ref refCursorIboxLeftBotBack, ref refCursorIboxRightTopFront, isCursorIboxValids[tmpIndex]);
                                break;

                            case KinectInterop.JointType.HandRight:
                            case KinectInterop.JointType.WristRight:
                            case KinectInterop.JointType.ElbowRight:
                            case KinectInterop.JointType.ShoulderRight:
                                isCursorIboxValids[tmpIndex] = kinectManager.GetRightHandInteractionBox(playerUserID, ref refCursorIboxLeftBotBack, ref refCursorIboxRightTopFront, isCursorIboxValids[tmpIndex]);
                                break;
                        }

                        if (isCursorIboxValids[tmpIndex] && kinectManager.GetJointTrackingState(playerUserID, (int)cursors[tmpIndex].JointType) != KinectInterop.TrackingState.NotTracked)
                        {
                            cursors[tmpIndex].RectTransform.gameObject.SetActive(true);

                            cursorPositions[tmpIndex] = kinectManager.GetJointPosition(playerUserID, (int)cursors[tmpIndex].JointType);
                            cursorScreenPositions[tmpIndex].z = Mathf.Clamp01((refCursorIboxLeftBotBack.z - cursorPositions[tmpIndex].z) / (refCursorIboxLeftBotBack.z - refCursorIboxRightTopFront.z));

                            if (cursors[tmpIndex].CursorType == ECursorType.InteractionBox)
                            {
                                cursorScreenPositions[tmpIndex].x = Mathf.Clamp01((cursorPositions[tmpIndex].x - refCursorIboxLeftBotBack.x) / (refCursorIboxRightTopFront.x - refCursorIboxLeftBotBack.x)) * screenSize.x;
                                cursorScreenPositions[tmpIndex].y = Mathf.Clamp01((cursorPositions[tmpIndex].y - refCursorIboxLeftBotBack.y) / (refCursorIboxRightTopFront.y - refCursorIboxLeftBotBack.y)) * screenSize.y;
                            }
                            else if (cursors[tmpIndex].CursorType == ECursorType.OverlayScreenPos)
                            {
                                GetCursorOverlayScreenPos(kinectManager, (int)cursors[tmpIndex].JointType, ref cursorScreenPositions[tmpIndex]);
                            }

                            if (oldCursorScreenPositions[tmpIndex] != Vector3.zero)
                            {
                                cursors[tmpIndex].RectTransform.anchoredPosition = oldCursorScreenPositions[tmpIndex] + (cursorScreenPositions[tmpIndex] - oldCursorScreenPositions[tmpIndex]) / smoothFactor;
                            }
                            else
                            {
                                cursors[tmpIndex].RectTransform.anchoredPosition = cursorScreenPositions[tmpIndex];
                            }

                            oldCursorScreenPositions[tmpIndex] = cursors[tmpIndex].RectTransform.anchoredPosition;

                            if (cursors[tmpIndex].UseProgressCircle)
                            {
                                for (int indexInteractivePoint = 0; indexInteractivePoint < lengthOfInteractivePoint; indexInteractivePoint++)
                                {
                                    if (!interactivePoints[indexInteractivePoint].GameStates.Contains(-1) && !interactivePoints[indexInteractivePoint].GameStates.Contains(currentGameState))
                                    {
                                        continue;
                                    }

                                    int tmpIndexInteractivePoint = indexInteractivePoint;
                                    int idTween = tmpIndex * 1000 + tmpIndexInteractivePoint;

                                    if (interactivePoints[tmpIndexInteractivePoint].Type == EInteractionType.Rectangle)
                                    {
                                        if (cursors[tmpIndex].RectTransform.anchoredPosition.IsInsideRectangle(interactivePoints[tmpIndexInteractivePoint].Position, interactivePoints[tmpIndexInteractivePoint].Range))
                                        {
                                            if(!dictionaryTweenProgresses.ContainsKey(idTween))
                                            {
                                                dictionaryTweenProgresses.Add(idTween, null);
                                            }
                                            if (dictionaryTweenProgresses[idTween] == null)
                                            {
                                                cursors[tmpIndex].ImageProgress.fillAmount = 0;

                                                dictionaryTweenProgresses[idTween] = cursors[tmpIndex].ImageProgress.DOFillAmount(1, 1).SetEase(Ease.Linear).OnComplete(() =>
                                                {
                                                    dictionaryTweenProgresses[idTween] = null;
                                                    cursors[tmpIndex].ImageProgress.fillAmount = 0;

                                                    if (OnKinectCursor_Interacted != null)
                                                    {
                                                        OnKinectCursor_Interacted(cursors[tmpIndex], currentGameState, interactivePoints[tmpIndexInteractivePoint].Name);
                                                    }

#if UNITY_EDITOR
                                                    Debug.Log(interactivePoints[tmpIndexInteractivePoint].Name);
#endif
                                                });
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            if (!dictionaryTweenProgresses.ContainsKey(idTween))
                                            {
                                                dictionaryTweenProgresses.Add(idTween, null);
                                            }
                                            if (dictionaryTweenProgresses[idTween] != null)
                                            {
                                                dictionaryTweenProgresses[idTween].Kill();
                                                dictionaryTweenProgresses[idTween] = null;

                                                cursors[tmpIndex].ImageProgress.fillAmount = 0;
                                            }
                                        }
                                    }
                                    else if (interactivePoints[tmpIndexInteractivePoint].Type == EInteractionType.Ellipse)
                                    {
                                        if (cursors[tmpIndex].RectTransform.anchoredPosition.IsInsideEllipse(interactivePoints[tmpIndexInteractivePoint].Position, interactivePoints[tmpIndexInteractivePoint].Range))
                                        {
                                            if (!dictionaryTweenProgresses.ContainsKey(idTween))
                                            {
                                                dictionaryTweenProgresses.Add(idTween, null);
                                            }
                                            if (dictionaryTweenProgresses[idTween] == null)
                                            {
                                                cursors[tmpIndex].ImageProgress.fillAmount = 0;

                                                dictionaryTweenProgresses[idTween] = cursors[tmpIndex].ImageProgress.DOFillAmount(1, 1).SetEase(Ease.Linear).OnComplete(() =>
                                                {
                                                    dictionaryTweenProgresses[idTween] = null;

                                                    if (OnKinectCursor_Interacted != null)
                                                    {
                                                        OnKinectCursor_Interacted(cursors[tmpIndex], currentGameState, interactivePoints[tmpIndexInteractivePoint].Name);
                                                    }

#if UNITY_EDITOR
                                                    Debug.Log(interactivePoints[tmpIndexInteractivePoint].Name);
#endif
                                                });
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            if (!dictionaryTweenProgresses.ContainsKey(idTween))
                                            {
                                                dictionaryTweenProgresses.Add(idTween, null);
                                            }

                                            if (dictionaryTweenProgresses[idTween] != null)
                                            {
                                                dictionaryTweenProgresses[idTween].Kill();
                                                dictionaryTweenProgresses[idTween] = null;

                                                cursors[tmpIndex].ImageProgress.fillAmount = 0;
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            cursors[tmpIndex].RectTransform.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        #region Public Function

        public virtual void Reset()
        {
            for (int index = 0; index < length; index++)
            {
                cursors[index].RectTransform.gameObject.SetActive(false);

                cursorPositions[index] = Vector3.zero;
                cursorScreenPositions[index] = Vector3.zero;
                oldCursorScreenPositions[index] = Vector3.zero;
            }

            for (int index = 0; index < length; index++)
            {
                if (cursors[index].UseProgressCircle)
                {
                    cursors[index].ImageProgress.fillAmount = 0;
                }
            }
        }

        public virtual void SetGameState(int gameState)
        {
            currentGameState = gameState;
            for (int index = 0; index < cursors.Length; index++)
            {
                if (!cursors[index].GameStates.Contains(currentGameState))
                {
                    cursors[index].UseProgressCircle = savedCursors[index].UseProgressCircle;
                    cursors[index].RectTransform.gameObject.SetActive(false);
                }
            }
        }

        public virtual KinectCursor GetCursor(int idCursor)
        {
            return cursors[idCursor];
        }

        public virtual InteractivePoint GetInteractivePoint(int idInteractivePoint)
        {
            return interactivePoints[idInteractivePoint];
        }

        #endregion Public Function

        #region Private Function

        protected virtual bool GetCursorOverlayScreenPos(KinectManager kinectManager, int iHandJointIndex, ref Vector3 handScreenPos)
        {
            Vector3 posJointRaw = kinectManager.GetJointKinectPosition(playerUserID, iHandJointIndex);

            if (posJointRaw != Vector3.zero)
            {
                Vector2 posDepth = kinectManager.MapSpacePointToDepthCoords(posJointRaw);
                ushort depthValue = kinectManager.GetDepthForPixel((int)posDepth.x, (int)posDepth.y);

                if (posDepth != Vector2.zero && depthValue > 0)
                {
                    // depth pos to color pos
                    Vector2 posColor = kinectManager.MapDepthPointToColorCoords(posDepth, depthValue);

                    if (!float.IsInfinity(posColor.x) && !float.IsInfinity(posColor.y))
                    {
                        // get the color image x-offset and width (use the portrait background, if available)
                        float colorWidth = kinectManager.GetColorImageWidth();
                        float colorOfsX = 0f;

                        PortraitBackground portraitBack = PortraitBackground.Instance;
                        if (portraitBack && portraitBack.enabled)
                        {
                            colorWidth = kinectManager.GetColorImageHeight() * kinectManager.GetColorImageHeight() / kinectManager.GetColorImageWidth();
                            colorOfsX = (kinectManager.GetColorImageWidth() - colorWidth) / 2f;
                        }

                        float xScaled = (posColor.x - colorOfsX) / colorWidth;
                        float yScaled = posColor.y / kinectManager.GetColorImageHeight();

                        handScreenPos.x = xScaled * screenSize.x;
                        handScreenPos.y = (1f - yScaled) * screenSize.y;

                        return true;
                    }
                }
            }

            return false;
        }

        #endregion Private Function

        #region Event



        #endregion Event

        #region Editor



        #endregion Editor
    }
}