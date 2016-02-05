using System;
using InControl;
using UnityEngine;

namespace GGJ2016
{
	public class PlayerActions : PlayerActionSet
	{
		public PlayerAction Left;
		public PlayerAction Right;
		public PlayerAction Up;
		public PlayerAction Down;

		public PlayerActions()
		{
			Left = CreatePlayerAction( "Move Left" );
			Right = CreatePlayerAction( "Move Right" );
			Up = CreatePlayerAction( "Move Up" );
			Down = CreatePlayerAction( "Move Down" );
		}

		public static PlayerActions CreateWithDefaultBindings(int playerNum)
		{
			var playerActions = new PlayerActions();

			if(playerNum == 0)
			{
				playerActions.Up.AddDefaultBinding( Key.W );
				playerActions.Down.AddDefaultBinding( Key.S );
				playerActions.Left.AddDefaultBinding( Key.A );
				playerActions.Right.AddDefaultBinding( Key.D );
			}
			else
			{							
				playerActions.Up.AddDefaultBinding( Key.UpArrow );
				playerActions.Down.AddDefaultBinding( Key.DownArrow );
				playerActions.Left.AddDefaultBinding( Key.LeftArrow );
				playerActions.Right.AddDefaultBinding( Key.RightArrow );
			}

			playerActions.Left.AddDefaultBinding( InputControlType.LeftStickLeft );
			playerActions.Right.AddDefaultBinding( InputControlType.LeftStickRight );
			playerActions.Up.AddDefaultBinding( InputControlType.LeftStickUp );
			playerActions.Down.AddDefaultBinding( InputControlType.LeftStickDown );

			playerActions.Left.AddDefaultBinding( InputControlType.DPadLeft );
			playerActions.Right.AddDefaultBinding( InputControlType.DPadRight );
			playerActions.Up.AddDefaultBinding( InputControlType.DPadUp );
			playerActions.Down.AddDefaultBinding( InputControlType.DPadDown );

			playerActions.Left.AddDefaultBinding( InputControlType.LeftBumper );
			playerActions.Right.AddDefaultBinding( InputControlType.RightBumper );
			playerActions.Up.AddDefaultBinding( InputControlType.RightTrigger );
			playerActions.Down.AddDefaultBinding( InputControlType.LeftTrigger );

			playerActions.ListenOptions.IncludeUnknownControllers = true;
			playerActions.ListenOptions.MaxAllowedBindings = 4;
			//			playerActions.ListenOptions.MaxAllowedBindingsPerType = 1;
			//			playerActions.ListenOptions.UnsetDuplicateBindingsOnSet = true;
			//			playerActions.ListenOptions.IncludeMouseButtons = true;

			playerActions.ListenOptions.OnBindingFound = ( action, binding ) =>
			{
				if (binding == new KeyBindingSource( Key.Escape ))
				{
					action.StopListeningForBinding();
					return false;
				}
				return true;
			};

			playerActions.ListenOptions.OnBindingAdded += ( action, binding ) =>
			{
				Debug.Log( "Binding added... " + binding.DeviceName + ": " + binding.Name );
			};

			playerActions.ListenOptions.OnBindingRejected += ( action, binding, reason ) =>
			{
				Debug.Log( "Binding rejected... " + reason );
			};

			return playerActions;
		}
	}
}
