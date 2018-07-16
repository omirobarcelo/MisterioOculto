using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shoguneko
{
    
    public class NoteController : MonoBehaviour
    {
        [Inject(InjectFrom.Anywhere)]
        public Inventory _inv;
        //[Inject(InjectFrom.Anywhere)]
        //public PlayerManager _player;

        public GameObject canvas;
        public Text text;

        RectTransform rect;

        // Use this for initialization
        void Start()
        {
            rect = GetComponent<RectTransform>();
            //_inv.blocked = true;
            _inv = FindObjectOfType<Inventory>();
            _inv.blocked = true;
        }

        // Update is called once per frame
        void Update()
        {
            // Keep the inventory blocked, since in Start it's still null
            //_inv.blocked = true;
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
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
            else if (Input.GetKeyDown(Grid.setup.GetInteractionKey()))
            {
                _inv.blocked = false;
                Destroy(canvas);
            }
        }

        public void SetText(string str)
        {
            text.text = str;
        }
    }
}
