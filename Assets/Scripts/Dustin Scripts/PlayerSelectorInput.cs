using UnityEngine;
using System.Collections;
using InControl;
using GGJ2016;

public class PlayerSelectorInput : MonoBehaviour {

	public PlayerActions Player1Actions;
	public PlayerActions Player2Actions;

	void Start()
	{
		Player1Actions = PlayerActions.CreateWithDefaultBindings(0);
		Player2Actions = PlayerActions.CreateWithDefaultBindings(1);
	}
}
