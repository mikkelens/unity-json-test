using Data;
using Tools.Helpers;
using Tools.Types;
using UnityEngine;

namespace Gameplay
{
	public class PlayerScript : PersistentSingleton<PlayerScript>
	{
		[SerializeField] private float moveSpeed = 4f;
		[SerializeField] private float moveAcceleration = 4f;

		[SerializeField] private float hitRadius = 1f;
		[SerializeField] private float hitDistanceOffset = 2f;

		public PlayerData Data { get; } = new PlayerData(); // todo: load data

		public Vector2 WalkInput { private get; set; }

		private Vector2 _velocity;

		private void FixedUpdate()
		{
			_velocity = Vector2.MoveTowards(_velocity, WalkInput * moveSpeed, moveAcceleration * Time.fixedDeltaTime);
			transform.Translate(_velocity.AsV3FromV2() * Time.fixedDeltaTime);
		}

		public void ResetToPosition(Vector2 resetPosition)
		{
			transform.position = resetPosition.AsV3FromV2();
		}

		public void TryHit()
		{
			EnemyScript enemy = FindEnemy();
			if (enemy == null) return;

			bool killedEnemy = enemy.Hit();
			if (killedEnemy) Data.points++;
		}

		private EnemyScript FindEnemy()
		{
			return null;
		}
	}
}