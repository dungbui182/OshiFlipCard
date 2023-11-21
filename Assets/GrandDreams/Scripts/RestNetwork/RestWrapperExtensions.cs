using UnityEngine;
using UnityEditor;

namespace GrandDreams.Core.RestNetwork
{
    public static class RestWrapperExtensions
    {
        public static string ToResponseText(this byte[] data)
        {
            string result = System.Text.Encoding.UTF8.GetString(data, 0, data.Length);
            return result;
        }
    }
}