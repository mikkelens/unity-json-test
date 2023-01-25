using System.Linq;
using Tools.Helpers;
using UnityEngine;

namespace Gameplay
{
	[RequireComponent(typeof(CircleCollider2D))]
	public class UpgradeScript : MonoBehaviour
	{
		[SerializeField] private LayerMask targetMask;

		public StrengthUpgrade stats;

		private CircleCollider2D _circle;
		private CircleCollider2D Circle => _circle ??= GetComponent<CircleCollider2D>();

		private void FixedUpdate()
		{
			PlayerScript player = TryFindPlayer();
			if (player == null) return;
			player.AddUpgrade(stats);
			Destroy(gameObject);
		}

		private PlayerScript TryFindPlayer()
		{
			Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position.AsV2FromV3(), Circle.radius, targetMask.value);
			if (colliders.Length == 0) return null;
			return colliders.Select(x => x.GetComponent<PlayerScript>()).First(x => x != null);
		}
	}
}