using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(GameData))]
public class GameController : MonoBehaviourSingleton<GameController>
{
	private GameData _gameData;
	public GameData _GameData { get { return this._gameData; } }

	protected override void Awake()
	{
		base.Awake();

		this._gameData = this.GetComponent<GameData>();
	}

#if UNITY_EDITOR
	protected virtual void OnDrawGizmos()
	{

	}
#endif
}