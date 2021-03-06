﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class ItemData : MonoBehaviour
    {

        [ReadOnlyAttribute]
        public Item item;
        [ReadOnlyAttribute]
        public int amount;
        [ReadOnlyAttribute]
        public int slotNumber;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Use()
        {
            if (item.UsedSound != null)
            {
                Grid.soundManager.PlaySound(item.UsedSound);
            }
            // TODO check language to select description
            switch (item.Type)
            {
                case Item.TYPE.Note:
                    {
                        //Debug.Log(item.Description_en);

                        GameObject canvasNote = Instantiate(Grid.setup.GetGameObjectPrefab("CanvasNote"));
                        canvasNote.GetComponentInChildren<UnityEngine.UI.Text>().text = item.Description_en;
                        break;
                    }

                case Item.TYPE.ImageNote:
                    {
                        //Debug.Log(item.Description_en);

                        GameObject canvasNote = Instantiate(Grid.setup.GetGameObjectPrefab("CanvasNoteImage"));
                        canvasNote.GetComponentsInChildren<UnityEngine.UI.Image>()[1].sprite = Grid.setup.GetSprite(item.Description_en);
                        break;
                    }

                case Item.TYPE.KeyItem:
                    {
                        //Debug.Log(item.Description_en);
                        GameObject canvasKeyItem = Instantiate(Grid.setup.GetGameObjectPrefab("CanvasInfoDialog"));
                        canvasKeyItem.GetComponentInChildren<UnityEngine.UI.Text>().text = item.Description_en;
                        break;
                    }
            }
        }
    }
}
