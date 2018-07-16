using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shoguneko
{
    public class InfoDialogueBoxController : MonoBehaviour
    {

        public GameObject canvas;
        public Text text;

        Inventory _inv;

        // Use this for initialization
        void Start()
        {
            _inv = FindObjectOfType<Inventory>();
            _inv.blocked = true;
            Grid.setup.FreezePlayer();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(Grid.setup.GetInteractionKey()))
            {
                _inv.blocked = false;
                Grid.setup.UnfreezePlayer();
                Destroy(canvas);
            }
        }
    }
}
