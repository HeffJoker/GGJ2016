using UnityEngine;
using System.Collections;

namespace SF_Tools.Managers
{
	public abstract class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
	{
		#region Private Members

		private static T instance = default(T);
		private static bool appIsQuitting = false;

		#endregion

		#region Public Properties

		public static T Instance
		{
			get 
			{
				if(appIsQuitting)
				{
					Debug.Log(string.Format("Singleton instance of {0} is already destroyed from the application quit. It won't be created... Returning null",
					                        typeof(T)));
					return null;
				}
				
				if(instance == null)
				{
					instance = (T)FindObjectOfType(typeof(T));

					if(instance == null)
					{
						GameObject singleton = new GameObject();
						instance = singleton.AddComponent<T>();
						singleton.name = string.Format("[SINGLETON] {0}", typeof(T));
						DontDestroyOnLoad(singleton);
					}
				}

				return instance; 
			}
		}

		#endregion

		#region Private Routines

		private void Awake()
		{
			if(instance == null)
			{
				instance = this as T;
				DontDestroyOnLoad(this);
			}
			else
			{
				if(this != instance)
				{
					Debug.LogError(string.Format("Found an extra instance of {0}.  Deleting now...", typeof(T)));
					Destroy(this.gameObject);
				}
			}

			appIsQuitting = false;
			OnWake();
		}

		private void OnDestroy()
		{
			appIsQuitting = true;
			instance = default(T);
			OnDestroy_Sub();
		}

		protected virtual void OnWake() {}
		protected virtual void OnDestroy_Sub() {}

		#endregion

		#region Public Interface

		public void Dispose()
		{
			instance = default(T);
			Destroy(gameObject);
		}

		#endregion
	}
}