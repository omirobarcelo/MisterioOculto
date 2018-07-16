using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shoguneko
{
    public class ScrollViewControl : MonoBehaviour
    {
        RectTransform rect;
        //public Scrollbar vertical;

        // Use this for initialization
        void Start()
        {
            rect = GetComponent<RectTransform>();
            //vertical.Select();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Debug.Log(rect.position.y);
                Vector3 tmp = rect.position;
                tmp += new Vector3(0f, 10f, 0f);
                rect.position = tmp;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Vector3 tmp = rect.position;
                tmp += new Vector3(0f, -10f, 0f);
                rect.position = tmp;
            }
        }
    }
}
