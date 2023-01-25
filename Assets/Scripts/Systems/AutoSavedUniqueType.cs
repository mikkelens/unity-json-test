using UnityEngine;

namespace Systems
{
	public struct AutoSavedUniqueType<T> where T : new()
	{
		private T _value;
		public readonly T ReadValue => _value; // use when not changing
		public T MutValue // use when changing
		{
			get
			{
				Debug.Log($"MutValue '{_value.GetType()}': {_value}");
				SaveSystem.AutoSaveAtEnd();
				return _value;
			}
		}

		public void LoadFromFileType()
		{
			_value = SaveSystem.LoadTypeData<T>(); // not null!
			Debug.Log($"Loaded {typeof(T).Name}: {_value.ToString()} from file (using type).");
		}
	}
}