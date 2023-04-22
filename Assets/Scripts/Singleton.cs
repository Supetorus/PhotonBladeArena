using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public abstract class Singleton<T> : MonoBehaviour
public class Singleton<T> : MonoBehaviour where T : Component
{
	public static T Instance { get; private set; }

	private void Start()
	{
		if (Instance != null)
		{
			Destroy(this);
			return;
		}

		Instance = this as T;
	}
}
