using System.Collections.Generic;
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

		public PlayerStats Stats { get; private set; } = new PlayerStats();
		public List<UpgradeStats> Upgrades { get; private set; } = new List<UpgradeStats>();

		public Vector2 WalkInput { private get; set; }

		private Vector2 _velocity;

		private void Start()
		{
			// Stats = SaveSystem.LoadPlayerStats();
			Upgrades = SaveSystem.LoadPlayerUpgrades();
			ReCalculateStats();
		}

		private void FixedUpdate()
		{
			_velocity = Vector2.MoveTowards(_velocity, WalkInput * Stats.moveSpeed, moveAcceleration * Time.fixedDeltaTime);
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

			Debug.Log($"Dealt {Stats.damage.ToString()} damage to '{enemy.name}'.");
			bool killedEnemy = enemy.Hit(Stats.damage);
			if (killedEnemy) Stats.points++;
		}

		private EnemyScript FindEnemy()
		{
			Vector2 castPos = transform.position.AsV2FromV3() + transform.up.AsV2FromV3() * hitDistanceOffset;
			Collider2D[] colliders = Physics2D.OverlapCircleAll(castPos, hitRadius, hitTargets.value);
			if (colliders.Length == 0) return null;
			return colliders.Select(x => x.GetComponent<EnemyScript>()).First(x => x != null);
		}

		private int _realDamage;
		public void AddUpgrade(UpgradeStats stats)
		{
			Upgrades.Add(stats);
			ReCalculateStats();
		}
		private void ReCalculateStats()
		{
			PlayerStats stats = new PlayerStats { points = Stats.points, damage = startingDamage, moveSpeed = startingMoveSpeed };
			foreach (UpgradeStats upgrade in Upgrades)
			{
				stats.damage += upgrade.damageUpgrade;
				stats.moveSpeed += upgrade.moveSpeedUpgrade;
			}
			Stats = stats;
			SaveSystem.AutoSave();
		}

		private void OnDrawGizmos()
		{
			Vector2 hitPos = transform.position.AsV2FromV3() + transform.up.AsV2FromV3() * hitDistanceOffset;
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(hitPos.AsV3FromV2(), hitRadius);
		}
	}
}