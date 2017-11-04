using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer (typeof (TileRow))]
public class TileRowDrawer : PropertyDrawer {

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
		Rect contentPosition = EditorGUI.PrefixLabel (position, label);
		contentPosition.width /= 3;

		EditorGUI.PropertyField (contentPosition, property.FindPropertyRelative ("col1"), GUIContent.none);
		contentPosition.x += contentPosition.width;
		EditorGUI.PropertyField (contentPosition, property.FindPropertyRelative ("col2"), GUIContent.none);
		contentPosition.x += contentPosition.width;
		EditorGUI.PropertyField (contentPosition, property.FindPropertyRelative ("col3"), GUIContent.none);
	}
}
