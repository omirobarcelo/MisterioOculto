using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class ButtonSceneChange : MonoBehaviour
    {

        public void StartGame()
        {
            Grid.helper.ChangeScene("Room2_1", "init");
            //UnityEngine.SceneManagement.SceneManager.LoadScene("Cutscene1");
        }

        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.Return))
        //    {
        //        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        //        foreach (GameObject b in buttons)
        //        {
        //            Debug.Log(b);
        //        }
        //    }
        //}
    }
}
