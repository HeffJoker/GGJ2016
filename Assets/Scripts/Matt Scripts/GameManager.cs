using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Slider sacrificeSlider;
	public float player1acc = 0;
	public float player2acc = 0;

	// Use this for initialization
	void Start () {
		sacrificeSlider.value = .5f;
	}
	
	// Update is called once per frame
	void Update () {
	}
	public void UpdateSacSlider()
	{
		//Find accuracy diff
		float dif = player2acc - player1acc;
		sacrificeSlider.value += dif;
	}
}
