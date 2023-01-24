using System.Collections.Generic;
using Data;
using Gameplay;
using NUnit.Framework;
using UnityEngine;

namespace Systems
{
	public static class SaveSystem
	{
		public static void AutoSave() // always on
		{
			bool autosaved = SaveRelevantData();
			if (!autosaved)
			{
				Debug.LogError("Could not autosave!");
			}
		}

		private static string SavePath => $"{Application.dataPath}/Saves";
		private static string GetPath<T>() => $"{SavePath}/{nameof(T)}.json";

		private static bool SaveRelevantData() // returns false if it could not save everything
		{
			if (!PlayerScript.Exists) return false;
			List<UpgradeStats> upgrades = PlayerScript.Instance.Upgrades;
			System.IO.File.WriteAllText(GetPath<List<UpgradeStats>>(), JsonUtility.ToJson(upgrades));
			// PlayerStats playerStats = PlayerScript.Instance.Stats;
			// string playerDataString = JsonUtility.ToJson(playerStats);
			//
			// System.IO.File.WriteAllText(PlayerDataPath, playerDataString);
			return true;
		}

		// public static PlayerStats LoadPlayerStats()
		// {
		// 	string playerDataString = System.IO.File.ReadAllText(PlayerDataPath);
		// 	PlayerStats playerStats = JsonUtility.FromJson<PlayerStats>(playerDataString);
		// 	if (playerStats == null) return new PlayerStats();
		// 	return playerStats;
		// }

		public static List<UpgradeStats> LoadPlayerUpgrades()
		{
			string loadedString = System.IO.File.ReadAllText(GetPath<List<UpgradeStats>>());
			return JsonUtility.FromJson<List<UpgradeStats>>(loadedString);
		}
	}
}