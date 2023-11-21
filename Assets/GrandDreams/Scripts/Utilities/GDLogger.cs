using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GrandDreams.Core.Utilities
{
    public class GDLogger : MonoBehaviour
    {

        public static GDLogger SharedInstance { get; private set; }

        private void Awake()
        {
            SharedInstance = this;
        }

        #region Public Function 

        public void Log(string content)
        {
            if(!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.streamingAssetsPath + "\\Log.txt", true))
            {
                file.WriteLine("(" + System.DateTime.Now.ToLongTimeString() + ") =======INFO=======: " + content);
            }
        }

        public void LogWarning(string content)
        {
            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.streamingAssetsPath + "\\Log.txt", true))
            {
                file.WriteLine("(" + System.DateTime.Now.ToLongTimeString() + ") =======Warning=======: " + content);
            }
        }

        public void LogError(string content)
        {
            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.streamingAssetsPath + "\\Log.txt", true))
            {
                file.WriteLine("(" + System.DateTime.Now.ToLongTimeString() + ") =======ERROR=======: " + content);
            }
        }

        #endregion Public Function 

    }
}
