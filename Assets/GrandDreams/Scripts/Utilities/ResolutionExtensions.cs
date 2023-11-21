using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrandDreams.Core.Utilities
{
    public static class ResolutionExtensions
    {
        public static bool EqualsTo(this Resolution currResolution, Resolution targetResolution)
        {
            if(currResolution.width == targetResolution.width && currResolution.height == targetResolution.height)
            {
                return true;
            }
            return false;
        }
    }
}