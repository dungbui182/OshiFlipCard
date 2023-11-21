using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GrandDreams.Core
{
    public static class RequestHelpers
    {

        public static Dictionary<string, string> AddNonEmptyData(this Dictionary<string, string> requestData, string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                requestData.Add(key, value);
            }
            return requestData;
        }

    }
}
