
using System.Collections.Generic;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Assets.Game.Scripts.Helpers
{
    public class ObjectsPool<T> where T : MonoBehaviour
    {
        private List<T> _objectsPool = new List<T>();
        private Transform _parent;
        private T _objectItem;
        private LayerMask _defaultLayer;

        public ObjectsPool(T objectItem, int initialSize = 1, Transform parent = null)
        {
            _parent = parent;
            _objectItem = objectItem;
            _defaultLayer = _objectItem.gameObject.layer;

            for (int i = 0; i < initialSize; i++)
            {
                _objectsPool.Add(CreateObject());
            }
        }

        private T CreateObject(bool isActive = false)
        {
            T newObject = Object.Instantiate(_objectItem,_parent);

            newObject.gameObject.SetActive(isActive);
            _objectsPool.Add(newObject);

            return newObject;
        }

        public T InstantiateToScene(Vector3 position, Quaternion rotation)
        {

            T instanceObj = null;

            foreach (var obj in _objectsPool)
            {
                if (!obj.gameObject.activeInHierarchy)
                {
                    obj.gameObject.SetActive(true);
                    instanceObj = obj;
                    Debug.Log("Get from pool");
                    break;
                }
            }

            if (instanceObj == null)
            {
                Debug.Log("Instantiate new");
                instanceObj = CreateObject(true);
            }


            instanceObj.transform.position = position;
            instanceObj.transform.rotation = rotation;
            instanceObj.gameObject.layer = _defaultLayer;

            return instanceObj;
        }

        public static void DeleteObjectFromScene(T obj, float delay = 0)
        {
            //obj.gameObject.layer = LayerMask.NameToLayer("Garbage");
            obj.gameObject.transform.position = new Vector3(0,0,-50);
            obj.StartCoroutine(DelayedDisable(obj, delay));
            //obj.gameObject.SetActive(false);
        }

        private static System.Collections.IEnumerator DelayedDisable(T obj, float interval)
        {
            yield return new WaitForSeconds(interval);
            obj.gameObject.SetActive(false);
        }
    }
}
