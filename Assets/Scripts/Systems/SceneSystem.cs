using Gameplay;
using UnityEngine.SceneManagement;

namespace Systems
{
	public static class PersistentManager /*: PersistentSingleton<PersistentManager>*/
	{
		public static bool LoadPreviousScene()
		{
			int previousIndex = SceneManager.GetActiveScene().buildIndex - 1;
			if (previousIndex < 0) return false;
			SceneManager.LoadScene(previousIndex);
			OnSceneLoad();
			return true;
		}
		public static bool LoadNextScene()
		{
			int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
			if (nextIndex == SceneManager.sceneCountInBuildSettings) return false;
			SceneManager.LoadScene(nextIndex);
			OnSceneLoad();
			return true;
		}

		private static void OnSceneLoad()
		{
			if (PlayerScript.Exists)
			{
				if (LevelScript.Exists)
				{
					PlayerScript.Instance.ResetToPosition(LevelScript.Instance.spawnPosition);
				}
			}
			SaveSystem.AutoSave();
		}
	}
}