using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Shoguneko
{
    [CustomPropertyDrawer(typeof(SpawnPoint))]
    public class SpawnPoint_Drawer : PropertyDrawer
    {

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            Rect idRect = new Rect(position.x, position.y, Screen.width / 2.65f, position.height);
            Rect posRect = new Rect(position.x + Screen.width / 2.45f, position.y, Screen.width / 2f, position.height);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(idRect, property.FindPropertyRelative("id"), GUIContent.none);
            EditorGUI.PropertyField(posRect, property.FindPropertyRelative("position"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }

    [CanEditMultipleObjects]
    [CustomEditor(typeof(PlayerSpawner))]
    public class PlayerSpawner_Editor : Editor
    {
        SerializedProperty player;
        SerializedProperty spawnPoints;

        private void OnEnable()
        {
            player = serializedObject.FindProperty("_player");
            spawnPoints = serializedObject.FindProperty("spawnPoints");
        }

        public override void OnInspectorGUI()
        {
            // Set the indentLevel to 0 as default (no indent).
            EditorGUI.indentLevel = 0;
            // Update
            serializedObject.Update();

            // Player property
            EditorGUILayout.PropertyField(player);

            //  >>> THIS PART RENDERS THE ARRAY
            EditorGUILayout.PropertyField(spawnPoints.FindPropertyRelative("Array.size"));

            // Start a horizontal layout.
            EditorGUILayout.BeginHorizontal();
            // Label fields to describe drawer.
            EditorGUILayout.LabelField("ID", EditorStyles.boldLabel, GUILayout.Width(Screen.width / 2.48f));
            EditorGUILayout.LabelField("Position", EditorStyles.boldLabel);
            // End horizontal layout.
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < spawnPoints.arraySize; i++)
            {
                EditorGUILayout.PropertyField(spawnPoints.GetArrayElementAtIndex(i), GUIContent.none);
            }
            //  >>>



            // Apply.
            serializedObject.ApplyModifiedProperties();
        }
    }
}
