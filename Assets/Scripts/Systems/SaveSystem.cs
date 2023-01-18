using Data;
using Gameplay;
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

		public static bool SaveRelevantData() // returns false if it could not save everything
		{
			string savePath = $"{Application.dataPath}/Saves";
			if (!PlayerScript.Exists) return false;
			PlayerData playerData = PlayerScript.Instance.Data;
			string playerDataString = JsonUtility.ToJson(playerData);

			System.IO.File.WriteAllText($"{savePath}/PlayerData.json", playerDataString);
			return true;
		}
	}
}