using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class LockedDoorController : MonoBehaviour
    {
        [Tooltip("The itemID of the necessary object to go through the door.")]
        public int keyID;

        public bool RemoveItemAfterUse;
        public string scene;
        public string exitID;

        public bool TriggerSceneIfLocked;
        public string LockedScene;
        
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
            // If it's the player
            if (col.gameObject.CompareTag("Player"))
            {
                // If the player possesses the key
                if (Grid.inventory.CheckIfItemInInventory(keyID))
                {
                    Debug.Log("Scene change!");
                    if (RemoveItemAfterUse)
                    {
                        Grid.inventory.RemoveItem(keyID);
                    }
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
}
