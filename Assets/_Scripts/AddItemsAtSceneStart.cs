using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shoguneko
{
    public class AddItemsAtSceneStart : MonoBehaviour
    {
        public int[] IDs;

        Text text;

        // Use this for initialization
        void Start()
        {
            text = GetComponentInChildren<Text>();

            // TODO language
            string obtained = "You obtained ";
            foreach (var id in IDs)
            {
                Grid.inventory.AddItem(id, 1);

                // TODO language
                string itemName = Grid.itemDataBase.FetchItemByID(id).Name_en;
                obtained += itemName + ", ";
            }

            if (text != null)
            {
                obtained = obtained.Substring(0, obtained.Length - 2);
                obtained += ".";

                text.text = obtained;
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
