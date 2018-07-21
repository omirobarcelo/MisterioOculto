using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class ResetPlayerPrefs : MonoBehaviour
    {
        private void Awake()
        {
            PlayerPrefs.DeleteAll();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
