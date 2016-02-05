using SF_Tools.Managers;
using UnityEngine;

namespace SF_Tools.Weapons
{
    public class ProjectileWeapon : BaseWeapon {

		#region Editor Properties

		public GameObject ProjPrefab;
		public int NumProjectiles = 5;

		#endregion

		#region Private Members

		private int currProjIndex = 0;
		private Projectile[] projectiles;

		#endregion 

		#region Private Routines

		void Start () 
		{
			projectiles = ProjectileManager.Instance.GetProjectiles(NumProjectiles, ProjPrefab);
		}

		void Update()
		{
		}
				
		protected override void DoFire()
		{
			//if(IsSingleShot && projectiles[0].isAlive)
			//	Stop();
			//else
			//{
				projectiles[currProjIndex].Move(transform.position, fireDir);
				currProjIndex = (currProjIndex < (NumProjectiles-1) ? currProjIndex + 1 : 0);
			//}
		}
		
		protected override void CleanUp()
		{
			ProjectileManager.Instance.RemoveProjectiles(projectiles);
		}

		#endregion
	}
}