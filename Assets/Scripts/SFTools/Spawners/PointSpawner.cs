using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SF_Tools.Spawners
{
	public class PointSpawner : ISpawnBehavior {

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }

		#region implemented abstract members of Spawner

        protected override bool DoSpawn(GameObject spawnObj)
        {
            return base.DoSpawn(spawnObj);
        }

        protected override bool DoSpawn(List<GameObject> spawnObjs)
        {
            return base.DoSpawn(spawnObjs);
        }

        protected override List<SpawnObj> DoSpawn_Prefab(SpawnObj prefab)
        {
            return base.DoSpawn_Prefab(prefab);
        }

        protected override List<SpawnObj> DoSpawn_Prefab(List<SpawnObj> prefab)
        {
            return base.DoSpawn_Prefab(prefab);
        }

		#endregion
	}
}
