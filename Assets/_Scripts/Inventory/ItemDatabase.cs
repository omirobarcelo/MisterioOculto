using UnityEngine;
using System.Collections.Generic;
using LitJson;
using System.IO;
//using System;

namespace Shoguneko
{

    public class ItemDatabase : MonoBehaviour
    {
        private string dbDir = "Databases";
        private string itemFile = "Items.json";

        private List<Item> database = new List<Item>();
        private JsonData itemData;


        void Start()
        {
            string[] arr = { Application.dataPath, dbDir, itemFile };
            itemData = JsonMapper.ToObject(File.ReadAllText(string.Join("/", arr)));
            ConstructItemDatabase();
        }

        public Item FetchItemByID(int id)
        {
            for (int i = 0; i < database.Count; i++)
            {
                if (database[i].ID == id)
                {
                    return database[i];
                }
            }
            return null;
        }

        private void ConstructItemDatabase()
        {
            for (int i = 0; i < itemData.Count; i++)
            {
                database.Add(new Item(
                    (int)itemData[i]["id"],
                    itemData[i]["name_en"].ToString(),
                    itemData[i]["type"].ToString(),
                    itemData[i]["description_en"].ToString(),
                    (bool)itemData[i]["stackable"],
                    itemData[i]["slug"].ToString(),
                    itemData[i]["pickupsound"].ToString(),
                    itemData[i]["usedsound"].ToString()
                ));
            }
        }
    }

    public class Item
    {
        public enum TYPE { Note, KeyItem };

        public int ID { get; set; }
        public string Name_en { get; set; }
        public TYPE Type { get; set; }
        public string Description_en { get; set; }
        public bool Stackable { get; set; }
        public string Slug { get; set; }
        public Sprite SpriteImage { get; set; }
        public AudioClip PickUpSound { get; set; }
        public AudioClip UsedSound { get; set; }

        public Item(int id, string name_en, string type, string description_en, bool stackable, string slug, string pickupSound, string usedSound)
        {
            this.ID = id;
            this.Name_en = name_en;
            this.Type = (TYPE) System.Enum.Parse(typeof(TYPE), type);
            this.Description_en = description_en;
            this.Stackable = stackable;
            this.Slug = slug;
            this.SpriteImage = Grid.setup.GetSprite(slug);
            this.PickUpSound = Resources.Load<AudioClip>("Sounds/" + pickupSound);
            this.UsedSound = Resources.Load<AudioClip>("Sounds/" + usedSound);
        }

        public Item()
        {
            this.ID = -1;
        }
    }
}
