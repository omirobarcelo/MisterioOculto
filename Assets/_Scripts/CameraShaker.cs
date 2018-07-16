using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Shoguneko
{
    public class CameraShaker : MonoBehaviour
    {

        public Camera shake;

        // Use this for initialization
        void Start()
        {
            shake.DOShakePosition(3);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

