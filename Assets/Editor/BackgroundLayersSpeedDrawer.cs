
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BackgroundLayersSpeed))]
public class BackgroundLayersSpeedDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty gameObject = property.FindPropertyRelative("GameObject");
        SerializedProperty layerSpeed = property.FindPropertyRelative("LayerSpeed");

        Rect labelPosition = new Rect(position.x, position.y, position.width, position.height);
        position = EditorGUI.PrefixLabel(
            labelPosition,
            EditorGUIUtility.GetControlID(FocusType.Passive),
            new GUIContent(
                gameObject.objectReferenceValue != null ?
                (gameObject.objectReferenceValue as GameObject).name :
                "Empty"
                ));
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        float widthSize = position.width / 3;
        float offsetSize = 5;
        Rect pos1 = new Rect(position.x, position.y, 2 * widthSize - offsetSize, position.height);
        Rect pos2 = new Rect(position.x + 2 * widthSize, position.y, widthSize, position.height);
        EditorGUI.PropertyField(pos1, gameObject, GUIContent.none);
        EditorGUI.PropertyField(pos2, layerSpeed, GUIContent.none);
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
