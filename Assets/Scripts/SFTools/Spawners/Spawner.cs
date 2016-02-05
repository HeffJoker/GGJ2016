using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SF_Tools.Managers;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

namespace SF_Tools.Spawners
{
	public abstract class Spawner : MonoBehaviour
    {
        #region Editor Properties

        public float SpawnTime = 1f;		// The amount of time between each spawn.
        public float SpawnDelay = 0f;		// The amount of time before spawning starts.
        public float Duration = 5f;
        public int NumObjPerSpawn = 1;
        public bool SpawnOnWake = false;
        public bool PassPrefab = false;

        #endregion

        #region Private Members

        protected ISpawnBehavior[] behaviors;
        protected List<SpawnObj> spawnObjs = new List<SpawnObj>();
        protected bool isSpawning = false;

        #endregion

        #region Public Interface

        public void StartSpawning()
        {
            isSpawning = true;
            StartCoroutine(Spawn());
        }

        public void StopSpawning()
        {
            isSpawning = false;
            StopAllCoroutines();

            spawnObjs.Clear();
        }

        public void ToggleSpawn(bool val)
        {
            if (val)
                StartSpawning();
            else
                StopSpawning();
        }

        #region GUI Routines

        public void IncrementDuration(float val)
        {
            Duration += val;
        }

        public void IncrementSpawnTime(float val)
        {
            SpawnTime += val;
        }

        public void SetTextDuration(Text txtDuration)
        {
            txtDuration.text = Duration.ToString();
        }

        public void SetTextSpawnTime(Text txtSpawnTime)
        {
            txtSpawnTime.text = SpawnTime.ToString();
        }

        #endregion

        #region GUI Properties

        public float SpawnTimeVal
        {
            get { return SpawnTime; }
            set
            {
                ToggleSpawn(false);
                SpawnTime = value;
                ToggleSpawn(true);
            }
        }

        public float DurationVal
        {
            get { return Duration; }
            set
            {
                ToggleSpawn(false);
                Duration = value;
                ToggleSpawn(true);
            }
        }

        #endregion
        
        #endregion

        #region Private Routines
        
        void Start()
        {
            behaviors = GetComponents<ISpawnBehavior>();

            if (behaviors.Length <= 0)
                Debug.LogError("[SPAWNER]: Does not contain a spawn behavior.  Please add one to get correct spawning.", gameObject);

            OnStart();

            if (SpawnOnWake)
                StartSpawning();
        }

        IEnumerator Spawn()
        {
            yield return new WaitForSeconds(SpawnDelay);

            while (isSpawning)
            {
                List<SpawnObj> newSpawn = ChooseSpawn();

                if (PassPrefab)
                {
                    foreach (ISpawnBehavior behavior in behaviors)
                        spawnObjs.AddRange(behavior.Spawn_Prefab(newSpawn));
                }
                else
                {
                    foreach (ISpawnBehavior behavior in behaviors)
                        behavior.Spawn(newSpawn);
                }

                yield return new WaitForSeconds(SpawnTime);
            }
        }

        protected abstract List<SpawnObj> ChooseSpawn();

        protected virtual void OnStart() { }

        #endregion
    }
}



