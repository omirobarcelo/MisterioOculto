using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace Shoguneko
{
    public class PlayerSpawner : MonoBehaviour
    {
        [Inject(InjectFrom.Anywhere)]
        public PlayerManager _player;
        public SpawnPoint[] spawnPoints;
        private Dictionary<string, Transform> dicSpawnPoints;

        private void Awake()
        {
            dicSpawnPoints = new Dictionary<string, Transform>();
            foreach (var sp in spawnPoints)
            {
                dicSpawnPoints.Add(sp.id, sp.position);
            }
        }

        // Use this for initialization
        void Start()
        {
            //dicSpawnPoints = new Dictionary<string, Transform>();
            //foreach (var sp in spawnPoints)
            //{
            //    dicSpawnPoints.Add(sp.id, sp.position);
            //}
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //Debug.Log("first");
            string prevSceneExitID = PlayerPrefs.GetString(Grid.helper.EXIT_ID);
            Transform pos = dicSpawnPoints[prevSceneExitID];
            _player.characterEntity.transform.position = pos.position;
            _player.characterEntity.SetActive(true);
            GameObject vcam = GameObject.FindWithTag("VCam");
            vcam.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = _player.characterEntity.transform;
            // Explicit call because needs to be executed after this OnSceneLoaded 
            // and we cannot determine the order of execution 
            Grid.setup.setLoadedLevel();
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    [System.Serializable]
    public class SpawnPoint
    {
        public string id;
        public Transform position;
    }

    //[CustomPropertyDrawer(typeof(SpawnPoint))]
    //public class SpawnPointDrawer : PropertyDrawer
    //{

    //    // Draw the property inside the given rect
    //    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //    {
    //        // Using BeginProperty / EndProperty on the parent property means that
    //        // prefab override logic works on the entire property.
    //        EditorGUI.BeginProperty(position, label, property);

    //        // Draw label
    //        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

    //        // Don't make child fields be indented
    //        var indent = EditorGUI.indentLevel;
    //        EditorGUI.indentLevel = 0;

    //        // Calculate rects
    //        Rect idRect = new Rect(position.x, position.y, Screen.width / 2.65f, position.height);
    //        Rect posRect = new Rect(position.x + Screen.width / 2.45f, position.y, Screen.width / 2f, position.height);

    //        // Draw fields - passs GUIContent.none to each so they are drawn without labels
    //        EditorGUI.PropertyField(idRect, property.FindPropertyRelative("id"), GUIContent.none);
    //        EditorGUI.PropertyField(posRect, property.FindPropertyRelative("position"), GUIContent.none);

    //        // Set indent back to what it was
    //        EditorGUI.indentLevel = indent;

    //        EditorGUI.EndProperty();
    //    }
    //}

    //[CustomEditor(typeof(PlayerSpawner))]
    //[CanEditMultipleObjects]
    //public class PlayerSpawner_Editor : Editor
    //{
    //    public override void OnInspectorGUI()
    //    {
    //        // Grab the script.
    //        PlayerSpawner myTarget = (PlayerSpawner)target;
    //        // Set the indentLevel to 0 as default (no indent).
    //        EditorGUI.indentLevel = 0;
    //        // Update
    //        serializedObject.Update();

    //        EditorGUILayout.BeginHorizontal();

    //        EditorGUILayout.BeginVertical();

    //        //  >>> THIS PART RENDERS THE ARRAY
    //        SerializedProperty spawnPoints = this.serializedObject.FindProperty("SpawnPoints");
    //        EditorGUILayout.PropertyField(spawnPoints.FindPropertyRelative("Array.size"));

    //        for (int i = 0; i < spawnPoints.arraySize; i++)
    //        {
    //            EditorGUILayout.PropertyField(spawnPoints.GetArrayElementAtIndex(i), GUIContent.none);
    //        }
    //        //  >>>

    //        EditorGUILayout.EndVertical();

    //        EditorGUILayout.EndHorizontal();

    //        // Apply.
    //        serializedObject.ApplyModifiedProperties();
    //    }
    //}
}
