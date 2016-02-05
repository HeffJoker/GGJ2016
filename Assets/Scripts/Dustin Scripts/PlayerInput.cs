using UnityEngine;
using System.Collections;
using InControl;
using GGJ2016;

public class PlayerInput : MonoBehaviour {

	public int PlayerIndex = 0;
	public TargetManager target;

	private PlayerActions input = null;

	// Use this for initialization
	void Start () {
		input = PlayerActions.CreateWithDefaultBindings(PlayerIndex);
	}
	
	// Update is called once per frame
	void Update () {

        #region Button Down

        if (input.Left.IsPressed)
            target.OnButtonDown(TargetManagerInputType.Left);

        if (input.Right.IsPressed)
            target.OnButtonDown(TargetManagerInputType.Right);

        if (input.Up.IsPressed)
            target.OnButtonDown(TargetManagerInputType.Up);

        if (input.Down.IsPressed)
            target.OnButtonDown(TargetManagerInputType.Down);

        #endregion

        #region Button Up

        if (input.Left.WasReleased)
            target.OnButtonUp(TargetManagerInputType.Left);

        if (input.Right.WasReleased)
            target.OnButtonUp(TargetManagerInputType.Right);

        if (input.Up.WasReleased)
            target.OnButtonUp(TargetManagerInputType.Up);

        if (input.Down.WasReleased)
            target.OnButtonUp(TargetManagerInputType.Down);

        #endregion

        if(input.Left.WasPressed)
			target.HandleInput(TargetManagerInputType.Left);

		if(input.Right.WasPressed)
			target.HandleInput(TargetManagerInputType.Right);

		if(input.Up.WasPressed)
			target.HandleInput(TargetManagerInputType.Up);

		if(input.Down.WasPressed)
			target.HandleInput(TargetManagerInputType.Down);

	}
}
