using System;
using System.Collections;
using Gameplay;
using UnityEngine;

namespace Systems
{
	public static class SaveSystem
	{
		private static bool _willSaveAtEndOfFrame;
		public static void AutoSaveAtEnd()
		{
			if (_willSaveAtEndOfFrame) return;
			if (Manager.Exists)
			{
				_willSaveAtEndOfFrame = true;
				Manager.Instance.StartCoroutine(AutoSaveEnumerator());
			}
			else
			{
				Debug.LogWarning("Could not autosave after delay because no manager gameobject existed to start a coroutine.");
			}
		}

		private static IEnumerator AutoSaveEnumerator()
		{
			yield return new WaitForEndOfFrame();
			AutoSaveImmediately();
			_willSaveAtEndOfFrame = false;
		}
		public static void AutoSaveImmediately() // always on
		{
			if (PlayerScript.Exists)
			{
				PlayerScript player = PlayerScript.Instance;
				SavePlayerData(player.Scores.ReadValue);
				SavePlayerData(player.Upgrades.ReadValue);
			}
		}

		private static string SavePath => $"{Application.dataPath}/Saves";
		private static string GetPath(Type dataType)
		{
			if (!System.IO.File.Exists(SavePath))
			{
				System.IO.Directory.CreateDirectory(SavePath); // create saves folder
			}
			return $"{SavePath}/{dataType}.json";
		}

		private static void SavePlayerData(object data)
		{
			Debug.Log($"Saved data of type {data.GetType()}.");
			System.IO.File.WriteAllText(GetPath(data.GetType()), JsonUtility.ToJson(data));
		}

		public static T LoadTypeData<T>() where T : new()
		{
			string path = GetPath(typeof(T));
			if (!System.IO.File.Exists(path))
			{
				Debug.LogWarning($"Found no data of type {typeof(T).Name}! Created new.");
				return new T();
			}
			string loadedString = System.IO.File.ReadAllText(path);
			T loadedData = JsonUtility.FromJson<T>(loadedString);
			if (loadedData == null)
			{
				Debug.LogWarning("Data was null! Created new.");
				return new T();
			}
			Debug.Log($"Loaded data of type {typeof(T).Name}.");
			return loadedData;
		}
	}
}