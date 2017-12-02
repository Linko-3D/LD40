using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
{
	public static T Instance_ { get; private set; }

	protected virtual void Awake()
	{
		if (Instance_ == null)
		{
			Instance_ = this as T;

			Transform currentTransform = transform;
			while (currentTransform.parent != null)
			{
				currentTransform = currentTransform.parent;
			}

			DontDestroyOnLoad(currentTransform.gameObject);
		}
		else if (Instance_ != this)
		{
			Destroy(this.gameObject);
		}
	}

#if UNITY_EDITOR
#endif
}