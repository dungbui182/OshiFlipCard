using UnityEngine;

namespace GrandDreams.Core.Utilities
{
    public static class GameObjectExtensions
    {
        public static bool IsDestroyed(this GameObject gameObject)
        {
            return gameObject == null && !ReferenceEquals(gameObject, null);
        }

        public static T CloneObject<T>(this object source)
        {
            T result = System.Activator.CreateInstance<T>();
            return result;
        }

        public static T AddComponentIfNotExist<T>(this GameObject gameObject) where T: UnityEngine.Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }

        public static void SetLayerRecursively(this GameObject gameObject, int idLayer)
        {
            if (null == gameObject)
            {
                return;
            }

            gameObject.layer = idLayer;

            foreach (Transform child in gameObject.transform)
            {
                if (null == child)
                {
                    continue;
                }
                SetLayerRecursively(child.gameObject, idLayer);
            }
        }
    }
}