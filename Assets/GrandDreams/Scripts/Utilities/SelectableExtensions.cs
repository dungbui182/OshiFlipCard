using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GrandDreams.Core.Utilities
{
    public static class SelectableExtensions
    {
        public static void AddEventTrigger(this GameObject gameObject, EventTriggerType triggerType, UnityAction<BaseEventData> action)
        {
            EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                eventTrigger = gameObject.AddComponent<EventTrigger>();
            }

            EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
            trigger.AddListener((eventData) => action(eventData));
            EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };
            eventTrigger.triggers.Add(entry);
        }

        public static void RemoveEventTrigger(this GameObject gameObject, EventTriggerType triggerType, UnityAction<BaseEventData> action)
        {
            EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                eventTrigger = gameObject.AddComponent<EventTrigger>();
            }

            EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
            trigger.AddListener((eventData) => action(eventData));
            EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };
            eventTrigger.triggers.Remove(entry);
        }
    }
}
