using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class ConditionalSceneChange : MonoBehaviour
    {
        [Tooltip("The default scene to change.")]
        public string scene;
        [Tooltip("The exit ID for the default scene.")]
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

        public void ChangeScene()
        {
            if (Conditional)
            {
                foreach (var cond in Conditions)
                {
                    //Debug.Log(PlayerPrefs.GetString(cond.key));
                    if (!string.IsNullOrEmpty(PlayerPrefs.GetString(cond.key, null)))
                    {
                        //Debug.Log("1 : " + cond.scene);
                        // If scene is null or empty, means that we don't want to activate
                        // a scene change with this condition
                        if (string.IsNullOrEmpty(cond.scene))
                        {
                            return;
                        }
                        //Debug.Log("2 : " + cond.scene);
                        Grid.helper.ChangeScene(cond.scene, cond.exitID);
                        return;
                    }
                }
            }
            // If it isn't conditional or no condition is true, change to default scene
            // unless the default scene is not to change
            if (string.IsNullOrEmpty(scene))
            {
                return;
            }
            Grid.helper.ChangeScene(scene, exitID);
            //Debug.Log(exitID);
        }
    }
}
