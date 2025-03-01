using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Telkom
{
    public class ObjectPool : MonoBehaviour
    {
        public GameObject ObjectPrefab;
        [SerializeField] private Queue<GameObject> _availableObject = new Queue<GameObject>();

        public GameObject GetObjectFromPool()
        {
            if (_availableObject.Count == 0)
                SpawnObject();
            GameObject instance = _availableObject.Dequeue();
            instance.SetActive(true);
            return instance;
        }

        public void AddObjectToPool(GameObject objectInstance)
        {
            objectInstance.SetActive(false);
            _availableObject.Enqueue(objectInstance);
        }

        private void SpawnObject()
        {
            GameObject objectToAdd = Instantiate(ObjectPrefab);
            AddObjectToPool(objectToAdd);
        }
    }
}


