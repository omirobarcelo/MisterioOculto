using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class PuzzleFuseboxController : MonoBehaviour
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
                Grid.helper.ChangeScene("Outside3", "init");
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                Grid.inventory.RemoveItem(1);
                Grid.helper.ChangeScene("Outside4", "init");
            }
        }
    }
}
