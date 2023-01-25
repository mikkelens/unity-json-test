using UnityEngine;

namespace Gameplay
{
	[RequireComponent(typeof(Collider2D), typeof(Animator))]
	public class EnemyScript : MonoBehaviour
	{
		[SerializeField] private int startingHP = 3;

		private int _hp;

		private Animator _anim;
		private static readonly int DamageTriggerString = Animator.StringToHash("TakeDamage");
		private static readonly int DieTriggerString = Animator.StringToHash("Die");

		private void Awake()
		{
			_hp = startingHP;
			_anim = GetComponent<Animator>();
		}

		public bool Hit(int damage) // returns true if killed enemy
		{
			_hp -= damage;

			if (_hp > 0)
			{
				_anim.SetTrigger(DamageTriggerString);
				return false;
			}

			_anim.SetTrigger(DieTriggerString);
			Destroy(gameObject);
			return true;
		}
	}
}