using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonkeyMovement : MonoBehaviour {

	public Transform Player1Obj;
	public Transform Player2Obj;

	public Slider SliderObj;

	public float Offset = 1f;

	private float lastVal = 0.5f;
	private Animator animator = null;

	void Start()
	{
		animator = GetComponent<Animator>();
	}

	void Update()
	{	
		Vector3 position = transform.position;

		position.x = Mathf.Lerp(Player1Obj.position.x, Player2Obj.position.x, SliderObj.value);

		position.x += Offset;
		transform.position = position;

		if(animator != null)
		{
			float diff = SliderObj.value - lastVal;

			if(diff < 0)
				animator.SetTrigger("DoLeft");
			else if(diff > 0)
				animator.SetTrigger("DoRight");	

			if(SliderObj.value != lastVal)
				lastVal = SliderObj.value;
		}
	}

}
