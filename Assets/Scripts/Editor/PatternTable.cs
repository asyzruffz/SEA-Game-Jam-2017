using UnityEngine;
using UnityEditor;

public static class PatternTable {
	
	public static void Show (SerializedProperty table) {
		EditorGUILayout.PropertyField (table);
		EditorGUI.indentLevel += 1;
		if (table.isExpanded) {
			for (int i = table.arraySize - 1; i >= 0; i--) {
				EditorGUILayout.PropertyField (table.GetArrayElementAtIndex (i), new GUIContent ("Row " + (i + 1)));
			}
		}
		EditorGUI.indentLevel -= 1;
	}
}
