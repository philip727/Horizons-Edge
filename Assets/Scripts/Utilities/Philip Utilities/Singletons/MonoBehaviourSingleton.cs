using UnityEngine;


namespace Philip.Utilities
{
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance { private set; get; }
        public virtual void Awake()
        {
            Instance = this as T;
        }

    }
}
