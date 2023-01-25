using System;

namespace Gameplay
{
	[Serializable]
	public class StrengthUpgrade
	{
		public int damageUpgrade;
		public float moveSpeedUpgrade;

		public override string ToString()
		{
			return $"Damage Upgrade: {damageUpgrade}, MoveSpeed Upgrade: {moveSpeedUpgrade}";
		}
	}
}