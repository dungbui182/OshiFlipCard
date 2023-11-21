using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrandDreams.Core.Utilities
{
    public static class LocationHelpers
    {
        private static IEnumerator ieRoutine;

        public static void DetermineLocation(this MonoBehaviour behaviour, System.Action<decimal, decimal> onSucceed = null, System.Action onLocationServiceDisabled = null, System.Action onRequestTimedOut = null, System.Action onUnableDetermineLocation = null)
        {
            if(ieRoutine != null)
            {
                behaviour.StopCoroutine(ieRoutine);
                ieRoutine = null;
            }

            ieRoutine = DetermineLocationRoutine(onSucceed, onLocationServiceDisabled, onRequestTimedOut, onUnableDetermineLocation);

            behaviour.StartCoroutine(ieRoutine);
        }

        public static void StopDetermineLocation(this MonoBehaviour behaviour)
        {
            if (ieRoutine != null)
            {
                behaviour.StopCoroutine(ieRoutine);
                ieRoutine = null;
            }
        }

        private static IEnumerator DetermineLocationRoutine(System.Action<decimal, decimal> onSucceed = null, System.Action onLocationServiceDisabled = null, System.Action onRequestTimedOut = null, System.Action onUnableDetermineLocation = null)
        {
            // First, check if user has location service enabled
            if (!Input.location.isEnabledByUser)
            {
                if (onLocationServiceDisabled != null)
                {
                    onLocationServiceDisabled();
                }

                yield break;
            }

            // Start service before querying location
            Input.location.Start();

            // Wait until service initializes
            int maxWait = 100;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(0.1f);
                maxWait--;
            }

            // Service didn't initialize in 20 seconds
            if (maxWait < 1)
            {
                if (onRequestTimedOut != null)
                {
                    onRequestTimedOut();
                }

                yield break;
            }

            // Connection has failed
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                if (onUnableDetermineLocation != null)
                {
                    onUnableDetermineLocation();
                }

                yield break;
            }
            else
            {
                // Access granted and location value could be retrieved
                decimal lat = (decimal)Input.location.lastData.latitude;
                decimal lng = (decimal)Input.location.lastData.longitude;
                if (onSucceed != null)
                {
                    onSucceed(lat, lng);
                }
            }

            // Stop service if there is no need to query location updates continuously
            Input.location.Stop();
        }
    }
}