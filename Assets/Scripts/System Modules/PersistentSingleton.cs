using System;
using UnityEngine;

namespace TonyLearning.ShootingGame.System_Modules
{
    public class PersistentSingleton<T> : MonoBehaviour where T: Component
    {
        public static T Instance { get; private set; }

        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }else if (Instance != this)
            {
                Destroy(gameObject);
            }
            
            DontDestroyOnLoad(gameObject);
        }
    }
}
