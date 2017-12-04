using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameplayDisplay : Display
{
	[SerializeField] private Text _weightTextField;
	[SerializeField] private Text _cakesTextField;
	[SerializeField] private Text _teaTextField;
	[SerializeField] private Text _timerTextField;

	public void DisplayWeight(int weight)
	{
		this._weightTextField.text = "Weight - " + weight.ToString();
	}

	public void DisplayCakes(int quantity)
	{
		this._cakesTextField.text = "Cakes - " + quantity.ToString();
	}

	public void DisplayTea(int quantity)
	{
		this._teaTextField.text = "Tea - " + quantity.ToString();
	}

	public void DisplayTimer(int time)
	{
		this._timerTextField.text = "Time left - " + time.ToString();
	}

#if UNITY_EDITOR
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
	}
#endif
}

namespace New.UTILITY
{
#if UNITY_EDITOR
	[CustomEditor(typeof(GameplayDisplay))]
	[CanEditMultipleObjects]
	public class GameplayDisplayEditor : Editor
	{
		private void OnEnable()
		{

		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

#pragma warning disable 0219
			GameplayDisplay sGameplayDisplay = target as GameplayDisplay;
#pragma warning restore 0219
		}
	}
#endif
}