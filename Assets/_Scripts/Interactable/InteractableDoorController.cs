using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class InteractableDoorController : MonoBehaviour, IInteractable
    {
        [Tooltip("The itemID of the necessary objects to go through the door.")]
        public int[] keyID;

        public bool RemoveItemAfterUse;
        public string scene;
        public string exitID;

        public bool TriggerSceneIfLocked;
        public string LockedScene;

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

        public void Interact()
        {
            // If one key is missing, hasAllKeys will become false
            bool hasAllKeys = true;
            foreach (var id in keyID)
            {
                hasAllKeys &= Grid.inventory.CheckIfItemInInventory(id);
            }
            // If the player possesses the keys
            if (hasAllKeys)
            {
                Debug.Log("Scene change!");
                if (RemoveItemAfterUse)
                {
                    foreach (var id in keyID)
                    {
                        Grid.inventory.RemoveItem(id);
                    }
                }
                if (Conditional)
                {
                    foreach (var cond in Conditions)
                    {
                        if (!string.IsNullOrEmpty(PlayerPrefs.GetString(cond.key, null)))
                        {
                            Grid.helper.ChangeScene(cond.scene, cond.exitID);
                            break;
                        }
                    }
                }
                // If it isn't conditional or no condition is true, change to default scene
                Grid.helper.ChangeScene(scene, exitID);
            }
            else
            {
                if (TriggerSceneIfLocked)
                {
                    Grid.helper.ChangeScene(LockedScene);
                }
                else
                {
                    GameObject canvasInfo = Instantiate(Grid.setup.GetGameObjectPrefab("CanvasInfoDialog"));
                    // TODO check language
                    canvasInfo.GetComponentInChildren<UnityEngine.UI.Text>().text = "The door is locked.";
                }
            }
        }
    }
}
