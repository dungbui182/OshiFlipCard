using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace GrandDreams.Core.Utilities
{
    public static class ImageExtensions
    {
        public static ushort GetImageDataAtLocation(this byte[] buffer, int offset)
        {
            return System.BitConverter.ToUInt16(buffer, offset);
        }
    }
}
