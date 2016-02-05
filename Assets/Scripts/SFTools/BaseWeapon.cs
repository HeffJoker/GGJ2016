using UnityEngine;
using System.Collections;

namespace SF_Tools.Weapons
{
	[HideInInspector]
	public abstract class BaseWeapon : MonoBehaviour {

		#region Editor Properties

		public float FireRate = 1f;
		public float FireDelay = 0f;
		public bool IsSingleShot = false;

		#endregion	

		#region Private Members

		protected bool firing = false;
		protected Vector2 fireDir = Vector2.zero;

		#endregion

		#region Public Properties

		public bool IsFiring
		{
			get { return firing; }
		}

		#endregion

		#region Public Interface

		public void Fire(Vector2 direction)
		{
			fireDir = direction;

			if(IsSingleShot)
			{
				OnFireStart();
				DoFire();
			}
			else
			{
				if(!firing)
				{
					InvokeRepeating("DoFire", FireDelay, FireRate);
					OnFireStart();
				}

				firing = true;
			}
		}

		public void Fire(GameObject target)
		{
			if(target == null)
				return;

			Vector3 tempDir = (target.transform.position - transform.position).normalized;
			fireDir = new Vector2(tempDir.x, tempDir.y);

			if(IsSingleShot)
			{
				OnFireStart();
				DoFire();
			}
			else
			{
				if(!firing)
				{
					InvokeRepeating("DoFire", FireDelay, FireRate);
					OnFireStart();
				}

				firing = true;
			}
		}
		
		public void Stop()
		{
			firing = false;
			fireDir = Vector2.zero;
			CancelInvoke("DoFire");
			OnFireStop();
		}
		
		public void OnDestroy()
		{
			CleanUp();
		}

		#endregion

		#region Private Routines

		protected virtual void OnFireStart(){}
		protected virtual void OnFireStop(){}
		protected abstract void DoFire();
		protected abstract void CleanUp();

		#endregion
	}
}
