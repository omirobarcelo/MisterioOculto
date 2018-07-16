using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class SceneChangeExitPoint : MonoBehaviour
    {
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
            // Check that it's the player
            if (col.gameObject.CompareTag("Player"))
            {
                Grid.helper.ChangeScene(scene, exitID);
            }
        }
    }
}
