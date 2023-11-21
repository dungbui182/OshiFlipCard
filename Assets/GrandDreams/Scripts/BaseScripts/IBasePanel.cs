using UnityEngine;
using System.Collections;

namespace GrandDreams.Core
{
    public interface IBasePanel
    {
        bool IsPanelChanged { get; set; }
        bool IsShowing { get; set; }

        System.Action<System.Action> ShowConfirmMessage { get; set; }

        void Initialize(object[] parameters);
        void Reset();
        void Show(object[] parameters = null, float delayedTime = 0, System.Action onAwake = null, System.Action onStart = null, System.Action onDone = null);
        void Hide(object[] parameters = null, float delayedTime = 0, System.Action onAwake = null, System.Action onStart = null, System.Action onDone = null);

        void InputKeyDown(string keyString);

        void InputKeyUp(string keyString);

    }
}