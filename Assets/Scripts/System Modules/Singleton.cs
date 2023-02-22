using UnityEngine;

namespace TonyLearning.ShootingGame.System_Modules
{
    public class Singleton<T> : MonoBehaviour where T: Component
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            Instance = this as T;
        }
    }
}
