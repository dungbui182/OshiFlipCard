using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDreams.Core.Utilities
{
    public static class DropdownHelpers
    {
        public static void SetValue(this Dropdown dropdown, int value)
        {
            dropdown.StartCoroutine(SetValueRoutine(dropdown, value));
        }

        private static IEnumerator SetValueRoutine(this Dropdown dropdown, int value)
        {
            dropdown.Select();
            yield return new WaitForEndOfFrame();
            dropdown.value = value;
            dropdown.RefreshShownValue();
        }
    }
}