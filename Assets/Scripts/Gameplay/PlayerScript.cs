using System.Linq;
using Data;
using Systems;
using Tools.Helpers;
using Tools.Types;
using UnityEngine;

namespace Gameplay
{
	[RequireComponent(typeof(Collider2D))]
	public class PlayerScript : PersistentSingleton<PlayerScript>
	{
		[SerializeField] private float startingMoveSpeed = 4f;
		[SerializeField] private float moveAcceleration = 4f;

		[SerializeField] private int startingDamage = 1;
		[SerializeField] private LayerMask hitTargets;
		[SerializeField] private float hitRadius = 1f;
		[SerializeField] private float hitDistanceOffset = 2f;

		// saved/loaded
		[field: SerializeField] public AutoSavedUniqueType<Statistics> Scores { get; private set; }
		[field: SerializeField] public AutoSavedUniqueType<Upgrades> Upgrades { get; private set; }

		// runtime
		[field: SerializeField] public PlayerStrengthValues StrengthValues { get; private set; } = new PlayerStrengthValues();

		public Vector2 WalkInput { private get; set; }

		private Vector2 _velocity;

		private void Start()
		{
			Scores.LoadFromFileType();
			Upgrades.LoadFromFileType();
			ReCalculateStrength();
		}

		private void FixedUpdate()
		{
			_velocity = Vector2.MoveTowards(_velocity, WalkInput * StrengthValues.increaseddMoveSpeed, moveAcceleration * Time.fixedDeltaTime);
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

			Debug.Log($"Dealt {StrengthValues.increasedDamage.ToString()} damage to '{enemy.name}'.");
			bool killedEnemy = enemy.Hit(StrengthValues.increasedDamage);
			if (killedEnemy)
			{
				Scores.MutValue.kills++;
			}
		}

		private EnemyScript FindEnemy()
		{
			Vector2 castPos = transform.position.AsV2FromV3() + transform.up.AsV2FromV3() * hitDistanceOffset;
			Collider2D[] colliders = Physics2D.OverlapCircleAll(castPos, hitRadius, hitTargets.value);
			if (colliders.Length == 0) return null;
			return colliders.Select(x => x.GetComponent<EnemyScript>()).First(x => x != null);
		}

		private int _realDamage;
		public void AddUpgrade(StrengthUpgrade stats)
		{
			Upgrades.MutValue.StrengthUpgrades.Add(stats);
			ReCalculateStrength();
		}
		private void ReCalculateStrength()
		{
			PlayerStrengthValues strengthValues = new PlayerStrengthValues { increasedDamage = startingDamage, increaseddMoveSpeed = startingMoveSpeed };
			Debug.Log($"Upgrades.MutValue: {Upgrades.MutValue}");
			foreach (StrengthUpgrade upgrade in Upgrades.MutValue.StrengthUpgrades)
			{
				strengthValues.increasedDamage += upgrade.damageUpgrade;
				strengthValues.increaseddMoveSpeed += upgrade.moveSpeedUpgrade;
			}
			StrengthValues = strengthValues;
		}

		private void OnDrawGizmos()
		{
			Vector2 hitPos = transform.position.AsV2FromV3() + transform.up.AsV2FromV3() * hitDistanceOffset;
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(hitPos.AsV3FromV2(), hitRadius);
		}
	}
}