using UnityEngine;
using System.Collections;

namespace Shoguneko
{

    public class DontDestroyOnLoad : MonoBehaviour
    {

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
