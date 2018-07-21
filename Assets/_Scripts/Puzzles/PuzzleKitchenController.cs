using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class PuzzleKitchenController : MonoBehaviour
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
                if (string.IsNullOrEmpty(PlayerPrefs.GetString("kitchen_mid", null)))
                {
                    Grid.helper.ChangeScene("Kitchen1", "kitchen_cave_01");
                }
                else
                {
                    Grid.helper.ChangeScene("KitchenMid", "kitchen_cave_01");
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerPrefs.DeleteKey("kitchen_mid");
                PlayerPrefs.SetString("kitchen_2", "true");
                Grid.helper.ChangeScene("Cave1", "kitchen_cave_01");
            }
        }
    }
}
