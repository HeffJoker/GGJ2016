using UnityEngine;
using System.Collections;

namespace SF_Tools.Weapons
{
	public class DamageProjectile : Projectile 
	{
		public int Damage = 1;

		protected override void OnTriggerEnter2D(Collider2D collider)
		{
			if(!isAlive)
				return;

			foreach(string tag in CollisionTags)
			{
				if(collider.gameObject.tag == tag)
				{
					Die();
					collider.gameObject.BroadcastMessage("TakeDamage", Damage);
					break;
				}
			}
		}
	}
}