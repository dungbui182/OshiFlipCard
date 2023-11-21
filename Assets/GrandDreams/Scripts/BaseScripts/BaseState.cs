using GrandDreams.Core.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GrandDreams.Core
{
    public class BaseState : MonoBehaviour, IBaseState
    {

        #region Declare Variables

        private static BaseState instance = null;
        public static BaseState Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("BaseState");
                    instance = go.AddComponent<BaseState>();
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

        protected int idState;
        public int IDState
        {
            get
            {
                return idState;
            }
            set
            {
                idState = value;
                if (MapStateNameToID.ContainsValue(idState))
                {
                    currentState = MapStateNameToID.Where(x => x.Value == idState).FirstOrDefault().Key;
                }
                else
                {
                    Debug.LogWarning("Không tồn tại id state: '" + idState + "'");
                    MapStateNameToID.Add(idState.ToString(), idState);
                }

                if (DictionaryAction.ContainsKey(idState))
                {
                    for (int index = 0; index < DictionaryAction[idState].Count; index++)
                    {
                        DictionaryAction[idState][index]();
                    }
                }
                else
                {
                    DictionaryAction.Add(idState, new List<Action>());
                }

                for (int index = 0; index < ActionAllStates.Count; index++)
                {
                    ActionAllStates[index]();
                }
            }
        }

        protected string currentState = "";
        public string CurrentState
        {
            get
            {
                return currentState;
            }
            set
            {
                currentState = value;
                if (MapStateNameToID.ContainsKey(currentState))
                {
                    idState = MapStateNameToID[currentState];
                }
                else
                {
                    Debug.LogWarning("Không tồn tại state: '" + currentState + "'");

                    idState = MapStateNameToID.Count == 0 ? 0 : MapStateNameToID.Max(x => x.Value) + 1;
                    MapStateNameToID.Add(currentState, idState);
                }

                if (DictionaryAction.ContainsKey(idState))
                {
                    for (int index = 0; index < DictionaryAction[idState].Count; index++)
                    {
                        DictionaryAction[idState][index]();
                    }
                }
                else
                {
                    DictionaryAction.Add(idState, new List<Action>());
                }

                for (int index = 0; index < ActionAllStates.Count; index++)
                {
                    ActionAllStates[index]();
                }
            }
        }

        protected Dictionary<int, List<Action>> dictionaryAction = new Dictionary<int, List<Action>>();
        public Dictionary<int, List<Action>> DictionaryAction
        {
            get
            {
                return dictionaryAction;
            }
            set
            {
                dictionaryAction = value;
            }
        }

        protected Dictionary<string, int> mapStateNameToID = new Dictionary<string, int>();
        public Dictionary<string, int> MapStateNameToID
        {
            get
            {
                return mapStateNameToID;
            }
            set
            {
                mapStateNameToID = value;
            }
        }

        protected List<Action> actionAllStates = new List<Action>();
        public List<Action> ActionAllStates
        {
            get
            {
                return actionAllStates;
            }
            set
            {
                actionAllStates = value;
            }
        }

        #endregion Declare Variables

        #region Public Function

        public string GetStateByID(int idState)
        {
            if(!MapStateNameToID.ContainsValue(idState))
            {
                return "";
            }

            return MapStateNameToID.Where(x => x.Value == idState).FirstOrDefault().Key;
        }

        public int GetIDByState(string state)
        {
            if (!MapStateNameToID.ContainsKey(state))
            {
                return -1;
            }

            return MapStateNameToID[state];
        }

        public void AddActionState(int idState, Action action)
        {
            if (!MapStateNameToID.ContainsValue(idState))
            {
                MapStateNameToID.Add(idState.ToString(), idState);
            }

            if (DictionaryAction.ContainsKey(idState))
            {
                DictionaryAction[idState].Add(action);
            }
            else
            {
                DictionaryAction.Add(idState, new List<Action>() { action });
            }
        }

        public void AddActionState(string state, Action action)
        {
            int idState = -1;
            if (!MapStateNameToID.ContainsKey(state))
            {
                idState = MapStateNameToID.Count == 0 ? 0 : MapStateNameToID.Max(x => x.Value) + 1;

                MapStateNameToID.Add(state, idState);
            }
            else
            {
                idState = MapStateNameToID[state];
            }

            if (DictionaryAction.ContainsKey(idState))
            {
                DictionaryAction[idState].Add(action);
            }
            else
            {
                DictionaryAction.Add(idState, new List<Action>() { action });
            }
        }

        public void AddActionToAllState(Action action)
        {
            ActionAllStates.Add(action);
        }

        public void AddState(params string[] states)
        {
            foreach (var state in states)
            {
                if (!MapStateNameToID.ContainsKey(state))
                {
                    int idState = MapStateNameToID.Count == 0 ? 0 : MapStateNameToID.Max(x => x.Value) + 1;

                    MapStateNameToID.Add(state, idState);

                    if (!DictionaryAction.ContainsKey(idState))
                    {
                        DictionaryAction.Add(idState, new List<Action>());
                    }
                }
            }
        }

        #endregion Public Function

    }
}