using System;
using System.Collections.Generic;
using UnityEngine;

namespace TonyLearning.ShootingGame.Pool_System
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField] Pool[] enemyPools;
        [SerializeField] Pool[] playerProjectilePools;
        [SerializeField] private Pool[] enemyProjectilePools;
        [SerializeField] private Pool[] vfxPools;

        static Dictionary<GameObject, Pool> _dictionary;
        
        private void Awake()
        {
            _dictionary = new Dictionary<GameObject, Pool>();
            
            Initialize(enemyPools);
            Initialize(playerProjectilePools);
            Initialize(enemyProjectilePools);
            Initialize(vfxPools);
        }

        void Initialize(Pool[] pools)
        {
            foreach (var pool in pools)
            {
#if UNITY_EDITOR
                
                if (_dictionary.ContainsKey(pool.Prefab))
                {
                    Debug.LogError("Same prefab in multiple pools! Prefab: " + pool.Prefab.name);
                    continue;
                }
#endif
                _dictionary.Add(pool.Prefab, pool);
                
                Transform poolParent =  new GameObject("Pool: " + pool.Prefab.name).transform;

                poolParent.parent = transform;
                pool.Initialize(poolParent);
            }
            
        }

#if UNITY_EDITOR
        
        private void OnDestroy()
        {
            CheckPoolSize(enemyPools);
            CheckPoolSize(playerProjectilePools);
            CheckPoolSize(enemyProjectilePools);
            CheckPoolSize(vfxPools);
        }
#endif

        void CheckPoolSize(Pool[] pools)
        {
            foreach (var pool in pools)
            {
                if (pool.RuntimeSize > pool.Size)
                {
                    Debug.LogWarning(
                        string.Format("Pool: {0} has a runtime size {1} bigger than its initial size {2}!",
                            pool.Prefab.name,
                            pool.RuntimeSize,
                            pool.Size));
                }
            }
        }
        

        /// <summary>
        /// <para>Return a specified<paramref name="prefab"/>gameObject in the pool.</para>
        /// </summary>
        /// <param name="prefab">
        ///<para>Specified gameObject prefab.</para>
        /// </param>
        /// <returns>
        /// <para>Prepared gameObject in the pool.</para>
        /// </returns>
        public static GameObject Release(GameObject prefab)
        {
#if UNITY_EDITOR
            
            if (!_dictionary.ContainsKey(prefab))
            {
                Debug.LogError("Pool Manager could NOT find prefab: "+ prefab.name);
                return null;
            }
#endif
            return _dictionary[prefab].PreparedObject();
        }
        
        /// <summary>
        /// <para>Release a specified prepared gameObject in the pool at specified position.</para>
        /// <para>根据传入的prefab参数，在position参数位置释放对象池中预备好的游戏对象。</para> 
        /// </summary>
        /// <param name="prefab">
        /// <para>Specified gameObject prefab.</para>
        /// <para>指定的游戏对象预制体。</para>
        /// </param>
        /// <param name="position">
        /// <para>Specified release position.</para>
        /// <para>指定释放位置。</para>
        /// </param>
        /// <returns></returns>
        public static GameObject Release(GameObject prefab, Vector3 position)
        {
#if UNITY_EDITOR
            
            if (!_dictionary.ContainsKey(prefab))
            {
                Debug.LogError("Pool Manager could NOT find prefab: "+ prefab.name);
                return null;
            }
#endif
            return _dictionary[prefab].PreparedObject(position);
        }
        
        /// <summary>
        /// <para>Release a specified prepared gameObject in the pool at specified position and rotation.</para>
        /// <para>根据传入的prefab参数和rotation参数，在position参数位置释放对象池中预备好的游戏对象。</para> 
        /// </summary>
        /// <param name="prefab">
        /// <para>Specified gameObject prefab.</para>
        /// <para>指定的游戏对象预制体。</para>
        /// </param>
        /// <param name="position">
        /// <para>Specified release position.</para>
        /// <para>指定释放位置。</para>
        /// </param>
        /// <param name="rotation">
        /// <para>Specified rotation.</para>
        /// <para>指定的旋转值。</para>
        /// </param>
        /// <returns></returns>
        public static GameObject Release(GameObject prefab, Vector3 position,Quaternion rotation)
        {
#if UNITY_EDITOR
            
            if (!_dictionary.ContainsKey(prefab))
            {
                Debug.LogError("Pool Manager could NOT find prefab: "+ prefab.name);
                return null;
            }
#endif
            return _dictionary[prefab].PreparedObject(position, rotation);
        }
        
        /// <summary>
        /// <para>Release a specified prepared gameObject in the pool at specified position, rotation and scale.</para>
        /// <para>根据传入的prefab参数, rotation参数和localScale参数，在position参数位置释放对象池中预备好的游戏对象。</para> 
        /// </summary>
        /// <param name="prefab">
        /// <para>Specified gameObject prefab.</para>
        /// <para>指定的游戏对象预制体。</para>
        /// </param>
        /// <param name="position">
        /// <para>Specified release position.</para>
        /// <para>指定释放位置。</para>
        /// </param>
        /// <param name="rotation">
        /// <para>Specified rotation.</para>
        /// <para>指定的旋转值。</para>
        /// </param>
        /// <param name="localScale">
        /// <para>Specified scale.</para>
        /// <para>指定的缩放值。</para>
        /// </param>
        /// <returns></returns>
        public static GameObject Release(GameObject prefab, Vector3 position,Quaternion rotation,Vector3 localScale)
        {
#if UNITY_EDITOR
            
            if (!_dictionary.ContainsKey(prefab))
            {
                Debug.LogError("Pool Manager could NOT find prefab: "+ prefab.name);
                return null;
            }
#endif
            return _dictionary[prefab].PreparedObject(position, rotation, localScale);
        }

    }
}
