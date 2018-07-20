using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class SceneChangeConditional : MonoBehaviour
    {
        public string scene;
        public string exitID;

        [Tooltip("The scene where it leads changes depending on some condition.")]
        public bool Conditional;
        [System.Serializable]
        public struct Condition
        {
            [Tooltip("The PlayerPrefs' key to check.")]
            public string key;
            [Tooltip("The scene to change if the condition is true.")]
            public string scene;
            [Tooltip("The exit ID of the conditional scene.")]
            public string exitID;
        }
        public Condition[] Conditions;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            // Check that it's the player
            if (col.gameObject.CompareTag("Player"))
            {
                if (Conditional)
                {
                    foreach (var cond in Conditions)
                    {
                        if (!string.IsNullOrEmpty(PlayerPrefs.GetString(cond.key, null)))
                        {
                            // If scene is null or empty, means that we don't want to activate
                            // a scene change with this condition
                            if (string.IsNullOrEmpty(cond.scene))
                            {
                                return;
                            }
                            Grid.helper.ChangeScene(cond.scene, cond.exitID);
                            break;
                        }
                    }
                }
                // If it isn't conditional or no condition is true, change to default scene
                Grid.helper.ChangeScene(scene, exitID);
                //Debug.Log(exitID);
            }
        }
    }
}
