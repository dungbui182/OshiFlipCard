using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrandDreams.Core.Kinect
{
    public class BaseGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
    {
        #region Declare Variables

        public System.Action<KinectGestures.Gestures> OnGestureListener_Excuted = delegate { };

        protected static BaseGestureListener instance = null;
        public static BaseGestureListener Instance
        {
            get
            {
                return instance;
            }
        }

        public static bool AllowExcute { get; set; }

        [SerializeField] protected int playerIndex = 0;
        [SerializeField] protected KinectGestures.Gestures[] detectGestures;

        protected int length;

        #endregion Declare Variables

        protected void Awake()
        {
            instance = this;

            length = detectGestures.Length;
        }

        protected void OnDestroy()
        {
            OnGestureListener_Excuted = delegate { };
        }

        #region Public Function



        #endregion Public Function

        #region Private Function



        #endregion Private Function

        #region Event

        public virtual void UserDetected(long userId, int userIndex)
        {
            KinectManager manager = KinectManager.Instance;
            if (!manager || (userIndex != playerIndex))
                return;

            for (int index = 0; index < length; index++)
            {
                manager.DetectGesture(userId, detectGestures[index]);
            }
        }

        public virtual void UserLost(long userId, int userIndex)
        {
            KinectManager manager = KinectManager.Instance;
            if (userIndex != playerIndex)
                return;

            for (int index = 0; index < length; index++)
            {
                manager.DeleteGesture(userId, detectGestures[index]);
            }
        }

        public virtual void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture, float progress, KinectInterop.JointType joint, Vector3 screenPos)
        {
            if (!AllowExcute)
            {
                return;
            }
        }

        public virtual bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture, KinectInterop.JointType joint, Vector3 screenPos)
        {
            if (!AllowExcute)
            {
                return true;
            }

            KinectManager manager = KinectManager.Instance;
            if (userIndex != playerIndex)
                return false;

            for (int index = 0; index < length; index++)
            {
                if (gesture == detectGestures[index])
                {
                    if(OnGestureListener_Excuted != null)
                    {
                        OnGestureListener_Excuted(gesture);
#if UNITY_EDITOR
                        Debug.Log(gesture.ToString());
#endif
                    }
                }
            }

            return true;
        }

        public virtual bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture, KinectInterop.JointType joint)
        {
            if (!AllowExcute)
            {
                return false;
            }

            if (userIndex != playerIndex)
                return false;
            return true;
        }

#endregion Event

#region Editor



#endregion Editor

    }
}