using UnityEngine;

namespace Gameplay
{
	public class EnemyScript : MonoBehaviour
	{
		[SerializeField] private int startingHP = 3;

		private int _hp;

		private void Awake()
		{
			_hp = startingHP;
		}

		public bool Hit() // returns true if killed enemy
		{
			_hp--;

			if (_hp > 0) return false;

			Die();
			return true;

		}

		private void Die()
		{

		}
	}
}