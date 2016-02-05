using AssemblyCSharp;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SF_Tools.Spawners
{
    public class DesignedSpawner : Spawner
	{
		#region Editor Properties

		public List<SpawnType> SpawnTypes;

		#endregion
                
		#region Private Routines
        
        protected override List<SpawnObj> ChooseSpawn()
        {
            float chance = UnityEngine.Random.Range(0f, 100f);
            List<SpawnObj> newSpawn = new List<SpawnObj>();

            foreach (SpawnType type in SpawnTypes)
            {
                if (chance >= type.LowerChance && chance <= type.UpperChance)
                {
                    if (PassPrefab)
                    {
                        newSpawn.Add(type.Prefab);
                    }
                    else
                    {
                        for (int i = 0; i < NumObjPerSpawn; ++i)
                        {
                            throw new Exception("FIX THIS DUSTIN!!");
                            SpawnObj newObj = new SpawnObj(); // UnitManager.Instance.GetUnit(type.Prefab);
                            newSpawn.Add(newObj);
                            spawnObjs.Add(newObj);
                        }
                    }

                    break;
                }
            }

            return newSpawn;
        }

		#endregion
	}

    public class SpawnObj : MonoBehaviour
    {
    }

	[Serializable]
	public class SpawnType
	{
		public SpawnObj Prefab;
		public float LowerChance = 0f;
		public float UpperChance = 100f;
	}

	[HideInInspector]
	public abstract class ISpawnBehavior : MonoBehaviour
    {
        #region Editor Properties

        public bool OverrideMovement = false;
        public float MoveDirAngle = -90f;

        #endregion

        #region Public Interface

        public void Spawn(List<SpawnObj> spawnObjs)
		{
			if(spawnObjs.Count > 1)
			{
				List<GameObject> objs = new List<GameObject>();
				spawnObjs.ForEach(x => objs.Add(x.gameObject));
				DoSpawn(objs);
			}
            else if (spawnObjs.Count > 0)
            {
                if (spawnObjs[0].gameObject != null)
                    DoSpawn(spawnObjs[0].gameObject);
                else
                    Debug.LogError("There was a problem spawning " + spawnObjs[0] + "!");
            }
            else
                Debug.LogError("The spawner tried to spawn 0 objects!  There be a bug!");
		}

		public List<SpawnObj> Spawn_Prefab(List<SpawnObj> prefabs)
		{
			List<SpawnObj> retList = new List<SpawnObj>();

			if(prefabs.Count > 1)
				retList.AddRange(DoSpawn_Prefab(prefabs));
			else if(prefabs.Count > 0)
				retList.AddRange(DoSpawn_Prefab(prefabs[0]));

			return retList;
		}

		#endregion

		#region Private Routines

		protected void MakeObj(GameObject newObj, Vector2 position)
		{
            MakeObj(newObj, position, MoveDirAngle);
		}

        protected void MakeObj(GameObject newObj, Vector2 position, float moveAngle)
        {
            newObj.transform.position = position;
            newObj.SetActive(true);
        }
        
		protected virtual bool DoSpawn(GameObject spawnObj)
		{
			MakeObj(spawnObj, transform.position);
			return true;
		}

		protected virtual bool DoSpawn(List<GameObject> spawnObjs)
		{
			spawnObjs.ForEach(x => DoSpawn(x));
			return true;
		}

		protected virtual List<SpawnObj> DoSpawn_Prefab(SpawnObj prefab)
		{
			return new List<SpawnObj>();
		}

		protected virtual List<SpawnObj> DoSpawn_Prefab(List<SpawnObj> prefab)
		{
			return new List<SpawnObj>();
		}

		#endregion
	}
}


