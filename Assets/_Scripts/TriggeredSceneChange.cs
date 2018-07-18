using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class TriggeredSceneChange : MonoBehaviour
    {
        public string scene;
        public bool DeactivateAfterTrigger;

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
                if (DeactivateAfterTrigger)
                {
                    gameObject.SetActive(false);
                }
                Grid.helper.ChangeScene(scene);
            }
        }
    }
}
