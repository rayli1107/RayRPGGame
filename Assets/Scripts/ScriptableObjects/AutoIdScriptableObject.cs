using System;
using UnityEditor;
using UnityEngine;

public class AudoIdAttribute : PropertyAttribute { }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(AudoIdAttribute))]
public class AudoIdAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        if (string.IsNullOrEmpty(property.stringValue))
        {
            property.stringValue = Guid.NewGuid().ToString();
        }
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
#endif

public abstract class AutoIdScriptableObject : ScriptableObject
{
    [field: SerializeField, AudoId]
    public string id { get; private set; }

    [field: SerializeField]
    public new string name { get; private set; }
}