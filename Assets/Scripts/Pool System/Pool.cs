using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

namespace TonyLearning.ShootingGame.Pool_System
{
    [System.Serializable]
    public class Pool
    {
        // public GameObject Prefab
        // {
        //     get
        //     {
        //         return prefab;
        //     }
        // }

        // public GameObject Prefab
        // {
        //     get => prefab;
        // }

        public GameObject Prefab => prefab;

        public int Size => size;

        public int RuntimeSize => _queue.Count;
        
        [SerializeField]private GameObject prefab;
        [SerializeField] private int size = 1;
        
        private Queue<GameObject> _queue;

        private Transform parent;

        public void Initialize(Transform parent)
        {
            _queue = new Queue<GameObject>();
            this.parent = parent;
            for (int i = 0; i < size; i++)
            {
                _queue.Enqueue(Copy());
            }
        }

        public GameObject Copy()
        {
            var copy = GameObject.Instantiate(prefab, parent);
            copy.SetActive(false);
            return copy;
        }

        public GameObject AvailableObject()
        {
            GameObject availableObject = null;

            if (_queue.Count > 0 && !_queue.Peek().activeSelf)
            {
                availableObject = _queue.Dequeue();
            }
            else
            {
                availableObject = Copy();
            }

            _queue.Enqueue(availableObject);
            
            return availableObject;

        }

        public GameObject PreparedObject()
        {
            GameObject prepareObject = AvailableObject();
            prepareObject.SetActive(true);
            return prepareObject;
        }

        public GameObject PreparedObject(Vector3 position)
        {
            GameObject prepareObject = AvailableObject();
            prepareObject.SetActive(true);
            prepareObject.transform.position = position;
            return prepareObject;
        }
        
        public GameObject PreparedObject(Vector3 position, Quaternion rotation)
        {
            GameObject prepareObject = AvailableObject();
            prepareObject.SetActive(true);
            prepareObject.transform.position = position;
            prepareObject.transform.rotation = rotation;
            return prepareObject;
        }
        
        public GameObject PreparedObject(Vector3 position, Quaternion rotation, Vector3 localScale)
        {
            GameObject prepareObject = AvailableObject();
            prepareObject.SetActive(true);
            prepareObject.transform.position = position;
            prepareObject.transform.rotation = rotation;
            prepareObject.transform.localScale = localScale;
            return prepareObject;
        }
        
        // public void Return(GameObject gameObject)
        // {
        //     _queue.Enqueue(gameObject);
        // }
    }
}
