using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class InteractableSceneChange : MonoBehaviour, IInteractable
    {
        public string scene;
        [Tooltip("The ID of the location to spawn in the next scene.")]
        public string exitID;

        [Tooltip("If true, we require an item to activate the scene change.")]
        public bool RequiresItem;
        [Tooltip("The item ID of the necessary object to go activate the scene change.")]
        public int itemID;
        [Tooltip("The message to display if the player does not have the item.")]
        public string message;


        [Tooltip("If true, the required item will be removed after using it.")]
        public bool RemoveItemAfterUse;

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
            if (RequiresItem)
            {
                // If the player possesses the required item
                if (!Grid.inventory.CheckIfItemInInventory(itemID))
                {
                    GameObject canvasInfo = Instantiate(Grid.setup.GetGameObjectPrefab("CanvasInfoDialog"));
                    // TODO check language
                    canvasInfo.GetComponentInChildren<UnityEngine.UI.Text>().text = message;
                    return;
                }
            }

            // If we reach here, we don't need an item or we have it
            if (RemoveItemAfterUse)
            {
                Grid.inventory.RemoveItem(itemID);
            }
            //Grid.helper.ChangeScene(scene, exitID);
            Debug.Log("Scene change!");
        }
    }
}
