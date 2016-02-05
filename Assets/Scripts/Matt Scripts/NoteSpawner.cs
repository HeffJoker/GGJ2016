using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoteSpawner : MonoBehaviour {

	public List<GameObject> notes;
	public GameObject spawnNode;
	public float spawnDelay;
	public bool gameOver;
	public TargetManager tm;

	public void SpawnNote(GameObject note)
	{
		Instantiate (note, spawnNode.transform.position, Quaternion.identity);
		tm.spawnCount += 1;

	}

	IEnumerator SpawnRandomNote()
	{
		while (!gameOver) {
			yield return new WaitForSeconds (spawnDelay);
			int i = Random.Range (0, 4);
			GameObject go = notes [i];
			SpawnNote (go);
		}

	}


	// Use this for initialization
	void Start () {
		gameOver = false;
		StartCoroutine("SpawnRandomNote");
	}
	

}
