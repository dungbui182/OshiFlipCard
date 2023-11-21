using UnityEngine;
using System.Collections;
using GrandDreams.Core.Utilities;
using System.Collections.Generic;
using GrandDreams.Core.RestNetwork;
using System.Linq;

namespace GrandDreams.Core.RestNetwork
{
    public class RestConfigManager : MonoBehaviour
    {
        #region Declare Variables

        public System.Action<RestConfigManager> OnConfigManager_Loaded = delegate { };

        private static RestConfigManager instance = null;
        public static RestConfigManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("RestConfigManager");
                    instance = go.AddComponent<RestConfigManager>();
                }
                else
                {
                    if (instance.gameObject.IsDestroyed())
                    {
                        instance = null;
                        return Instance;
                    }
                }

                return instance;
            }
        }


        public static bool IsLoaded { get; set; }

        private Dictionary<string, string> configData = new Dictionary<string, string>();

        #endregion Declare Variables

        private void OnDestroy()
        {
            instance = null;
            OnConfigManager_Loaded = delegate { };
        }

        public void DownloadConfig(string conditionType = null, string configConditions = null, System.Action<RestConfigManager> onDone = null)
        {
            configData = new Dictionary<string, string>();

            var data = new Dictionary<string, string>();
            data.Add("type", "select");
            data.Add("tableName", "config");
            data.Add("selectingFields", "clientData|serverData");
            
            if (!string.IsNullOrEmpty(conditionType))
            {
                data.Add("conditionType", conditionType);
            }

            if (!string.IsNullOrEmpty(configConditions))
            {
                data.Add("conditions", configConditions);
            }

            RestWrapper.Instance.Request(data, response =>
            {
                string[] responseData = response.Data.ToResponseText().Split('#');
                if (responseData[0] == "1")
                {
                    if (responseData.Length > 1 && responseData[1] != "")
                    {
                        string[] contentData = responseData[1].Split('^');

                        for (int indexConfig = 0; indexConfig < contentData.Length; indexConfig++)
                        {
                            string[] configDataArray = contentData[indexConfig].Split('|');

                            configData.Add(configDataArray[0], configDataArray[1]);
                        }
                    }
                }

                IsLoaded = true;
                if (onDone != null)
                {
                    onDone(this);
                }

                if(OnConfigManager_Loaded != null)
                {
                    OnConfigManager_Loaded(this);
                }
            }, exc =>
            {
#if UNITY_EDITOR
                Debug.Log(exc.Message);
#endif
            });
        }

        #region Public Function

        public T Get<T>(string key, System.Func<string, T> convertFunc = null)
        {
            if (!configData.ContainsKey(key))
            {
                return default(T);
            }

            if (convertFunc == null)
            {
                return (T)System.Convert.ChangeType(configData[key], typeof(T));
            }
            else
            {
                return convertFunc(configData[key]);
            }
        }

        public string Get(string key)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Trying to get key: " + key);
#endif
            if (!IsLoaded)
            {
                Debug.LogWarning("Config file hasn't been downloaded yet.");
                return null;
            }

            if (configData.ContainsKey(key))
            {
                return configData[key];
            }
            return null;
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