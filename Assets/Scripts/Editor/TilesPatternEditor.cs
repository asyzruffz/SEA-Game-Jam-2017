using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (TilesPattern))]
public class TilesPatternEditor : Editor {

	public override void OnInspectorGUI () {
		serializedObject.Update ();
		EditorGUILayout.PropertyField (serializedObject.FindProperty ("rowNo"), true);
		//EditorGUILayout.PropertyField (serializedObject.FindProperty ("pattern"), true);
		PatternTable.Show (serializedObject.FindProperty ("pattern"));
		serializedObject.ApplyModifiedProperties ();
	}
}
