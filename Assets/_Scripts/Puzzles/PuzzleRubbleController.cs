using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class PuzzleRubbleController : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Grid.helper.ChangeScene("RubbleRoom1", "init");
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                //PlayerPrefs.DeleteKey("kitchen_mid");
                PlayerPrefs.SetString("rubble", "true");
                Grid.helper.ChangeScene("RubbleRoom2", "init");
            }
        }
    }
}
