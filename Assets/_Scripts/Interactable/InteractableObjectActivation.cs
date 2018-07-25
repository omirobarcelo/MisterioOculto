using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class InteractableObjectActivation : MonoBehaviour, IInteractable
    {
        public GameObject theObject;

        // Use this for initialization
        void Start()
        {
            theObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Interact()
        {
            if (Input.GetKeyDown(Grid.setup.GetInteractionKey()))
            {
                if (theObject.activeInHierarchy)
                {
                    Grid.setup.player.CanMove = true;
                    Grid.setup.player.CanOpenInventory = true;
                }
                else
                {
                    Grid.setup.player.CanMove = false;
                    Grid.setup.player.CanOpenInventory = false;
                }
                theObject.SetActive(!theObject.activeInHierarchy);
            }
        }
    }
}
