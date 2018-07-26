using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class PuzzleRoom1Controller : MonoBehaviour
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
                Grid.helper.ChangeScene("Room1_1", "init");
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                //PlayerPrefs.DeleteKey("kitchen_mid");
                PlayerPrefs.SetString("room1", "true");
                Grid.helper.ChangeScene("Room1_2", "init");
            }
        }
    }
}
