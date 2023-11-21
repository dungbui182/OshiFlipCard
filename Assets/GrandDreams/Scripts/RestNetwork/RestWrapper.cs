using GrandDreams.Core.Utilities;
using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace GrandDreams.Core.RestNetwork
{
    public class RestWrapper : MonoBehaviour
    {

        #region Declare Variables

        private static RestWrapper instance = null;
        public static RestWrapper Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("RestNetwork");
                    instance = go.AddComponent<RestWrapper>();
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

        public System.Action<System.Exception> OnRestWrapper_DefaultRequestFailed = delegate { };

        public string DefaultUrl = "";
        public string DefaultMethod = "POST";
        public int DefaultTimeOut = 10;

        private RequestHelper currentRequest;

        #endregion Declare Variables

        private void OnDestroy()
        {
            OnRestWrapper_DefaultRequestFailed = delegate { };
        }

        #region Public Function

        public void Initialize(string url, string method, int timeOut)
        {
            DefaultUrl = url;
            DefaultMethod = method;
            DefaultTimeOut = timeOut;
        }

        public void CreateTable(string tableName, char splitChar = ';', System.Action onDone = null)
        {
            var data = new Dictionary<string, string>();
            data.Add("type", "createTable");
            data.Add("tableName", tableName);

            Request(data, response =>
            {
                string[] responseData = response.Data.ToResponseText().Split(splitChar);
                if (responseData[0] == "1")
                {
                    Debug.Log("Table " + tableName + " created.");
                }
                else
                {
                    Debug.Log("Table " + tableName + " existed.");
                }

                if(onDone != null)
                {
                    onDone();
                }
            }, exc =>
            {
#if UNITY_EDITOR
                Debug.Log(exc.Message);
#endif
            });
        }

        public void RequestUrl(string url, string method = "", int timeOut = -1, System.Action<ResponseHelper> onSucceed = null, System.Action<System.Exception> onFailed = null)
        {
            currentRequest = new RequestHelper
            {
                Uri = url,
                Method = method == "" ? DefaultMethod : method,
                Timeout = timeOut == -1 ? DefaultTimeOut : timeOut,
            };

            RestClient.Request(currentRequest).Then(response =>
            {
#if UNITY_EDITOR
                Debug.Log(response.Data.ToResponseText());
#endif

                if (onSucceed != null)
                {
                    onSucceed(response);
                }
            }).Catch(exception =>
            {
                if (onFailed != null)
                {
                    onFailed(exception);
                }
                else
                {
                    if(OnRestWrapper_DefaultRequestFailed != null)
                    {
                        OnRestWrapper_DefaultRequestFailed(exception);
                    }
                }
            }).Finally(() =>
            {
                currentRequest = null;
            });
        }

        public void Request(Dictionary<string, string> fields, System.Action<ResponseHelper> onSucceed, System.Action<System.Exception> onFailed = null)
        {
            currentRequest = new RequestHelper
            {
                Uri = DefaultUrl,
                Method = DefaultMethod,
                Timeout = DefaultTimeOut,
                Params = fields,
            };

            RestClient.Request(currentRequest).Then(response =>
            {
#if UNITY_EDITOR
                Debug.Log(response.Data.ToResponseText());
#endif

                if (onSucceed != null)
                {
                    onSucceed(response);
                }
            }).Catch(exception =>
            {
                if (onFailed != null)
                {
                    onFailed(exception);
                }
                else
                {
                    if (OnRestWrapper_DefaultRequestFailed != null)
                    {
                        OnRestWrapper_DefaultRequestFailed(exception);
                    }
                }
            }).Finally(() =>
            {
                currentRequest = null;
            });
        }

        public void Request(Dictionary<string, string> fields, Dictionary<string, byte[]> binaryData, System.Action<ResponseHelper> onSucceed, System.Action<System.Exception> onFailed = null)
        {
            RestClient.DefaultRequestParams["Content-Type"] = "application/x-wwwform-urlencoded";
            //RestClient.DefaultRequestParams["Content-Type"] = "image/jpeg";

            WWWForm formData = new WWWForm();

            if (fields != null && fields.Count > 0)
            {
                foreach (var keyValuePair in fields)
                {
                    formData.AddField(keyValuePair.Key, keyValuePair.Value);
                }
            }

            if (binaryData != null && binaryData.Count > 0)
            {
                foreach (var keyValuePair in binaryData)
                {
                    formData.AddBinaryData(keyValuePair.Key, keyValuePair.Value);
                }
            }

            currentRequest = new RequestHelper
            {
                Uri = DefaultUrl,
                Method = DefaultMethod,
                Timeout = DefaultTimeOut,
                FormData = formData
            };

            RestClient.Request(currentRequest).Then(response =>
            {
                if (onSucceed != null)
                {
                    onSucceed(response);
                }
            }).Catch(exception =>
            {
                if (onFailed != null)
                {
                    onFailed(exception);
                }
                else
                {
                    if (OnRestWrapper_DefaultRequestFailed != null)
                    {
                        OnRestWrapper_DefaultRequestFailed(exception);
                    }
                }
            }).Finally(() =>
            {
                RestClient.ClearDefaultHeaders();
                currentRequest = null;
            });
        }

        public void AbortRequest()
        {
            if (currentRequest != null)
            {
                currentRequest.Abort();
                currentRequest = null;
            }
        }

        public void CheckConnection(string uri, System.Action onConnected = null, System.Action onFailed = null)
        {
            string HtmlText = GetHtmlFromUri(uri);
            if (HtmlText == "")
            {
                if(onFailed != null)
                {
                    onFailed();
                }
            }
            else if (!HtmlText.Contains("schema.org/WebPage"))
            {
                //Redirecting since the beginning of googles html contains that 
                //phrase and it was not found
            }
            else
            {
                if (onConnected != null)
                {
                    onConnected();
                }
            }
        }

        public string GetHtmlFromUri(string uri)
        {
            string html = string.Empty;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
            try
            {
                using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                {
                    bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                    if (isSuccess)
                    {
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(resp.GetResponseStream()))
                        {
                            char[] cs = new char[80];
                            reader.Read(cs, 0, cs.Length);
                            foreach (char ch in cs)
                            {
                                html += ch;
                            }
                        }
                    }
                }
            }
            catch
            {
                return "";
            }
            return html;
        }

        #endregion Public Function
    }
}