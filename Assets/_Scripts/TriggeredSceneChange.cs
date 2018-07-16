using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class TriggeredSceneChange : MonoBehaviour
    {
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
            if (col.gameObject.tag.Equals("Player"))
            {
                if (DeactivateAfterTrigger)
                {
                    gameObject.SetActive(false);
                }
                UnityEngine.SceneManagement.SceneManager.LoadScene("Cutscene");
            }
        }
    }
}
