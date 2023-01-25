using System;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace Data
{
	[Serializable]
	public class Upgrades
	{
		private List<StrengthUpgrade> _strengthUpgrades = new List<StrengthUpgrade>();
		public List<StrengthUpgrade> StrengthUpgrades
		{
			get
			{
				if (_strengthUpgrades == null)
				{
					Debug.LogWarning("StrengthUpgrades was null! Creating new.");
					_strengthUpgrades = new List<StrengthUpgrade>();
				}
				return _strengthUpgrades;
			}
			set
			{
				if (value == null)
				{
					Debug.LogWarning("Set Strength Upgrades list was null! Creating new.");
					_strengthUpgrades = new List<StrengthUpgrade>();
				}
				else
				{

					_strengthUpgrades = value;
				}
			}
		}

		public override string ToString()
		{
			if (StrengthUpgrades.Count == 0)
			{
				return "[Empty List]";
			}
			string listString = string.Join(", ", StrengthUpgrades);
			return $"[{listString}]";
		}
	}
}