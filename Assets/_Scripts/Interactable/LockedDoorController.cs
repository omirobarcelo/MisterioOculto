using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class LockedDoorController : MonoBehaviour
    {
        [Tooltip("The itemID of the necessary object to go through the door")]
        public int keyID;

        public string scene;
        public string exitID;
        
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
            if (col.gameObject.tag.Equals("Player"))
            {
                // If the player possesses the key
                if (Grid.inventory.CheckIfItemInInventory(keyID))
                {
                    Debug.Log("Scene change!");
                    Grid.helper.ChangeScene(scene, exitID);
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
