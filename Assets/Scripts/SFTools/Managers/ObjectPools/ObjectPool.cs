using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SF_Tools.Managers
{
	public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Editor Properties

        public int MaxInstances = 0;
        //public T Prefab;

        #endregion

        #region Private Members

        private List<T> instances = new List<T>();

        #endregion

        #region Public Properties

        public List<T> ActiveObjects
        {
            get { return instances.FindAll(x => x.gameObject.activeSelf); }
        }

        #endregion

        #region  Public Interface

        public T GetObject(bool activate)
        {
            T retObj = instances.Find(x => !x.gameObject.activeSelf);
            
            if(retObj != null)
                retObj.gameObject.SetActive(activate);
            
            return retObj;
        }

        public void Clear()
        {
            instances.ForEach(x => Destroy(x.gameObject));
            instances.Clear();
        }

        #endregion

        #region Private Routines

        private void Awake()
        {
            Clear();
            BuildInstances();
        }

        protected abstract T GetPrefab();

        private void BuildInstances()
        {
            for (int i = 0; i < MaxInstances; ++i)
            {
                T newObj = Instantiate<T>(GetPrefab());
                newObj.transform.parent = transform;
                newObj.gameObject.SetActive(false);
                instances.Add(newObj);
            }
        }

        private void OnDestroy()
        {
            Clear();
        }

        #endregion
    }
}
