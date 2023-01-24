using Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
	[RequireComponent(typeof(PlayerScript))]
	public class PlayerInputScript : MonoBehaviour
	{
		private InputBindings _bindings;

		private PlayerScript _player;
		private bool PlayerExists => _player != null;

		private void OnEnable()
		{
			_player = GetComponent<PlayerScript>();
			if (!PlayerExists) return;

			_bindings = new InputBindings();
			_bindings.Player.Walk.performed += WalkInput;
			_bindings.Player.Walk.canceled += WalkInput;
			_bindings.Player.Hit.performed += HitInput;
			_bindings.Enable();
		}
		private void OnDisable()
		{
			_bindings.Disable();
		}

		private void WalkInput(InputAction.CallbackContext ctx)
		{
			_player.WalkInput = ctx.performed ? ctx.ReadValue<Vector2>() : Vector2.zero;
		}
		private void HitInput(InputAction.CallbackContext ctx)
		{
			_player.TryHit();
		}
	}
}