using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrandDreams.Core
{
    public interface IBaseState
    {

        int IDState { get; set; }
        string CurrentState { get; set; }

        Dictionary<int, List<System.Action>> DictionaryAction { get; set; }
        Dictionary<string, int> MapStateNameToID { get; set; }
        List<System.Action> ActionAllStates { get; set; }

        string GetStateByID(int idState);
        int GetIDByState(string state);

        void AddActionState(int idState, System.Action action);
        void AddActionState(string state, System.Action action);
        void AddActionToAllState(System.Action action);
        void AddState(params string[] state);
    }
}