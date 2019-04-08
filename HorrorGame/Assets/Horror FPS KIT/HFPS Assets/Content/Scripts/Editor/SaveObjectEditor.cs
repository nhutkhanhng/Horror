using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveObject)), CanEditMultipleObjects]
public class SaveObjectEditor : Editor
{

    public SerializedProperty prop_saveType;
    public SerializedProperty prop_name;
    public SerializedProperty prop_obj;

    void OnEnable()
    {
        prop_saveType = serializedObject.FindProperty("saveType");
        prop_name = serializedObject.FindProperty("uniqueName");
        prop_obj = serializedObject.FindProperty("CustomObjectActive");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SaveObject.SaveType type = (SaveObject.SaveType)prop_saveType.enumValueIndex;
        EditorGUILayout.PropertyField(prop_saveType);
        EditorGUILayout.PropertyField(prop_name, new GUIContent("Unique Name:"));

        if (type == SaveObject.SaveType.ObjectActive)
        {
            EditorGUILayout.PropertyField(prop_obj, new GUIContent("Object Save:"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
