using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shoguneko
{
    public class ItemSlot : MonoBehaviour
    {
        readonly Color normal = new Color(255 / 255.0f, 255 / 255.0f, 255 / 255.0f, 192 / 255.0f);
        readonly Color selected = new Color(255 / 255.0f, 255 / 255.0f, 0 / 255.0f, 255 / 255.0f);

        [ReadOnlyAttribute]
        public int slotNumber;
        private Inventory inv;

        public Image img;

        private void Awake()
        {
            //inv = GetComponentInParent<Inventory>();
            img = GetComponent<Image>();
        }

        // Use this for initialization
        void Start()
        {
            inv = GetComponentInParent<Inventory>();
            //img = gameObject.GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Select()
        {
            img.color = selected;
        }

        public void Unselect()
        {
            img.color = normal;
        }
    }
}
