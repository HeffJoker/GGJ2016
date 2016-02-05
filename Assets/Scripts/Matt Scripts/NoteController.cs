using UnityEngine;
using System.Collections;

public class NoteController : MonoBehaviour {
	private Transform thisNote;
	public float destroyDelay;
	public float speed;
	public bool inTargetZone;
	public KeyCode note1Key;
	public KeyCode note2Key;
	public KeyCode note3Key;
	public KeyCode note4Key;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		thisNote.position= new Vector2(thisNote.position.x,thisNote.position.y - speed);
//		if (Input.GetKeyDown (note1Key) && inTargetZone && this.gameObject.name == "NotePF(Clone)") {
//			Destroy (this.gameObject);
//		}
//		if (Input.GetKeyDown (note2Key) && inTargetZone && this.gameObject.name == "NotePF 1(Clone)") {
//			Destroy (this.gameObject);
//		}
//		if (Input.GetKeyDown (note3Key) && inTargetZone && this.gameObject.name == "NotePF 2(Clone)") {
//			Destroy (this.gameObject);
//		}
//		if (Input.GetKeyDown (note4Key) && inTargetZone && this.gameObject.name == "NotePF 3(Clone)") {
//			Destroy (this.gameObject);
//		}
	}
	void OnEnable()
	{
		Destroy (this.gameObject, destroyDelay);
		thisNote = this.gameObject.transform;
		inTargetZone = false;
	}

	public void MissNote ()
	{
		Vector3 shkAmt = new Vector3 (3, 3, 0); 
		iTween.ShakeScale (this.gameObject,iTween.Hash( "Amount",shkAmt, "Time", 1));
	}


}
