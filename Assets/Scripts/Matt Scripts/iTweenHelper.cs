using UnityEngine;
using System.Collections;

public class iTweenHelper : MonoBehaviour {

	public static iTweenHelper Instance = null;

	// Use this for initialization
	void Awake () {
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy (this.gameObject);
	}

	public void ShakeObjScale(GameObject go, float xAmt,float yAmt,float time)
	{
		iTween.ShakeScale (go, iTween.Hash ("Amount", new Vector3 (xAmt, yAmt, 0), "Time", time));
	}

	public void ShakeObject(GameObject obj, float xAmt,float yAmt,float time)
	{
		iTween.ShakePosition(obj,iTween.Hash("Amount",new Vector3(xAmt,yAmt,0),"Time",time));
	}


}
